/*******************************************************************************
 *
 * Pentaho IoT
 *
 * Copyright (C) 2016 by Pentaho : http://www.pentaho.com
 *
 *******************************************************************************
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 ******************************************************************************/

package org.pentaho.di.trans.steps.pentahomqttsubscriber;

import org.eclipse.paho.client.mqttv3.IMqttDeliveryToken;
import org.eclipse.paho.client.mqttv3.MqttCallback;
import org.eclipse.paho.client.mqttv3.MqttClient;
import org.eclipse.paho.client.mqttv3.MqttConnectOptions;
import org.eclipse.paho.client.mqttv3.MqttException;
import org.eclipse.paho.client.mqttv3.MqttMessage;
//Adding Persistence store (for CleanSession=false)
import org.eclipse.paho.client.mqttv3.MqttClientPersistence;
import org.eclipse.paho.client.mqttv3.persist.MemoryPersistence;
import org.eclipse.paho.client.mqttv3.persist.MqttDefaultFilePersistence;
//-----
import org.pentaho.di.core.Const;
import org.pentaho.di.core.exception.KettleException;
import org.pentaho.di.core.exception.KettlePluginException;
import org.pentaho.di.core.row.RowDataUtil;
import org.pentaho.di.core.row.RowMeta;
import org.pentaho.di.core.row.ValueMetaInterface;
import org.pentaho.di.core.row.value.ValueMetaFactory;
import org.pentaho.di.i18n.BaseMessages;
import org.pentaho.di.trans.Trans;
import org.pentaho.di.trans.TransMeta;
import org.pentaho.di.trans.step.BaseStep;
import org.pentaho.di.trans.step.StepDataInterface;
import org.pentaho.di.trans.step.StepInterface;
import org.pentaho.di.trans.step.StepMeta;
import org.pentaho.di.trans.step.StepMetaInterface;
import org.pentaho.di.trans.steps.pentahomqttpublisher.MQTTPublisherMeta;
import org.pentaho.mqtt.SSLSocketFactoryGenerator;

import java.io.ByteArrayInputStream;
import java.io.ObjectInputStream;
import java.nio.ByteBuffer;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

/**
 * MQTT subscriber step
 *
 * @author Mark Hall (mhall{[at]}pentaho{[dot]}com)
 * @version $Revision: $
 */
public class MQTTSubscriber extends BaseStep implements StepInterface {

  protected boolean m_reconnectFailed;
  protected boolean m_isInit=false; // Added this for persistence. We need to be sure init is done before processing unconsumed messages at broker reconnection

  public MQTTSubscriber( StepMeta stepMeta, StepDataInterface stepDataInterface, int copyNr, TransMeta transMeta,
      Trans trans ) {
    super( stepMeta, stepDataInterface, copyNr, transMeta, trans );
  }

  public boolean processRow( StepMetaInterface smi, StepDataInterface sdi ) throws KettleException {

    if ( !isStopped() ) {

      if ( first ) {
        first = false;
        logBasic("process row first");
        if ( ( (MQTTSubscriberData) sdi ).m_executionDuration > 0 ) {
          ( (MQTTSubscriberData) sdi ).m_startTime = new Date();
        }

        ( (MQTTSubscriberData) sdi ).m_outputRowMeta = new RowMeta();

        smi.getFields( ( (MQTTSubscriberData) sdi ).m_outputRowMeta, getStepname(), null, null, getTransMeta(), null,
            null );
      }

      if ( m_reconnectFailed ) {
        logError( BaseMessages.getString( MQTTPublisherMeta.PKG, "MQTTClientStep.Error.ReconnectFailed" ) );
        setStopped( true );
        return false;
      }

      if ( ( (MQTTSubscriberData) sdi ).m_executionDuration > 0 ) {
        if ( System.currentTimeMillis() - ( (MQTTSubscriberData) sdi ).m_startTime.getTime()
            > ( (MQTTSubscriberData) sdi ).m_executionDuration * 1000 ) {
          setOutputDone();
          return false;
        }
      }

      return true;
    } else {
      setStopped( true );
      return false;
    }
  }

  protected synchronized void shutdown( MQTTSubscriberData data ) {
    if ( data.m_client != null ) {
      try {
        if ( data.m_client.isConnected() ) {
          logBasic( "Disconnecting from MQTT broker" );
          data.m_client.disconnect();
        }
		data.m_client.close();
        data.m_client = null;
      } catch ( MqttException e ) {
        logError( BaseMessages.getString( MQTTPublisherMeta.PKG, "MQTTClientStep.ErrorClosingMQTTClient.Message" ), e );
      }
    }
  }

  public boolean init( StepMetaInterface smi, StepDataInterface sdi ) {
    if ( super.init( smi, sdi ) ) {
      try {
        configureConnection( (MQTTSubscriberMeta) smi, (MQTTSubscriberData) sdi );
        String runFor = ( (MQTTSubscriberMeta) smi ).getExecuteForDuration();
        try {

		  ( (MQTTSubscriberData) sdi ).m_executionDuration = Long.parseLong( runFor );
        } catch ( NumberFormatException e ) {
          logError( e.getMessage(), e );
          return false;
        }
      } catch ( KettleException e ) {
        logError( e.getMessage(), e );
        return false;
      }

      try {
        ValueMetaInterface
            messageMeta =
            ValueMetaFactory.createValueMeta( "Message",
                ValueMetaFactory.getIdForValueMeta( ( (MQTTSubscriberMeta) smi ).getMessageType() ) );
        if ( messageMeta.isSerializableType() && !( (MQTTSubscriberMeta) smi ).getAllowReadMessageOfTypeObject() ) {
          logError( BaseMessages
              .getString( MQTTPublisherMeta.PKG, "MQTTClientStep.Error.MessageTypeObjectButObjectNotAllowed" ) );
          return false;
        }
      } catch ( KettlePluginException e ) {
        logError( e.getMessage(), e );
        return false;
      }
      m_isInit=true;
      return true;
    }

    return false;
  }

  public void dispose( StepMetaInterface smi, StepDataInterface sdi ) {
    MQTTSubscriberData data = (MQTTSubscriberData) sdi;
    shutdown( data );
    super.dispose( smi, sdi );
  }

  public void stopRunning( StepMetaInterface smi, StepDataInterface sdi ) throws KettleException {

    MQTTSubscriberData data = (MQTTSubscriberData) sdi;
    shutdown( data );
    super.stopRunning( smi, sdi );
  }
  
  //Adding creation of persistence store 
  public MqttClientPersistence getPersistence(String path) {
        if (path==null || path.length()==0) {
            logBasic( BaseMessages.getString( MQTTPublisherMeta.PKG, "MQTTClientStep.Message.MemoryPersitence" ) );
            return new MemoryPersistence();
        } else {
			logBasic( BaseMessages.getString( MQTTPublisherMeta.PKG,"MQTTClientStep.Message.PathPersitence",path));
            return new MqttDefaultFilePersistence(path);
        }
    }

  protected void configureConnection( MQTTSubscriberMeta meta, MQTTSubscriberData data ) throws KettleException {
    if ( data.m_client == null ) {
	  String broker = environmentSubstitute( meta.getBroker() );
      if ( Const.isEmpty( broker ) ) {
        throw new KettleException(
            BaseMessages.getString( MQTTPublisherMeta.PKG, "MQTTClientStep.Error.NoBrokerURL" ) );
      }
      String clientId = environmentSubstitute( meta.getClientId() );
      if ( Const.isEmpty( clientId ) ) {
        throw new KettleException( BaseMessages.getString( MQTTPublisherMeta.PKG, "MQTTClientStep.Error.NoClientID" ) );
      }
      List<String> topics = meta.getTopics();
      if ( topics == null || topics.size() == 0 ) {
        throw new KettleException( "No topic(s) to subscribe to provided" );
      }
      List<String> resolvedTopics = new ArrayList<>();
      for ( String topic : topics ) {
        resolvedTopics.add( environmentSubstitute( topic ) );
      }

      String qosS = environmentSubstitute( meta.getQoS() );
      int qos = 0;
      if ( !Const.isEmpty( qosS ) ) {
        try {
          qos = Integer.parseInt( qosS );
        } catch ( NumberFormatException e ) {
          // quietly ignore
        }
      }
      int[] qoss = new int[resolvedTopics.size()];
      for ( int i = 0; i < qoss.length; i++ ) {
        qoss[i] = qos;
      }
      try {
		Boolean isCleanSession=meta.isCleanSession();
		//Handling Persistence Store if cleanSession=false
		MqttClientPersistence persistenceStore=null;
		// Handling persistenceStore in memory or on disk
		if(isCleanSession==false){
				persistenceStore=getPersistence(meta.getPath());		
		}
		//
        if(!isCleanSession){
			data.m_client = new MqttClient( broker, clientId, persistenceStore );
		}
		else data.m_client = new MqttClient( broker, clientId);
        MqttConnectOptions connectOptions = new MqttConnectOptions();
        if ( meta.isRequiresAuth() ) {
          connectOptions.setUserName( environmentSubstitute( meta.getUsername() ) );
          connectOptions.setPassword( environmentSubstitute( meta.getPassword() ).toCharArray() );
        }
        if ( broker.startsWith( "ssl:" ) || broker.startsWith( "wss:" ) ) {
          connectOptions.setSocketFactory( SSLSocketFactoryGenerator
              .getSocketFactory( environmentSubstitute( meta.getSSLCaFile() ),
                  environmentSubstitute( meta.getSSLCertFile() ), environmentSubstitute( meta.getSSLKeyFile() ),
                  environmentSubstitute( meta.getSSLKeyFilePass() ) ) );
        }
		
		
		connectOptions.setCleanSession( isCleanSession); //Adding Clean Session option true=Clean session, false=persistence management
        
		logBasic( BaseMessages
            .getString( MQTTPublisherMeta.PKG, "MQTTClientStep.CreateMQTTClient.MessageEarly", broker, clientId,Boolean.toString(isCleanSession) ) );

        String timeout = environmentSubstitute( meta.getTimeout() );
        String keepAlive = environmentSubstitute( meta.getKeepAliveInterval() );
        try {
          connectOptions.setConnectionTimeout( Integer.parseInt( timeout ) );
        } catch ( NumberFormatException e ) {
          throw new KettleException(
              BaseMessages.getString( MQTTPublisherMeta.PKG, "MQTTClientStep.WrongTimeoutValue.Message", timeout ), e );
        }

        try {
          connectOptions.setKeepAliveInterval( Integer.parseInt( keepAlive ) );
        } catch ( NumberFormatException e ) {
          throw new KettleException(
              BaseMessages.getString( MQTTPublisherMeta.PKG, "MQTTClientStep.WrongKeepAliveValue.Message", keepAlive ),
              e );
        }

        logBasic( BaseMessages
            .getString( MQTTPublisherMeta.PKG, "MQTTClientStep.CreateMQTTClient.Message", broker, clientId,Boolean.toString(meta.isCleanSession()) ) );
        data.m_client.setCallback( new SubscriberCallback( data, meta ) );
        data.m_client.connect( connectOptions );
        if(data.m_client.isConnected()){ //adding this in case cleansession=flse otherwise exception
			data.m_client.subscribe( resolvedTopics.toArray( new String[resolvedTopics.size()] ), qoss );
					logBasic(BaseMessages.getString( MQTTPublisherMeta.PKG,"MQTTClientStep.DebugMessage",4));
		}

      } catch ( Exception e ) {
        throw new KettleException(
            BaseMessages.getString( MQTTPublisherMeta.PKG, "MQTTClientStep.ErrorCreateMQTTClient.Message", broker ),
            e );
      }
    } else {logBasic( BaseMessages.getString( MQTTPublisherMeta.PKG,"MQTTClientStep.DebugMessage","data cleint not null"));}
  }

  protected class SubscriberCallback implements MqttCallback {

    protected MQTTSubscriberData m_data;
    protected MQTTSubscriberMeta m_meta;
    protected ValueMetaInterface m_messageValueMeta;

    public SubscriberCallback( MQTTSubscriberData data, MQTTSubscriberMeta meta ) throws KettlePluginException {
      m_data = data;
      m_meta = meta;
      logBasic(BaseMessages.getString( MQTTPublisherMeta.PKG,"MQTTClientStep.DebugMessage","message callback"));
      m_messageValueMeta =
          ValueMetaFactory.createValueMeta( "Message", ValueMetaFactory.getIdForValueMeta( m_meta.getMessageType() ) );
    }

    @Override public void connectionLost( Throwable throwable ) {
      // connection retry logic here
      shutdown( m_data );
      logBasic( BaseMessages
          .getString( MQTTPublisherMeta.PKG, "MQTTClientStep.Log.LostConnectionToBroker", throwable.getMessage() ) );
      logBasic( BaseMessages.getString( MQTTPublisherMeta.PKG, "MQTTClientStep.Log.AttemptingToReconnect" ) );
      try {
		if(!m_reconnectFailed){
		   configureConnection( m_meta, m_data ); //to debug, only one reconnection try do not reconnect a thousand times
		}
      } catch ( KettleException e ) {
        logError( e.getMessage(), e );
        m_reconnectFailed = true;
      }
    }

    @Override public void messageArrived( String topic, MqttMessage mqttMessage ) throws Exception {
      
	  
	  if(m_data.m_outputRowMeta==null) // if clean session=false, we might reicieve messages before processrows has been called if the broker sends back all unconsumed mesages
			{
				while(!m_isInit)
				{
				//Wait until assync init done
				}			
			}
		
		  

	  Object[] outRow = RowDataUtil.allocateRowData( m_data.m_outputRowMeta.size() );
      outRow[0] = topic;
      Object converted = null;

      byte[] raw = mqttMessage.getPayload();
      ByteBuffer buff = null;
      switch ( m_messageValueMeta.getType() ) {
        case ValueMetaInterface.TYPE_INTEGER:
          buff = ByteBuffer.wrap( raw );
          outRow[1] = raw.length == 4 ? (long) buff.getInt() : buff.getLong();
          break;
        case ValueMetaInterface.TYPE_STRING:
        case ValueMetaInterface.TYPE_NONE:
          outRow[1] = new String( raw );
          break;
        case ValueMetaInterface.TYPE_NUMBER:
          buff = ByteBuffer.wrap( raw );
          outRow[1] = raw.length == 4 ? (double) buff.getFloat() : buff.getDouble();
          break;
        case ValueMetaInterface.TYPE_DATE:
          buff = ByteBuffer.wrap( raw );
          outRow[1] = new Date( buff.getLong() );
          break;
        case ValueMetaInterface.TYPE_BINARY:
          outRow[1] = raw;
          break;
        case ValueMetaInterface.TYPE_BOOLEAN:
          outRow[1] = raw[0] > 0;
          break;
        case ValueMetaInterface.TYPE_TIMESTAMP:
          buff = ByteBuffer.wrap( raw );
          long time = buff.getLong();
          int nanos = buff.getInt();
          Timestamp t = new Timestamp( time );
          t.setNanos( nanos );
          outRow[1] = t;
          break;
        case ValueMetaInterface.TYPE_SERIALIZABLE:
          ObjectInputStream ois = new ObjectInputStream( new ByteArrayInputStream( raw ) );
          outRow[1] = ois.readObject();
          break;
        default:
          throw new KettleException( "Unhandled type" );
      }
      putRow( m_data.m_outputRowMeta, outRow );
	  
    }

    @Override public void deliveryComplete( IMqttDeliveryToken iMqttDeliveryToken ) {

    }
  }
}

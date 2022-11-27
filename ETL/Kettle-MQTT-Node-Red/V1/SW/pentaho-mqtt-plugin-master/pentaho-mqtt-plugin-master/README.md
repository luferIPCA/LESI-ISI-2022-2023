Pentaho MQTT IoT Plugin
=======================

The Pentaho MQTT Project is a plugin for the Pentaho Kettle engine which provides the ability to subscribe to, and publish to, MQTT topics on a MQTT broker.
A few features are avaiable :
- Keepalive
- Quality Of Service
- Last Will Message
- Retained Message
- Clean Session and persistence
A few additional explainations here : https://www.linkedin.com/pulse/exploring-mqtt-cool-features-pentaho-demo-jean-francois-monteil/

Building
--------
The Pentaho MQTT plugin is built using Maven.

    $ git clone https://github.com/jfmonteil/pentaho-mqtt-plugin
    $ cd pentaho-mqtt-plugin
    $ mvn install

This will produce a plugin archive in the target directory. This archive can then be extracted into your Pentaho Data Integration plugin directory.

License
-------
Licensed under the Apache License, Version 2.0. See LICENSE.txt for more information.

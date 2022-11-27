<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" >

  <xsl:template match="Payments">
    <html>
      <head>
        <title>SIBS payments</title>
        <style type="text/css">
          h1, h2, h3
          {
          font-family:Times New Roman, Helvetica, sans-serif;
          color:#00000;
          }
          
          #payments
          {
          font-family:"Trebuchet MS", Arial, Helvetica, sans-serif;
          width:100%;
          border-collapse:collapse;
          }
          #payments td, #payments th
          {
          font-size:1em;
          border:1px solid #6495ed;
          padding:3px 7px 2px 7px;
          }
          #payments th
          {
          font-size:1.1em;
          text-align:left;
          padding-top:5px;
          padding-bottom:4px;
          background-color:#6495ed;
          color:#ffffff;
          }
          #payments tr.alt td
          {
          color:#000000;
          background-color:#EAF2D3;
          }
        </style>
      </head>
      <body>
        <h1>SIBS Payments
		</h1>
        <br/>
        <br/>
        <h3>
          Company Name:
          <i>
            <xsl:value-of select="ID_Instituicao_Destino"/>
          </i>
        </h3>
        <h3>
          Processing Data:
          <i>
            <xsl:value-of select="Data_Processamento"/>
          </i>
        </h3>
        <h3>
          Currency code:
          <i>
            <xsl:value-of select="Codigo_Moeda"/>
          </i>
        </h3>
        <br/>

        <table id="payments">
          <th>#</th>
          <th>Transaction Time</th>
          <th>Amount</th>
          <th>Local</th>
          <th>Reference</th>
          <xsl:apply-templates select="//Details"/>
        </table>

        <xsl:apply-templates select="//Trailer"/>

      </body>
    </html>
  </xsl:template>

  <xsl:template match="Detail">
    <tr class="alt">
      <td>
        <xsl:number/>
      </td>
      <td>
        <xsl:value-of select="Data_Transaccao"/>
      </td>
      <td>
        <xsl:value-of select="Montante_Pago" />
      </td>
      <td>
        <xsl:value-of select="Localidade_Terminal"/>
      </td>
      <td>
        <xsl:value-of select='Referencia_Pagamento'/>
      </td>
    </tr>

  </xsl:template>

  <xsl:template match="Trailer">
    <h5>
      Number of records:
      <i>
        <xsl:value-of select="Numero_Registos"/>
      </i>
    </h5>
    <h5>
      Total amount:
      <i>
        <xsl:value-of select="Total_Transaccoes"/>
      </i>
    </h5>
    <h5>
      Total IVA:
      <i>
        <xsl:value-of select="IVA"/>
      </i>
    </h5>
    <h5>
      Total Taxes:
      <i>
        <xsl:value-of select="Total_Tarifa"/>
      </i>
    </h5>
  </xsl:template>

</xsl:stylesheet>

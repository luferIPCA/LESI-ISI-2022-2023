/*
 * RESTful Client implementado em C#
 * 
 * RECEITA: Para aceder a recursos Web:
 * 1 - URI (URL+Parametros) - HttpUtility
 * 2 - HttpWebRequest
 * 3 - HttpWebResponse
 * 
 * Mais:
 *  Parsing XML     - https://msdn.microsoft.com/en-us/library/hh534080.aspx
 *  Parsing JSON    - https://msdn.microsoft.com/en-us/library/hh674188.aspx
 *  
 *  JSON serialization in .NET
 *  --------------------------
 *  System.Runtime.Serialization
 *  System.ServiceModel.Web
 *
 *  Exemplos REST Services:
 *  Groupkt: http://services.groupkt.com/country/get/iso2code/{SIGLA}
 *  Groupkt: http://services.groupkt.com/country/get/all
 *  VER: http://services.groupkt.com/post/c9b0ccb9/restful-webservices-to-get-and-search-countries.htm
 *
 *  Ver http://json2csharp.com/
 *  
 *  Outros:
 *  Explorar
 *      http://samples.openweathermap.org/data/2.5/forecast?q=M%C3%BCnchen,DE&appid=b1b15e88fa797225412429c1c50c122a1
 *      http://api.openweathermap.org/data/2.5/weather?q=CITYNAME&mode=MODE&appid=API
 *      APIKEY: b4e56d454f9ddc1a5b66102f99f28fa2
 * by lufer
 * LESI-EST-IPCA
 * */


using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;                            //NameSpace referência
using System.Runtime.Serialization;         //DataContract
using System.Runtime.Serialization.Json;
//Adicionar COM System.Runtime.Serialization em "Project Properties"
using System.Text;
using System.Web;                           //NameSpace referência - HttpUtility
using System.Xml.Serialization;

namespace RESTWS
{
    class Program
    {
        static void Main(string[] args)
        {
            #region VAR
            //Suportar Pedido e URI
            HttpWebRequest request;
            StringBuilder uri;
            string url;
            string apiKey = "3f1edc0d2175fd7f93109cb6cd21466b";
            string mode = "json";
            #endregion

            #region WEATHER_1
            try
            {
                #region Weather
                //Weather URL
                url = "http://api.openweathermap.org/data/2.5/weather?q=[CITY]&mode=[MODE]&appid=[APIKEY]";

                #region ConstroiURI
                uri = new StringBuilder();
                uri.Append(url);
                uri.Replace("[CITY]", HttpUtility.UrlEncode("Lisbon"));
                uri.Replace("[APIKEY]", apiKey);
                uri.Replace("[MODE]", mode);
                //String.Format();
                #endregion
                

                #region PreparaPedido
                //Prepara e envia pedido
                request = WebRequest.Create(uri.ToString()) as HttpWebRequest;

                // Caso necessite de autenticação no pedido 
                //request.Credentials = new NetworkCredential("username", "password"); 

                // Mais flags
                // request.KeepAlive = false;
                // request.Timeout = 10 * 10000;
                #endregion

                #endregion

                #region EnviaPedidoAnalisaResposta
                //analisa resposta
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)     //via GET
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        string message = String.Format("GET falhou. Recebido HTTP {0}", response.StatusCode);
                        throw new ApplicationException(message);
                    }
                    //Se for JSON

                    //Mostra JSON bruto
                    Console.WriteLine("Recebido:");
                    //NOTA: "GetResponseStream" só pode ser usado uma vez!!!
                    //request: non-buffered stream that can only be read once
                    
                    //Preserva conteudo num stream de memória
                    var copyStream = new MemoryStream();
                    response.GetResponseStream().CopyTo(copyStream);

                    //ou
                    //StreamReader reader = new StreamReader(response.GetResponseStream());
                    //string str = reader.ReadToEnd();
                    //Console.WriteLine(str);

                    //ou 
                    //Console.WriteLine(GetPageAsString(uri.ToString()));

                    if (mode == "json")
                    {
                        //Serializa de JSON para Objecto
                        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Root));
                        copyStream.Position = 0L;               //inicio do stream
                        Root myClass = (Root)jsonSerializer.ReadObject(copyStream);
                        //Ou
                        //Root myClass = (Root)jsonSerializer.ReadObject(response.GetResponseStream());
                    }
                    else //se for XML
                    {           
                        // Converte em "ficheiro" a resposta recebida, em XML   
                        //GetXMLData(response.GetResponseStream());
                        GetXMLData(copyStream);
                    }
                }
                #endregion
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);

            }
            #endregion

            #region WEATHER_2
            
            #endregion
        }

        #region Métodos auxiliares  XML  

        /// <summary>
        /// Método para devolver o conteúdo de uma página em string
        /// </summary>
        /// <param name="address">URI completo</param>
        /// <returns></returns>
        public static string GetPageAsString(String address)
        {
            string result = "";

            // Cria pedido Web 
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            // Analisa a resposta  
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Obtem a Resposta 
                StreamReader reader = new StreamReader(response.GetResponseStream());
                // Lê e converte em string
                result = reader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// Trata XML
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static void GetXMLData(Stream f)
        {
            DataSet ds = new DataSet();
            // converte o Stream num DataSet
            ds.ReadXml(f);
            // Mostra o XML do DataSet
            PrintDataSet(ds);
        }        
        
        /// <summary>
        /// Prepara output de XML
        /// </summary>
        /// <param name="ds">Dataset</param>
        public static void PrintDataSet(DataSet ds)
        {
            //int x = ds.Tables.Count;
            foreach (DataTable table in ds.Tables)
            {
                for (int rn = 0; rn < table.Rows.Count; rn++)
                {
                    for (int cn = 0; cn < table.Columns.Count; cn++)
                    {

                        Console.WriteLine("Row: {0} : Col: {1} <{2}> = {3}", rn, cn, table.Columns[cn].Caption, table.Rows[rn][cn]);
                    }
                }

            }
        }

        #endregion       

        #region Métodos auxiliares_JSON
        /// <summary>
        /// Processa JSON Serializado
        /// </summary>
        /// <param name="r"></param>
        static public void ProcessResponse(Response r)
        {
            int s = r.response.results.Length;  //número de results

            Console.WriteLine("Mostra todos os países");
            for (int i = 0; i < s; i++)
            {
                Result res = (Result)r.response.results[i];
                Console.WriteLine("Nome = " + res.name);
            }
            Console.WriteLine();
        }

        static public void ProcessResponse(Root r)
        {
            double tmax = r.main.temp_max;  
        }

        #endregion 
    }

    #region CONTRACTS_SERIALIZAÇÂO    

    #region CONTRACT_REST
    // Ver http://json2csharp.com/

    #region CONTRACT_GET_ALL_COUNTRY
    [DataContract]
    internal class Response
    {
        [DataMember(Name = "RestResponse")]
        public RestResponse response { get; set; }

    }

    [DataContract]    
    internal class RestResponse
    {
        [DataMember(Name = "messages")]
        public string[] messages { get; set; }
        [DataMember(Name = "result")]
        public Result[] results { get; set; }
    }
   
    [DataContract]
    internal class Result
    {
        [DataMember(Name = "name")]
        public string name { get; set; }
        [DataMember(Name = "alpha2_code")]
        public string alpha2_code { get; set; }
        [DataMember(Name = "alpha3_code")]
        public string alpha3_code { get; set; }
    }
    #endregion

    #region CONTRACT_GET_ONE_COUNTRY

    [DataContract]
    internal class ResponseOne
    {
        [DataMember(Name = "RestResponse")]
        public RestResponseOne response { get; set; }

    }

    [DataContract]
    internal class RestResponseOne
    {
        [DataMember(Name = "messages")]
        public string[] messages { get; set; }
        [DataMember(Name = "result")]
        public Result result { get; set; }
    }
    #endregion

    #endregion

    #region WEATHER_CLASSES
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class Root
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }
    #endregion

    #endregion

    #region SERIALIZARXML

    //https://xmltocsharp.azurewebsites.net

    [XmlRoot(ElementName = "Hotel", Namespace = "http://schemas.datacontract.org/2004/07/")]
    public class Hotel
    {
        [XmlElement(ElementName = "Capacidade", Namespace = "http://schemas.datacontract.org/2004/07/")]
        public string Capacidade { get; set; }
        [XmlElement(ElementName = "Cidade", Namespace = "http://schemas.datacontract.org/2004/07/")]
        public string Cidade { get; set; }
        [XmlElement(ElementName = "Nome", Namespace = "http://schemas.datacontract.org/2004/07/")]
        public string Nome { get; set; }
    }

    [XmlRoot(ElementName = "ArrayOfHotel", Namespace = "http://schemas.datacontract.org/2004/07/")]
    public class ArrayOfHotel
    {
        [XmlElement(ElementName = "Hotel", Namespace = "http://schemas.datacontract.org/2004/07/")]
        public List<Hotel> Hotel { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "i", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string I { get; set; }
    }
    #endregion
}

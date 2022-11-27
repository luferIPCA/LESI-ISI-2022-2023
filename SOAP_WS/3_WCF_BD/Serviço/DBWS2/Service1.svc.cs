/**
 * lufer
 * 
 * WCF Service and MongoDB
 * Ver:
 * https://www.mongodb.com/developer/languages/csharp/transactions-csharp-dotnet/
 * Use MongoDB .NET driver - Extensions Manager
 * https://www.mongodb.com/docs/drivers/csharp/
 * 
 * 1ª ConnectionString
 * 2ª Connetar
 * 3º Preparar query
 * 4º Executar query
 * 5º Tratar resultado
 */

//Mongodb
using MongoDB.Bson;
using MongoDB.Driver;
using System;
//OleDB
using System.Data;
using System.Data.OleDb;
using System.Configuration;

using System.Linq;

namespace WCFBD2
{

    public class Service1 : IService1
    {

        /// <summary>
        /// Get mongodb data
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetData()
        {
            const string MongoDBConnectionString = "mongodb://localhost:27017";
            var client = new MongoClient(MongoDBConnectionString);

            var database = client.GetDatabase("local");
            var collection = database.GetCollection<BsonDocument>("CodigosPostais");

            var firstDocument = collection.Find(new BsonDocument()).FirstOrDefault();

            return firstDocument.ToString();

        }

        /// <summary>
        /// Devolve todos os hoteis de uma determinada cidade
        /// </summary>
        /// <param name="cidade"></param>
        /// <returns></returns>
        public DataSet GetHotelCidade(string cidade)
        {
// *1ª ConnectionString
// *2ª Connetar
// *3º Preparar query
//* 4º Executar query
//* 5º Tratar resultado

           DataSet ds = new DataSet();
            //1 Connection
            string conString = ConfigurationManager.ConnectionStrings["turismoConnectionString"].ConnectionString;
            //string cons = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\temp\turismo.mdb";
            OleDbConnection con = new OleDbConnection(conString);
            //con.ConnectionString = "";

            //2 Query
            string query = "select * from hoteis where cidade=@cidade";

            //3 Adapter
            OleDbDataAdapter da = new OleDbDataAdapter(query, con);
            da.SelectCommand.Parameters.Add("@cidade", OleDbType.VarChar);
            da.SelectCommand.Parameters["@cidade"].Value = cidade;

            //da.SelectCommand.Parameters.AddWithValue("@cidade", cidade);

            //4 Execute

            da.Fill(ds, "Hotel");

            return ds;
        }

        #region OUTRO
        public DataSet GetAllHoteisComCapacidade(int capacidade)
        {
            DataSet ds = new DataSet();

            //1º ConnectionString
            string cs = ConfigurationManager.ConnectionStrings["turismoConnectionString"].ConnectionString;

            //2º OpenConnection
            OleDbConnection con = new OleDbConnection(cs);

            //3º Query Parametrizada
            string q = "SELECT nome, (capacidade-ocupacao) as livres, cidade FROM Hoteis WHERE (capacidade-ocupacao)>@capacidade; ";    //aplicar critério            

            //4º Prepara DataAdapter
            OleDbDataAdapter da = new OleDbDataAdapter(q, con);

            //Instancia parâmetros
            da.SelectCommand.Parameters.Add("@capacidade", OleDbType.Integer);
            da.SelectCommand.Parameters["@capacidade"].Value = capacidade;

            //Executa e preenche o DataSet com os resultados
            da.Fill(ds, "Hoteis2");

            return (ds);
        }

        #endregion
    }


}

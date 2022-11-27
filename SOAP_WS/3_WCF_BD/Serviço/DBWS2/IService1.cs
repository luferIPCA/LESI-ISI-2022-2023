/**
 * 
 * WCF Service and MongoDB
 * Ver:
 * https://www.mongodb.com/developer/languages/csharp/transactions-csharp-dotnet/
 * Use MongoDB .NET driver - Extensions Manager
 * https://www.mongodb.com/docs/drivers/csharp/
 * 
 * Outras BDs
 * 1-Add DataSet
 * 
 * 1ª ConnectionString
 * 2ª Connetar
 * 3º Preparar query
 * 4º Executar query
 * 5º Tratar resultado
 * 
 */
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WCFBD2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData();

        [OperationContract]
        DataSet GetHotelCidade(string cidade);
    }

    public struct Hotel
    {
        public string nome;
        public string cidade;
        public int capacidade;
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}

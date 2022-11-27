/**
 * XML Web Service
 * lufer & Oscar
 * 2022-2023
 **/
using System.Web.Services;

namespace XML_WS.Services
{
    /// <summary>
    /// Summary description for Funcoes
    /// </summary>
    [WebService(Namespace = "http://www.ipca.pt/", Description ="Vários Cálculos...")]
    public class Funcoes : System.Web.Services.WebService
    {

       [WebMethod(Description ="Soma de ...")]
        public int Sum(int x, int y)
        {
            return (x + y);
        }

        [WebMethod(Description = "Subtração de ...")]
        public int Sub(int x, int y)
        {
            return (x - y);
        }
    }
}

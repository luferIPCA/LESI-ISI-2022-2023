/**
 * Cliente Console de serviço WCF sobre Bases de Dados
 * lufer
 * */
using System;

using System.Data;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSet ds = new DataSet();
            WS.Service1Client ws = new WS.Service1Client();
            ds = ws.GetHotelCidade("Viana");

            DataTable dr = ds.Tables["Hotel"];

            for(int i=0; i < dr.Rows.Count; i++)
            {
                Console.WriteLine("Hotel: " + dr.Rows[i]["Nome"] + "\tLugares: " + dr.Rows[i]["Capacidade"]);
            }
        }
    }
}

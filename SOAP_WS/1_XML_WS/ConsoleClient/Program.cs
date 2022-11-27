/*
 * XML Web Service Client
 * by lufer & Oscar
 * 2022-2023
 */

using System;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //proxy do serviço externo
            //WS.FuncoesSoapClient proxy = new WS.FuncoesSoapClient();
            //Console.WriteLine("\nChamada de Serviço Externo:");
            //Console.WriteLine("2+3={0}", proxy.Sum(2, 3));


            WS2.FuncoesSoapClient outro = new WS2.FuncoesSoapClient();
            outro.SumCompleted += Outro_SumCompleted1;
            outro.SumAsync(2, 3);
            //outro.SumAsync(3, 4);
            Console.WriteLine("Antes");
            Console.ReadKey();
        }

        private static void Outro_SumCompleted1(object sender, WS2.SumCompletedEventArgs e)
        {
            Console.WriteLine("Res=" + e.Result.ToString());
        }

        private static void Outro_SumCompleted(object sender, WS2.SumCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

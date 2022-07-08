using System;

namespace lab_nbp_xml
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string[] startArguments = args;
                string currency = startArguments[0];
                DateTime from = DateTime.Parse(startArguments[1]);
                DateTime to = DateTime.Parse(startArguments[2]);
                Console.WriteLine("Retrieving data... Please wait");
                var XMLdata = new CurrencyXML(currency, from, to);
                Console.WriteLine("");

                Console.WriteLine(currency + " currency data");

                Console.WriteLine("");
                Console.WriteLine("------PURCHASE-----");
                Console.WriteLine("MAX: " + XMLdata.getMax("purchase"));
                Console.WriteLine("MIN: " + XMLdata.getMin("purchase"));
                Console.WriteLine("AVG: " + XMLdata.getAvg("purchase"));
                Console.WriteLine("DEVIATION: " + XMLdata.getDeviation("purchase"));

                Console.WriteLine("");

                Console.WriteLine("--------SALE-------");
                Console.WriteLine("MAX: " + XMLdata.getMax("sale"));
                Console.WriteLine("MIN: " + XMLdata.getMin("sale"));
                Console.WriteLine("AVG: " + XMLdata.getAvg("sale"));
                Console.WriteLine("DEVIATION: " + XMLdata.getDeviation("sale"));
            }
            catch(Exception ex)
            {
                Console.WriteLine("--------ERROR------");
                Console.WriteLine(ex);
                Console.WriteLine("--------ERROR------");
            }
            

        }
    }
}

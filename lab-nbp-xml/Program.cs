using System;
using System.Linq;

namespace lab_nbp_xml
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime f = new DateTime(2022, 07, 01);
            DateTime t = new DateTime(2022, 07, 06);
            var x = new CurrencyXML("eur", f, t);

            Console.WriteLine("Purchase");
            foreach(var c in x.valuesPurchase)
            {
                Console.WriteLine(c.ToString());
            }Console.WriteLine("Sale");
            foreach(var c in x.valuesSale)
            {
                Console.WriteLine(c.ToString());
            }

            Console.WriteLine("Dates");
            foreach(var c in x.dataDates)
            {
                Console.WriteLine(c.ToString());
            }
            Console.WriteLine("------Purchase-----");
            Console.WriteLine(x.getMax("purchase"));
            Console.WriteLine(x.getMin("purchase"));
            Console.WriteLine(x.getAvg("purchase"));
            Console.WriteLine(x.getDeviation("purchase"));

            Console.WriteLine("-----Sale------");
            Console.WriteLine(x.getMax("sale"));
            Console.WriteLine(x.getMin("sale"));
            Console.WriteLine(x.getAvg("sale"));
            Console.WriteLine(x.getDeviation("sale"));

        }
    }
}

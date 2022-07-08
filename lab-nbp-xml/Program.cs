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

            /* DateTime from = DateTime.Parse(Console.ReadLine());
            DateTime to = DateTime.Parse(Console.ReadLine());

            var dates = Enumerable
                .Range(0, int.MaxValue)
                .Select(index => new DateTime?(from.AddDays(index)))
                .TakeWhile(date => date <= to)
                .ToList();

            foreach (DateTime d in dates)
            {
                Console.WriteLine(d.Day);
            } */

            /* try
            {
                Start();
            }
            catch
            {
                Console.WriteLine("błąd");
            } */



        }
    }
}

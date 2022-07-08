using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace lab_nbp_xml
{
    public class CurrencyXML
    {
        private string[] availableCurrs = { "USD", "EUR", "CHF", "GBP" };
        public CurrencyXML(string curr, DateTime from, DateTime to)
        {
            curr = curr.ToUpper();

            if (!availableCurrs.Contains(curr))
            {
                throw new ArgumentOutOfRangeException();
            }

            getDataFromNBP(curr, from);


            List<DateTime?> range = getRange(from, to);

        }

        private List<DateTime?> getRange(DateTime from, DateTime to)
        {
            return Enumerable
                    .Range(0, int.MaxValue)
                    .Select(index => new DateTime?(from.AddDays(index)))
                    .TakeWhile(date => date <= to)
                    .ToList();
        }

        private static List<string> getListXML(DateTime date)
        {
            string listFilesYear = (date.ToString("yy") == DateTime.Today.ToString("yy")) ? "" : date.ToString("yy");

            WebClient listClient = new WebClient();
            Stream listStream = listClient.OpenRead("https://www.nbp.pl/kursy/xml/dir" + listFilesYear + ".txt");
            StreamReader listReader = new StreamReader(listStream);

            List<string> result = new List<string>();

            string dateFileName = date.ToString("yyMMdd");
            string filename = "c(.*?)(.*?)(.*?)z" + dateFileName;
            Regex rg = new Regex(filename, RegexOptions.IgnoreCase);

            foreach (Match match in rg.Matches(listReader.ReadToEnd()))
            {
                result.Add(match.Value);
            }

            return result;
        }

        public static void getDataFromNBP(string curr, DateTime date)
        {
            List<string> dataFiles = getListXML(date);
            foreach(string dF in dataFiles)
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("https://www.nbp.pl/kursy/xml/" + dF +".xml");
                StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();
                XDocument doc = XDocument.Parse(content);
                doc.Declaration = new XDeclaration("1.0", "utf-8", null);

                var result = doc.Descendants("kod_waluty")
                        .Where(el => el.Value == "EUR")
                        .First()
                        .Parent;

                var result2 = result.Element("nazwa_waluty").Value;

                Console.WriteLine(result2);
                Console.WriteLine("");
            }

        }
    }
}

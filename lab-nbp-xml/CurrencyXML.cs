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

        public readonly List<float> valuesPurchase = new List<float>();
        public readonly List<float> valuesSale = new List<float>();
        public readonly List<DateTime> dataDates = new List<DateTime>();
        public CurrencyXML(string curr, DateTime from, DateTime to)
        {
            curr = curr.ToUpper();

            if (!availableCurrs.Contains(curr))
            {
                throw new ArgumentOutOfRangeException();
            }

            List<string> dataFiles = new List<string>();
            List<DateTime?> range = getRange(from, to);
            foreach(DateTime dt in range)
            {
                List<string> getList = getListXML(dt);
                dataFiles = dataFiles.Concat(getList).ToList();
                if (getList.Count != 0)
                {
                    dataDates.Add(dt);
                }
            }

            getDataFromNBP(dataFiles, curr);
        }

        public string getMax(string s)
        {
            switch (s)
            {
                case "sale":
                    return "MAX " + valuesSale.Max() + " in " + dataDates[valuesSale.IndexOf(valuesSale.Max())].ToString("dd.MM.yyyy");
                case "purchase":
                    return "MAX " + valuesPurchase.Max() + " in " + dataDates[valuesPurchase.IndexOf(valuesPurchase.Max())].ToString("dd.MM.yyyy");
            }
            return "ERROR";
        }

        public string getMin(string s)
        {
            switch (s)
            {
                case "sale":
                    return "MIN " + valuesSale.Min() + " in " + dataDates[valuesSale.IndexOf(valuesSale.Min())].ToString("dd.MM.yyyy");
                case "purchase":
                    return "MIN " + valuesPurchase.Min() + " in " + dataDates[valuesPurchase.IndexOf(valuesPurchase.Min())].ToString("dd.MM.yyyy");
            }
            return "ERROR";
        }

        public string getAvg(string s)
        {
            switch (s)
            {
                case "sale":
                    return "AVG: " + valuesSale.Average();
                case "purchase":
                    return "AVG: " + valuesPurchase.Average();
            }
            return "ERROR";
        }

        public string getDeviation(string s)
        {
            double result = 0;
            double avg = 0;
            double sum = 0;

            switch (s)
            {
                case "sale":
                    avg = valuesSale.Average();
                    sum = valuesSale.Sum(d => Math.Pow(d - avg, 2));
                    result = Math.Sqrt((sum) / valuesSale.Count());
                    break;
                case "purchase":
                    avg = valuesSale.Average();
                    sum = valuesSale.Sum(d => Math.Pow(d - avg, 2));
                    result = Math.Sqrt((sum) / valuesSale.Count());
                    break;
            }
            return "DEVIATION: " + result;
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

        private void getDataFromNBP(List<string> dataFiles, string curr)
        {

            foreach (string dF in dataFiles)
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("https://www.nbp.pl/kursy/xml/" + dF +".xml");
                StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();
                XDocument doc = XDocument.Parse(content);
                doc.Declaration = new XDeclaration("1.0", "utf-8", null);

                var result = doc.Descendants("kod_waluty")
                        .Where(el => el.Value == curr)
                        .First()
                        .Parent;

                valuesPurchase.Add(float.Parse(result.Element("kurs_kupna").Value));
                valuesSale.Add(float.Parse(result.Element("kurs_sprzedazy").Value));
            }

        }
    }
}

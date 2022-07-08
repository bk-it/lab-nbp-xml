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

        private List<float> valuesPurchase = new List<float>();
        private List<float> valuesSale = new List<float>();
        private List<DateTime> dataDates = new List<DateTime>();

        public CurrencyXML(string curr, DateTime from, DateTime to)
        {
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
            float result = 0;
            string date = null;

            switch (s)
            {
                case "sale":
                    result = valuesSale.Max();
                    date = dataDates[valuesSale.IndexOf(valuesSale.Max())].ToString("dd.MM.yyyy");
                    break;
                case "purchase":
                    result = valuesPurchase.Max();
                    date = dataDates[valuesPurchase.IndexOf(valuesPurchase.Max())].ToString("dd.MM.yyyy");
                    break;
            }
            return result + " (" + date + ")";
        }

        public string getMin(string s)
        {
            float result = 0;
            string date = null;

            switch (s)
            {
                case "sale":
                    result = valuesSale.Min();
                    date = dataDates[valuesSale.IndexOf(valuesSale.Min())].ToString("dd.MM.yyyy");
                    break;
                case "purchase":
                    result = valuesPurchase.Min();
                    date = dataDates[valuesPurchase.IndexOf(valuesPurchase.Min())].ToString("dd.MM.yyyy");
                    break;
            }
            return result + " (" + date + ")";
        }

        public string getAvg(string s)
        {
            float result = 0;

            switch (s)
            {
                case "sale":
                    result = valuesSale.Average();
                    break;
                case "purchase":
                    result = valuesPurchase.Average();
                    break;
            }
            return result.ToString();
        }

        public string getDeviation(string s)
        {
            double result = 0;

            switch (s)
            {
                case "sale":
                    result = calcDeviation(valuesSale);
                    break;
                case "purchase":
                    result = calcDeviation(valuesPurchase);
                    break;
            }
            return result.ToString();
        }

        private double calcDeviation(List<float> f)
        {
            double avg = f.Average();
            double sum = f.Sum(d => Math.Pow(d - avg, 2));
            return Math.Sqrt((sum) / f.Count());
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
            string listFilesYear = (date.ToString("yyyy") == DateTime.Today.ToString("yyyy")) ? "" : date.ToString("yyyy");

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

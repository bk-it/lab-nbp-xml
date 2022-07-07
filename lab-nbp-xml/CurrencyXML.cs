using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Xml.Linq;

namespace lab_nbp_xml
{
    public static class CurrencyXML
    {
        public static void Start()
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead("https://www.nbp.pl/kursy/xml/lastC.xml");
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

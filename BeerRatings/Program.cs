using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace BeerRatings
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: BeerRatings beer_name");
                return 1;
            }
            var beerName = args[0];

            var baInfo = FetchBeerAdvocateRatings(beerName);
            foreach (var beerString in baInfo.Select(x => x.ToString()))
            {
                Console.WriteLine(beerString);
            }

            return 0;
        }

        private static BeerInfo[] FetchBeerAdvocateRatings(string beerName)
        {
            var url = string.Format("{0}/search/?q={1}&qt=beer", BeerAdvocateBaseUrl, WebUtility.UrlEncode(beerName));

            var baDoc = GetHtmlDoc(url);
            var baRatings = ParseBeerAdvocate(baDoc);

            return baRatings;
        }

        private static BeerInfo[] ParseBeerAdvocate(HtmlDocument htmlDoc)
        {
            var beerPageLinks = htmlDoc.DocumentNode.SelectNodes("//div[contains(@id, 'baContent')]/div/ul/li/a[1]");

            var ret = new List<BeerInfo>();
            foreach (var beerPageLink in beerPageLinks)
            {
                var beerUrl = beerPageLink.GetAttributeValue("href", null);
                var beerDoc = GetHtmlDoc(BeerAdvocateBaseUrl + beerUrl);
                ret.Add(GetSingleBeerInfo(beerDoc));
            }
            return ret.ToArray();
        }

        private static BeerInfo GetSingleBeerInfo(HtmlDocument beerDoc)
        {
            var name = beerDoc.DocumentNode.SelectSingleNode("//h1").InnerText.Split('-')[0].Trim();
            var strRating = beerDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'BAscore_big')]").InnerText;

            var rating = strRating.Trim() == "-" ? (double?) null : double.Parse(strRating) / 100;
            return new BeerInfo(name, rating, RatingSource.BeerAdvocate);
        }

        private static HtmlDocument GetHtmlDoc(string url)
        {
            var web = new HtmlWeb();
            return web.Load(url);

//            var request = (HttpWebRequest)WebRequest.Create(url);
//            using (var response = (HttpWebResponse) request.GetResponse())
//            {
//                if (response.StatusCode != HttpStatusCode.OK) throw new Exception(string.Format("Failed to call '{0}'. Received status code: {1}.", url, response.StatusCode));
//
//                using (var receiveStream = response.GetResponseStream())
//                {
//                    using (
//                        var readStream = response.CharacterSet == null
//                            ? new StreamReader(receiveStream)
//                            : new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet)))
//                    {
//                        return readStream.ReadToEnd();
//                    }
//                }
//            }

        }

        private const string BeerAdvocateBaseUrl = "http://www.beeradvocate.com";

    } 
}

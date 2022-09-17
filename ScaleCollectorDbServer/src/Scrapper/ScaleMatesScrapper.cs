using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapper
{
    public class ScaleMatesScrapper
    {
        public async Task<ScaleModelKit> GetKitInformationFromUrlAsync(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var address = url;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(address);

            if (document.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(nameof(document.StatusCode));

            var content = document.GetElementById("cont");
            var title = content!.GetElementsByTagName("h1")[0].TextContent;
            var subtitle = content.GetElementsByTagName("h2")[0].TextContent;

            var dataGrid = content.GetElementsByTagName("dl");

            string prevTitle = "";
            string brand = "";
            string manufacturerArticleNumber = "";
            string scale = "";
            string barcode = "";
            List<string> topics = new List<string>();
            foreach (var item in dataGrid.Children((string?)null))
            {
                if (item.LocalName == "dt")
                {
                    prevTitle = item.TextContent;
                    continue;
                }
                if (prevTitle.Equals("Brand:", StringComparison.InvariantCultureIgnoreCase))
                {
                    brand = item.Children[0].TextContent;
                }
                if (prevTitle.Equals("Number:", StringComparison.InvariantCultureIgnoreCase))
                {
                    manufacturerArticleNumber = item.TextContent;
                }
                if (prevTitle.Equals("Barcode:", StringComparison.InvariantCultureIgnoreCase))
                {
                    barcode = item.TextContent;
                }
                if (prevTitle.Equals("Scale:", StringComparison.InvariantCultureIgnoreCase))
                {
                    scale = item.TextContent;
                }
                if (prevTitle.Equals("Barcode:", StringComparison.InvariantCultureIgnoreCase))
                {
                    barcode = item.TextContent;
                }
                if (prevTitle.Equals("Topic:", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (var subItem in item.Children)
                    {
                        string s = subItem.TextContent;
                        s = s.Trim('»');
                        s = s.Trim();
                        topics.Add(s);
                    }
                }
            }

            var img = document.GetElementsByTagName("picture");
            var imgUrl = ((IHtmlImageElement)img.Children("").First(i => i.LocalName == "img")).Source;
            var httpClient = new HttpClient();
            var responseStream = await httpClient.GetStreamAsync(imgUrl);
            using var memoryStream = new MemoryStream();
            responseStream.CopyTo(memoryStream);

            return new ScaleModelKit(title, subtitle, brand, manufacturerArticleNumber, scale, barcode, topics, memoryStream.ToArray(), GetFileExtensionFromUrl(imgUrl));
        }

        public static string GetFileExtensionFromUrl(string url)
        {
            url = url.Split('?')[0];
            url = url.Split('/').Last();
            return url.Contains('.') ? url.Substring(url.LastIndexOf('.')) : "";
        }
    }

    public record ScaleModelKit
    {
        public string Title { get; init; }

        public string SubTitle { get; init; }

        public string Brand { get; init; }
        public string ManufacturerArticleNumber { get; }
        public string Scale { get; }
        public string Barcode { get; }
        public List<string> Topics { get; }
        public byte[] CoverImageBytes { get; }
        public string CoverImageExtension { get; }

        public ScaleModelKit(string title, string subtitle, string brand, string manufacturerArticleNumber, string scale, string barcode, List<string> topics, byte[] bytes, string extension)
        {
            Title = title;
            SubTitle = subtitle;
            Brand = brand;
            ManufacturerArticleNumber = manufacturerArticleNumber;
            Scale = scale;
            Barcode = barcode;
            Topics = topics;
            CoverImageBytes = bytes;
            CoverImageExtension = extension;
        }
    }
}

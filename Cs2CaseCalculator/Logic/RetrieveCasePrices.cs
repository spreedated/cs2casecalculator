using Cs2CaseCalculator.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Cs2CaseCalculator.Logic.Constants;

namespace Cs2CaseCalculator.Logic
{
    public class RetrieveCasePrices
    {
        internal HttpClientHandler httpHandler;
        public DateTime LastUpdated { get; private set; }
        public List<Case> Cases { get; private set; } = new();
        public async Task Update()
        {
            httpHandler ??= new();

            string content = null;

            using (HttpClient hc = new(this.httpHandler))
            {
                hc.Timeout = TimeSpan.FromSeconds(10);
                hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                hc.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8");

                HttpResponseMessage res = await hc.GetAsync(DATA_URL);
                content = await res.Content.ReadAsStringAsync();
            }

            HtmlDocument doc = new();
            doc.LoadHtml(content);

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='col-lg-4 col-md-6 col-widen text-center']");
            _ = nodes.Where(x => x.SelectSingleNode(".//h4") != null && x.SelectSingleNode(".//h4").InnerText.Contains("case", StringComparison.CurrentCultureIgnoreCase));

            this.Cases.Clear();

            foreach (HtmlNode n in nodes.Where(x => x.SelectSingleNode(".//h4") != null && x.SelectSingleNode(".//h4").InnerText.Contains("case", StringComparison.CurrentCultureIgnoreCase)))
            {
                this.Cases.Add(new Case()
                {
                    Id = this.Cases.Any() ? this.Cases.Last().Id + 1 : 1,
                    Name = n.SelectSingleNode(".//h4").InnerText.Replace("&amp;", " - "),
                    Price = double.Parse(n.SelectSingleNode(".//p[@class='nomargin']").InnerText.Replace(",", ".").Replace("-", "0").TrimEnd('€').TrimEnd('$'), CultureInfo.InvariantCulture)
                });

                this.Cases.Last().Imagepath = await this.DownloadImage(n);
            }
        }

        private async Task<string> DownloadImage(HtmlNode node)
        {
            Bitmap b = null;
            this.httpHandler = new();

            PrepareFolderStructure();

            if (DoesFileExist(node))
            {
                return $"{node.SelectSingleNode(".//h4").InnerText.Replace(":", "")}.png";
            }

            using (HttpClient hc = new(this.httpHandler))
            {
                hc.Timeout = TimeSpan.FromSeconds(10);
                hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                using (Stream s = await hc.GetStreamAsync(node.SelectSingleNode(".//img").GetAttributeValue<string>("src", null)))
                {
                    b = new(s);
                }
                
                b.Save(Path.Combine(Environment.CurrentDirectory, "cache", $"{node.SelectSingleNode(".//h4").InnerText.Replace(":", "")}.png"));
            }

            return $"{node.SelectSingleNode(".//h4").InnerText.Replace(":", "")}.png";
        }

        private static void PrepareFolderStructure()
        {
            string cacheFolder = Path.Combine(Environment.CurrentDirectory, "cache");

            if (!Directory.Exists(cacheFolder))
            {
                Directory.CreateDirectory(cacheFolder);
            }
        }

        private static bool DoesFileExist(HtmlNode node)
        {
            return File.Exists(Path.Combine(Environment.CurrentDirectory, "cache", $"{node.SelectSingleNode(".//h4").InnerText.Replace(":", "")}.png"));
        }
    }
}

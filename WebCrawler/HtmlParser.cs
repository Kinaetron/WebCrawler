using System;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebCrawler
{
    public class HtmlParser
    {
        private readonly Uri url;
        private HtmlDocument doc;

        private HtmlWeb web = new HtmlWeb();
        private List<string> pagesToCrawl = new List<string>();

        public HtmlParser(Uri url)
        {
            this.url = url ?? throw new ArgumentNullException(nameof(url));
        }

        public async Task<CrawlResultModel> Crawl()
        {
            pagesToCrawl.Add(url.AbsoluteUri);
            List<CrawlModel> crawlList = new List<CrawlModel>();

            for(int i = 0; i < pagesToCrawl.Count; i++)
            {
                var crawlData = new CrawlModel();

                crawlData.Page = pagesToCrawl[i];

                try {
                    doc = await web.LoadFromWebAsync(pagesToCrawl[i]);
                }
                catch {
                    Console.WriteLine("Failed to crawl page: " + pagesToCrawl[i]);
                    continue;
                }

                var links = doc.DocumentNode.SelectNodes("//a[@href]");

                if(links == null) {
                    continue;
                }

                foreach (var link in links)
                {
                    var hrefValue = link.GetAttributeValue("href", string.Empty);
                    string crawlLink = String.Empty;


                    if(hrefValue.StartsWith('/')) {
                        crawlLink = url.AbsoluteUri + hrefValue.TrimStart('/');
                    }
                    else if(hrefValue.StartsWith(url.AbsoluteUri)) {
                        crawlLink = hrefValue;
                    }
                    else {
                        continue;
                    }

                    if (crawlData.Links.Contains(crawlLink) || crawlData.Page == crawlLink) {
                        continue;
                    }

                    crawlData.Links.Add(crawlLink);

                    if (pagesToCrawl.Contains(crawlLink)) {
                        continue;
                    }

                    pagesToCrawl.Add(crawlLink);
                }

                if(crawlData.Links.Count > 0) {
                    crawlList.Add(crawlData);
                }
            }

            return new CrawlResultModel() { FileName = url.Host , CrawlModels = crawlList };
        }
    }
}

using System.Collections.Generic;

namespace WebCrawler
{

    public class CrawlResultModel
    {
        public string FileName { get; set; }
        public IEnumerable<CrawlModel> CrawlModels { get; set; }
    }

    public class CrawlModel
    {
        public string Page { get; set; }
        public List<string> Links { get; set; } = new List<string>();
    }
}

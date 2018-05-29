using Newtonsoft.Json;
using System;
using System.IO;

namespace WebCrawler
{
    public class IOHandler
    {
        public Uri Url { get; private set; }

        public (bool, string) ValidateUrl(string input)
        {
            if(String.IsNullOrWhiteSpace(input)) {
                return (false, "No url was passed");
            }

            Uri result;
            if(!Uri.TryCreate(input, UriKind.Absolute, out result)) {
                return (false, "The url isn't valid");
            }

            Url = result;
            return (true, String.Empty);
        }

        public string ExportFile(CrawlResultModel result)
        {
            if(result == null) {
                throw new ArgumentNullException(nameof(result));
            }

            using (StreamWriter file = File.CreateText(result.FileName + ".json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, result.CrawlModels);
            }

            return $"The site crawl is complete, results are in {result.FileName} json file";
        }
    }
}

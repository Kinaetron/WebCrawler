using System;
using System.Threading.Tasks;

namespace WebCrawler
{
    class Program
    {
        private static readonly string intro = "Please enter a url to webcrawl";

        private static IOHandler handler = new IOHandler();
        private static HtmlParser parser;

        public static async Task Main(string[] args)
        {
            Console.WriteLine(intro);

            Validation();
            parser = new HtmlParser(handler.Url);
            handler.ExportFile(await parser.Crawl());

            Console.ReadLine();
        }

        private static void Validation()
        {
            var results = handler.ValidateUrl(Console.ReadLine());

            if(results.Item1 == false)
            {
                Console.WriteLine(results.Item2);
                Validation();
            }
        }
    }
}

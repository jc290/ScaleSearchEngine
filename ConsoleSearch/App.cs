using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleSearch
{
    public class App
    {
        public void Run()
        {
            Console.WriteLine("Console Search");

            HttpClient http = new HttpClient();

            http.BaseAddress = new Uri("http://localhost:8080");

            while (true)
            {
                Console.WriteLine("enter search terms - q for quit");
                string input = Console.ReadLine() ?? string.Empty;
                if (input.Equals("q")) break;
                
                Task<string> httpTask = 
                    http.GetStringAsync(String.Format("/Search?terms={0}&numberOfResults=10", input));
                httpTask.Wait();

                var result = httpTask.Result;
                
                var searchResult = JsonConvert.DeserializeObject<SearchResult>(result);

                foreach (var document in searchResult.Documents)
                {
                    Console.WriteLine(String.Format("Document: {0}, Hits: {1}", document.Path, document.NumberOfOccurences));
                }

                foreach (var term in searchResult.IgnoredTerms)
                {
                    Console.WriteLine("Ignored: " + term);
                }
            }
        }
    }
}

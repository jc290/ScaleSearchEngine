using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;


namespace SearchAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        [HttpPost]
        public async Task<SearchResult> Search(string terms, int numberOfResults)
        {
            var mSearchLogic = new SearchLogic(new Database());

            var wordIds = new List<int>();

            var searchTerms = terms.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var result = new SearchResult();

            foreach (var t in searchTerms)
            {
                int id = mSearchLogic.GetIdOf(t);
                if (id != -1)
                {
                    wordIds.Add(id);
                }
                else
                {
                    result.IgnoredTerms.Add(t);
                }
            }

            DateTime start = DateTime.Now;

            var docIds = await mSearchLogic.GetDocuments(wordIds);


            var top = new List<int>();
            foreach (var p in docIds.GetRange(0, Math.Min(numberOfResults, docIds.Count)))
            {
                top.Add(p.Key);
            }

            int idx = 0;

            TimeSpan used = DateTime.Now - start;
            result.ElapsedMilliseconds = used.TotalMilliseconds;

            foreach (var doc in await mSearchLogic.GetDocumentDetails(top))
            {
                result.Documents.Add(new Document
                {
                    Id = idx + 1,
                    Path = doc,
                    NumberOfOccurences = docIds[idx].Value
                });
                idx++;
            }

            return result;
        }
    }
}

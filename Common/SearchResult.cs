namespace Common
{
    public class SearchResult
    {
        public List<Document> Documents { get; set; }
        public List<string> IgnoredTerms { get; set; }
        public double ElapsedMilliseconds { get; set; }

        public SearchResult()
        {
            IgnoredTerms = new List<string>();
            Documents = new List<Document>();
        }
    }
}
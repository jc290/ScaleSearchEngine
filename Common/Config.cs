namespace Common
{
    public static class Config
    {
        public static string DatabasePath { get; } = "/data/database.db";
        public static string DataSourcePath { get; } = "/data";
        public static int NumberOfFoldersToIndex { get; } = 10; // Use 0 or less for indexing all folders

        public static List<string> Services = new List<string>
        {
            "http://searchapi:80",
            "http://searchapi-1:80", 
            "http://searchapi-2:80", 
            "http://searchapi-3:80", 
            "http://searchapi-4:80"
        };
    }
}
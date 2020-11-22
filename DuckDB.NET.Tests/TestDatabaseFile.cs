using System.IO;

namespace DuckDB.NET.Tests
{
    public static class TestDatabaseFile
    {
        public static string GetRandomPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "db", Path.GetRandomFileName() + ".db");
        }

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }
        public static void DeleteIfExists(string path)
        {
            if (Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}

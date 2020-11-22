using DuckDB.NET.Data;
using Xunit;
using Xunit.Priority;

namespace DuckDB.NET.Tests
{
    public class ConnectionTests
    {
        [Fact, Priority(1)]
        public void ShouldOpenAndCloseInMemoryConnection()
        {
            using (var duckDBConnection = new DuckDBConnection())
            {
                duckDBConnection.Open();
                duckDBConnection.Close();
            }
        }
    }
}

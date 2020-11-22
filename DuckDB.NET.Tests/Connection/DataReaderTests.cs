using DuckDB.NET.Data;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Priority;

namespace DuckDB.NET.Tests
{
    public class DataReaderTests
    {
        [Fact, Priority(1)]
        public void ShouldReadSimpleTable()
        {
            using (var duckDBConnection = new DuckDBConnection())
            {
                duckDBConnection.Open();

                var createTableCommand = duckDBConnection.CreateCommand();

                createTableCommand.CommandText = "CREATE TABLE integers(foo INTEGER, bar INTEGER);";
                var executeNonQuery = createTableCommand.ExecuteNonQuery();

                var insertValuesCommand = duckDBConnection.CreateCommand();
                insertValuesCommand.CommandText = "INSERT INTO integers VALUES (3, 4), (5, 6), (7, NULL);";
                executeNonQuery = insertValuesCommand.ExecuteNonQuery();

                var readValuesCommand = duckDBConnection.CreateCommand();
                readValuesCommand.CommandText = "SELECT foo, bar FROM integers;";
                var reader = readValuesCommand.ExecuteReader();

                reader.HasRows.Should().BeTrue();

                var results = new List<Tuple<int?, int?>>();

                while (reader.Read())
                {
                    var foo = reader.GetInt32(0);
                    var bar = reader.GetInt32(1);

                    results.Add(new Tuple<int?, int?>(foo, bar));
                }

                results.Count.Should().Be(3);

                results[0].Item1.Should().Be(3);
                results[0].Item2.Should().Be(4);

                results[1].Item1.Should().Be(5);
                results[1].Item2.Should().Be(6);

                results[2].Item1.Should().Be(7);
                results[2].Item2.Should().Be(null);
            }
        }
    }
}

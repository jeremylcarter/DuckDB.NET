﻿using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace DuckDB.NET.Data
{
    public class DuckDBDataReader : DbDataReader
    {
        private DuckDbCommand command;
        private CommandBehavior behavior;

        private DuckDBResult queryResult;

        private int currentRow = -1;
        private bool closed = false;

        public DuckDBDataReader(DuckDbCommand command, CommandBehavior behavior)
        {
            this.command = command;
            this.behavior = behavior;

            var state = PlatformIndependentBindings.NativeMethods.DuckDBQuery(command.DBNativeConnection, command.CommandText, out queryResult);

            if (state.IsSuccess())
            {
                FieldCount = (int)queryResult.ColumnCount;
            }
            else
            {
                throw new DuckDBException("DuckDBQuery failed", state);
            }
        }

        public override bool GetBoolean(int ordinal)
        {
            return PlatformIndependentBindings.NativeMethods.DuckDBValueBoolean(queryResult, ordinal, currentRow);
        }

        public override byte GetByte(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            return queryResult.Columns[ordinal].Type.ToString();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override decimal GetDecimal(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override double GetDouble(int ordinal)
        {
            return PlatformIndependentBindings.NativeMethods.DuckDBValueDouble(queryResult, ordinal, currentRow);
        }

        public override Type GetFieldType(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override float GetFloat(int ordinal)
        {
            return PlatformIndependentBindings.NativeMethods.DuckDBValueFloat(queryResult, ordinal, currentRow);
        }

        public override Guid GetGuid(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override short GetInt16(int ordinal)
        {
            return PlatformIndependentBindings.NativeMethods.DuckDBValueInt16(queryResult, ordinal, currentRow);
        }

        public override int GetInt32(int ordinal)
        {
            return PlatformIndependentBindings.NativeMethods.DuckDBValueInt32(queryResult, ordinal, currentRow);
        }

        public override long GetInt64(int ordinal)
        {
            return PlatformIndependentBindings.NativeMethods.DuckDBValueInt64(queryResult, ordinal, currentRow);
        }

        public override string GetName(int ordinal)
        {
            return queryResult.Columns[ordinal].Name;
        }

        public override int GetOrdinal(string name)
        {
            var index = queryResult.Columns.ToList().FindIndex(c => c.Name == name);

            return index;
        }

        public override string GetString(int ordinal)
        {
            return PlatformIndependentBindings.NativeMethods.DuckDBValueVarchar(queryResult, ordinal, currentRow);
        }

        public override object GetValue(int ordinal)
        {
            var column = queryResult.Columns[ordinal];

            switch(column.Type)
            {
                case DuckDBType.DuckdbTypeInteger:
                    return GetInt32(ordinal);
                case DuckDBType.DuckdbTypeVarchar:
                    return GetString(ordinal);
            }

            throw new NotImplementedException();
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override bool IsDBNull(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int FieldCount { get; }

        public override object this[int ordinal] => throw new NotImplementedException();

        public override object this[string name] => throw new NotImplementedException();

        public override int RecordsAffected { get; }

        public override bool HasRows { get
            {
                return queryResult.RowCount > 0;
            }
        }

        public override bool IsClosed => closed;

        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        public override bool Read()
        {
            if (currentRow + 1 < queryResult.RowCount)
            {
                currentRow++;
                return true;
            }

            return false;
        }

        public override int Depth { get; }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            PlatformIndependentBindings.NativeMethods.DuckDBDestroyResult(out queryResult);

            if (behavior == CommandBehavior.CloseConnection)
            {
                command.CloseConnection();
            }

            closed = true;
        }
    }
}
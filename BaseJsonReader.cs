using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace PROJECT
{
    public class BaseJsonReader : IDataReader
    {
        public Stream responseStream;
        public StreamReader streamReader;
        public JsonTextReader jsonReader;
        public JsonSerializer serializer;

        public BaseJsonReader(Stream stream)
        {
            responseStream = stream;
            streamReader = new StreamReader(responseStream);
            jsonReader = new JsonTextReader(streamReader);
            serializer = new JsonSerializer();
            jsonReader.Read(); // To start reading the JSON array
        }

        public void Dispose() { responseStream.Dispose(); streamReader.Dispose(); jsonReader.Close(); }
        public void Close() => responseStream.Close();
        public int Depth => 0;
        public bool IsClosed => responseStream == null;
        public bool NextResult() => false;
        public int RecordsAffected => -1;

        #region Virtual methods 
        public virtual object this[int i] => throw new NotImplementedException();
        public virtual bool Read() => throw new NotImplementedException();
        public virtual object GetValue(int i) => throw new NotImplementedException();
        public virtual int FieldCount => throw new NotImplementedException();
        public virtual int GetValues(object[] values) => throw new NotImplementedException();
        public virtual bool IsDBNull(int i) => throw new NotImplementedException();
        #endregion
        
        #region Non-implemented methods
        public DataTable GetSchemaTable() => throw new NotImplementedException();
        public bool GetBoolean(int i) => throw new NotImplementedException();
        public byte GetByte(int i) => throw new NotImplementedException();
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        public char GetChar(int i) => throw new NotImplementedException();
        public long GetChars(int i, long fieldOffset, char[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        public string GetDataTypeName(int i) => throw new NotImplementedException();
        public DateTime GetDateTime(int i) => throw new NotImplementedException();
        public decimal GetDecimal(int i) => throw new NotImplementedException();
        public double GetDouble(int i) => throw new NotImplementedException();
        public Type GetFieldType(int i) => throw new NotImplementedException();
        public float GetFloat(int i) => throw new NotImplementedException();
        public Guid GetGuid(int i) => throw new NotImplementedException();
        public short GetInt16(int i) => throw new NotImplementedException();
        public int GetInt32(int i) => throw new NotImplementedException();
        public long GetInt64(int i) => throw new NotImplementedException();
        public string GetName(int i) => throw new NotImplementedException();
        public int GetOrdinal(string name) => throw new NotImplementedException();
        public string GetString(int i) => throw new NotImplementedException();
        public object this[string name] => throw new NotImplementedException();
        public IDataReader GetData(int i) => throw new NotImplementedException();

        #endregion
    }
}
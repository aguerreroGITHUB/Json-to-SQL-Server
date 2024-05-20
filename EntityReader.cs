using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace PROJECT
{
    class EntityClassReader : BaseJsonReader
    {
        //Only chaneg class object
        private EntityClass currentRecord;

        //Set number of class attributes
        public override int FieldCount => 5;


        public override bool Read()
        {//Change only class object it Deserialize<object>
            if (jsonReader.Read() && jsonReader.TokenType != JsonToken.EndArray)
            {
                currentRecord = serializer.Deserialize<EntityClass>(jsonReader);
                return true;
            }
            return false;
        }

        public override object GetValue(int i)
        {//Indexing configuration
            switch (i)
            {
                case 0: return currentRecord.attribute1;
                case 1: return currentRecord.attribute2;
                case 2: return currentRecord.attribute3;
                case 3: return currentRecord.attribute4;
                case 4: return currentRecord.attribute5;
                default: throw new IndexOutOfRangeException();
            }
        }

        // Match with current reader class name
        public EntityClassReader(Stream stream) : base(stream) { }

        #region Common methods for all derived classes
        public override int GetValues(object[] values)
        {
            int count = 0;
            for (int i = 0; i < FieldCount; i++)
            {
                values[i] = GetValue(i);
                count++;
            }
            return count;
        }

        public override object this[int i] => GetValue(i);
        public override bool IsDBNull(int i) => GetValue(i) is null;
        #endregion
    }
}
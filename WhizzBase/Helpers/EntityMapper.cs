using System;
using System.Collections.Concurrent;
using System.Data;
using Npgsql;

namespace WhizzBase.Helpers
{
    public static class EntityMapper
    {
        private static readonly ConcurrentDictionary<NpgsqlDataReader, ConcurrentDictionary<Type, DataReaderEntityMapper>> DataReaders = new ConcurrentDictionary<NpgsqlDataReader, ConcurrentDictionary<Type, DataReaderEntityMapper>>();
        private static readonly ConcurrentDictionary<DataTable, ConcurrentDictionary<Type, TableEntityMapper>> DataTables = new ConcurrentDictionary<DataTable, ConcurrentDictionary<Type, TableEntityMapper>>();

        public static DataReaderEntityMapper GetReaderMapper<T>(NpgsqlDataReader dataReader)
            where T : class, new()
        {
            var type = typeof(T);

            if (!DataReaders.ContainsKey(dataReader) || !DataReaders[dataReader].ContainsKey(typeof(T)))
            {
                DataReaders[dataReader] = new ConcurrentDictionary<Type, DataReaderEntityMapper>
                {
                    [type] = new DataReaderEntityMapper(dataReader, type)
                };
            }

            return DataReaders[dataReader][type];
        }
        
        public static TableEntityMapper GetTableMapper<T>(DataTable table)
            where T : class, new()
        {
            var type = typeof(T);

            if (!DataTables.ContainsKey(table) || !DataTables[table].ContainsKey(typeof(T)))
            {
                DataTables[table] = new ConcurrentDictionary<Type, TableEntityMapper>
                {
                    [type] = new TableEntityMapper(table, type)
                };
            }

            return DataTables[table][type];
        }
    }
}
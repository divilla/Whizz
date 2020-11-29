using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WhizzBase.Helpers;

namespace WhizzBase.Extensions
{
    public static class DataTableExtensions
    {
        public static async Task<ICollection<T>> QueryAsync<T>(this DataTable table)
            where T : class, new()
        {
            var result = new List<T>();
            var mapper = EntityMapper.GetTableMapper<T>(table);
            var reader = table.CreateDataReader();

            while (await reader.ReadAsync())
            {
                var entity = new T();
                foreach (var columnName in mapper.ColumnMaps.Keys)
                {
                    var value = reader[columnName];
                    if (value is DBNull) continue;
                    mapper.ColumnMaps[columnName].Set(entity, value);
                }
                
                result.Add(entity);
            }

            return result;
        }
    }
}

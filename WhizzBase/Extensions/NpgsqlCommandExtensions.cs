using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using WhizzBase.Helpers;

namespace WhizzBase.Extensions
{
    public static class NpgsqlCommandExtensions
    {
        public static async Task<ICollection<T>> QueryAsync<T>(this NpgsqlCommand command, bool prepare = false)
            where T : class, new()
        {
            if (prepare)
                await command.PrepareAsync();
            
            var reader = await command.ExecuteReaderAsync();
            var mapper = EntityMapper.GetReaderMapper<T>(reader);
            var result = new List<T>();

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
            await reader.CloseAsync();
            await command.DisposeAsync();

            return result;
        }
    }
}

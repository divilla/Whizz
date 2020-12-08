using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using WhizzBase.Base;
using WhizzBase.Helpers;
using WhizzBase.Reflection;

namespace WhizzBase.Extensions
{
    public static class NpgsqlCommandExtensions
    {

        public static ICollection<T> Query<T>(this NpgsqlCommand command, bool prepare = false)
            where T : class, new()
        {
            if (prepare && !command.IsPrepared)
                command.Prepare();
            
            var reader = command.ExecuteReader();
            var result = new List<T>();
            while (reader.Read())
            {
                var dictionary = new Dictionary<string, object>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    dictionary[reader.GetName(i)] = reader.GetValue(i);
                }
                
                result.Add(dictionary.ToInstance<T>());
            }
            reader.Close();
            reader.Dispose();

            return result;
        }

        public static async Task<ICollection<T>> QueryAsync<T>(this NpgsqlCommand command, T entity = null, bool prepare = false)
            where T : class, new()
        {
            if (prepare)
                await command.PrepareAsync();
            
            var reader = await command.ExecuteReaderAsync();
            var setters = ReflectionFactory.SetterDelegatesByPropertyName(typeof(T), Case.Snake);
            var result = new List<T>();

            if (entity != null && await reader.ReadAsync())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    setters[reader.GetName(i)](entity, reader.GetValue(i));
                }
            }
            else
            {
                while (await reader.ReadAsync())
                {
                    var dictionary = new Dictionary<string, object>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        dictionary[reader.GetName(i)] = reader.GetValue(i);
                    }
                
                    result.Add(dictionary.ToInstance<T>());
                }
            }

            await reader.CloseAsync();
            await reader.DisposeAsync();

            return result;
        }
    }
}

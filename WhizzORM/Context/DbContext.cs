using System;
using System.Collections.Concurrent;
using Npgsql;
using WhizzORM.Base;
using WhizzSchema;
using WhizzSchema.Interfaces;

namespace WhizzORM.Context
{
    public class DbContext
    {
        public DbContext(NpgsqlConnection connection)
        {
            Connection = connection;
            DbSchema = new DbSchema(connection.ConnectionString);
            _init();
        }

        public NpgsqlConnection Connection { get; }
        public IDbSchema DbSchema { get; }

        private void _init()
        {
            foreach (var propertyInfo in GetType().GetProperties())
            {
                var type = propertyInfo.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(RequestFactory<>))
                {
                    var entityType = type.GetGenericArguments()[0];
                    var instance = Activator.CreateInstance(propertyInfo.PropertyType, this);
                    propertyInfo.SetValue(this, instance, null);
                }
            }
        }
    }
}

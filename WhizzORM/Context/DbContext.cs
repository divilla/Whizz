using System;
using System.Collections.Concurrent;
using WhizzORM.Base;
using WhizzSchema;
using WhizzSchema.Interfaces;

namespace WhizzORM.Context
{
    public class DbContext
    {
        public DbContext(string connectionString)
        {
            ConnectionString = connectionString;
            DbSchema = new DbSchema(connectionString);
        }

        public IDbSchema DbSchema { get; }
        internal string ConnectionString { get; }

        private void _init()
        {
            
        }
    }
}

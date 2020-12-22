using System;
using Npgsql;
using WhizzJsonRepository.Interfaces;

namespace WhizzJsonRepository.Services
{
    public class PgConnectionFactory : IDisposable, IConnectionFactory
    {
        protected PgConnectionFactory(IDatabase database)
        {
            Database = database;
            Connection = new NpgsqlConnection(database.ConnectionString);
        }

        public IDatabase Database { get; }
        public NpgsqlConnection Connection { get; }
        
        public void Dispose()
        {
            Connection?.Close();
            Connection?.Dispose();
        }
    }
}
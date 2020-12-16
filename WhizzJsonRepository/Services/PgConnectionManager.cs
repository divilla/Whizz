using System;
using Npgsql;

namespace WhizzJsonRepository.Services
{
    public class PgConnectionManager<TDb> : IDisposable
        where TDb : PgDatabase
    {
        protected PgConnectionManager(TDb db)
        {
            Connection = new NpgsqlConnection(db.ConnectionString);
            Connection.Open();
        }

        public NpgsqlConnection Connection { get; }

        public void Dispose()
        {
            Connection?.Close();
            Connection?.Dispose();
        }
    }
}
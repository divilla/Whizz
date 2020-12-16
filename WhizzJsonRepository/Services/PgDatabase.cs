using System;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using WhizzBase.Enums;
using WhizzBase.Extensions;
using WhizzJsonRepository.Interfaces;
using WhizzORM.Interfaces;
using WhizzSchema;
using WhizzSchema.Interfaces;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace WhizzJsonRepository.Services
{
    public abstract class PgDatabase : IPgDatabase
    {
        protected PgDatabase(string connectionString)
        {
            ConnectionString = connectionString;
            Schema = new DbSchema(connectionString);
        }


        public string ConnectionString { get; } 
        public DbSchema Schema { get; }
        public Func<string, string> Quote => Schema.Quote;
        public Case DbCase = Case.Snake;
        public Func<string, string> ToDbCase => (s) => s.ToSnakeCase();
        public Func<string, string> ToQuotedDbCase => (s) => Quote(s.ToSnakeCase());
        public Case JsonCase => Case.Camel;
        public Func<string, string> ToJsonCase => (s) => s.ToCamelCase();
        public Func<string, string> ToQuotedJsonCase  => (s) => Quote(s.ToCamelCase());
        public IPgTypeValidator TypeValidator { get; set; } = new PgTypeValidator();
        public PgValidationErrorMessages ErrorMessages { get; set; } = new PgValidationErrorMessages();

        
        
        public NpgsqlConnection OpenConnection()
        {
            var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            return connection;
        }

        public async Task<NpgsqlConnection> OpenConnectionAsync()
        {
            var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();
            
            return connection;
        }
    }
}
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
    public abstract class PgDatabase : IDatabase
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

        public ITypeValidator TypeValidator { get; set; } = new TypeValidator();
        public ValidationErrorMessages ErrorMessages { get; set; } = new ValidationErrorMessages();

        public string QuotedRelationName(string relationName, string schemaName)
        {
            return Schema.QuotedSchemaRelation(relationName, schemaName);
        }
    }
}
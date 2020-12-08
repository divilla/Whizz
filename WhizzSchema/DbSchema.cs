using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;
using Utf8Json;
using Utf8Json.Resolvers;
using WhizzBase.Base;
using WhizzBase.Extensions;
using WhizzSchema.Entities;
using WhizzSchema.Interfaces;
using WhizzSchema.Schema;

namespace WhizzSchema
{
    public class DbSchema : IDbSchema
    {
        public const string DefaultSchema = "public";

        public string DatabaseName { get; private set; }
        public ImmutableArray<SchemaEntity> SchemaEntities { get; private set; }
        public ImmutableArray<RelationEntity> RelationEntities { get; private set; }
        public ImmutableArray<ColumnEntity> ColumnEntities { get; private set; }
        public ImmutableArray<ForeignKeyEntity> ForeignKeyEntities { get; private set; }
        public ImmutableArray<UniqueIndexEntity> UniqueIndexEntities { get; private set; }
        public ImmutableDictionary<uint, TypeEntity> TypeEntities { get; private set; }
        public ImmutableDictionary<string, ImmutableArray<Type>> TypeMap { get; private set; }
        public ImmutableArray<string> UsingNamespaces { get; private set; }
        public ImmutableSortedSet<string> Keywords { get; private set; }
        
        private NpgsqlConnection _connection;
 
        public DbSchema(string connectionString)
        {
            _init(connectionString).Wait();

            // var relationSchema = GetRelationSchema("relations", "public");
            // var fields = relationSchema.ColumnNames.Select(s => $"{s} AS \"{s.ToCamelCase()}\"");
            // var sql = $"SELECT json_agg(t) FROM (SELECT {string.Join(", ", fields)} FROM relations) t";
            // var command = new NpgsqlCommand(sql, _connection);
            // Console.WriteLine(command.ExecuteScalar());
        }

        public bool SchemaExists(string schemaName)
        {
            return SchemaEntities.Any(q => q.SchemaName == schemaName);
        }

        public bool RelationExists(string relationName, string schemaName)
        {
            return RelationEntities.Any(q => q.RelationName == relationName && q.SchemaName == schemaName);
        }

        public string QuotedRelationName(string relationName, string schemaName)
        {
            if (!RelationExists(relationName, schemaName))
                throw new DbException($"Relation does not exist: '{relationName}'.'{schemaName}'");
            
            return schemaName == DefaultSchema ? Quote(relationName) : $"{Quote(schemaName)}.{Quote(relationName)}";
        }

        public RelationEntity UnquoteRelationName(string name)
        {
            if (string.IsNullOrEmpty(name)) 
                throw new DbArgumentException("Relation name should not be null or empty", nameof(name));

            var names = name.Split(".");
            var relationName = Unquote(names[0]);
            var schemaName = (names.Length > 1) ? Unquote(names[1]) : DefaultSchema;
            
            if (!RelationExists(relationName, schemaName))
                throw new DbArgumentException($"Relation '{name}' does not exist in database");

            return GetRelation(relationName, schemaName);
        }

        public string Unquote(string value)
        {
            return value.Replace("\"", "");
        }
        
        public string Quote(string value)
        {
            if (Regex.IsMatch(value, "[^a-z0-9_]") || Keywords.Contains(value))
                return $"\"{value}\"";

            return value;
        }

        public string EscapedQuotedRelationName(string relationName, string schemaName)
        {
            if (!RelationExists(relationName, schemaName))
                throw new DbException($"Relation does not exist: '{relationName}'.'{schemaName}'");
            
            return schemaName == DefaultSchema ? EscapedQuote(relationName) : $"{EscapedQuote(schemaName)}.{EscapedQuote(relationName)}";
        }
        
        public string EscapedQuote(string value)
        {
            if (Regex.IsMatch(value, "[^a-z0-9_]") || Keywords.Contains(value))
                return $"\\\"{value}\\\"";

            return value;
        }
        
        public RelationEntity GetRelation(string relationName, string schemaName = DefaultSchema)
        {
            if (!RelationExists(relationName, schemaName))
                throw new DbException($"Relation does not exist: '{relationName}'.'{schemaName}'");
            
            return RelationEntities.Single(q => q.RelationName == relationName && q.SchemaName == schemaName);
        }

        public ImmutableArray<ColumnEntity> GetColumns(string relationName, string schemaName = DefaultSchema)
        {
            if (!RelationExists(relationName, schemaName))
                throw new DbException($"Relation does not exist: '{relationName}'.'{schemaName}'");
            
            return ColumnEntities.Where(q => q.RelationName == relationName && q.SchemaName == schemaName).OrderBy(o => o.Position).ToImmutableArray();
        }

        public ImmutableArray<ForeignKeyEntity> GetForeignKeys(string tableName, string schemaName = DefaultSchema)
        {
            if (!RelationExists(tableName, schemaName))
                throw new DbException($"Relation does not exist: '{tableName}'.'{schemaName}'");
            
            return ForeignKeyEntities.Where(q => q.TableName == tableName && q.SchemaName == schemaName).ToImmutableArray();
        }

        public ImmutableArray<UniqueIndexEntity> GetUniqueIndexes(string tableName, string schemaName = DefaultSchema)
        {
            if (!RelationExists(tableName, schemaName))
                throw new DbException($"Relation does not exist: '{tableName}'.'{schemaName}'");
            
            return UniqueIndexEntities.Where(q => q.TableName == tableName && q.SchemaName == schemaName).ToImmutableArray();
        }

        public TypeEntity GetType(ColumnEntity column)
        {
            return TypeEntities.ContainsKey(column.TypeOid) ? TypeEntities[column.TypeOid] : null;
        }

        public string GetTypeName(ColumnEntity column)
        {
            var typeEntity = GetType(column);
            var type = typeEntity?.Type == null ? typeof(string) : typeEntity.Type;

            return type.ToKeywordName(column.Dimension);
        }

        public ImmutableArray<string> GetColumnNames(string relationName, string schemaName = IDbSchema.DefaultSchema)
        {
            return GetColumns(relationName, schemaName)
                .Select(s => s.ColumnName)
                .ToImmutableArray();
        }

        public ImmutableDictionary<string, Type> GetColumnTypes(string relationName, string schemaName = IDbSchema.DefaultSchema)
        {
            return  GetColumns(relationName, schemaName)
                .ToDictionary(k => k.ColumnName, v => GetType(v)?.Type ?? typeof(string))
                .ToImmutableDictionary();
        }

        private async Task _init(string connectionString)
        {
            JsonSerializer.SetDefaultResolver(StandardResolver.AllowPrivateCamelCase);
            
            _connection = new NpgsqlConnection(connectionString);
            await _connection.OpenAsync();

            DatabaseName = _databaseName();
            SchemaEntities = SchemasLoader.Load(_connection).ToImmutableArray();
            RelationEntities = RelationsLoader.Load(_connection).ToImmutableArray();
            ColumnEntities = ColumnsLoader.Load(_connection).ToImmutableArray();
            ForeignKeyEntities = ForeignKeysLoader.Load(_connection).ToImmutableArray();
            UniqueIndexEntities = UniqueIndexesLoader.Load(_connection).ToImmutableArray();
            TypeEntities = TypesLoader.Load(_connection).ToImmutableDictionary();
            UsingNamespaces = _usingNamespaces().ToImmutableArray();
            TypeMap = _typesMap().ToImmutableDictionary();
            Keywords = _keywords().ToImmutableSortedSet();
        }

        private string _databaseName()
        {
            const string sql = "SELECT current_database()::text";
            var command = new NpgsqlCommand(sql, _connection);
            return command.ExecuteScalar().ToString();
        }

        private IEnumerable<string> _usingNamespaces()
        {
            var usings = new HashSet<string>();
            foreach (var columnEntity in ColumnEntities)
            {
                if (columnEntity.TypeType == "e")
                    continue;
                
                var typeOid = columnEntity.TypeOid;
                if (!TypeEntities.ContainsKey(typeOid)) throw new DbException($"DataType '{columnEntity.RelationName}' '{columnEntity.ColumnName}' is not supported");
                var typeEntity = TypeEntities[typeOid];
                if (typeEntity.Using == null) continue;

                if (!usings.Contains(typeEntity.Using))
                    usings.Add(typeEntity.Using);
            }

            return usings;
        }
        
        private Dictionary<string, ImmutableArray<Type>> _typesMap()
        {
            var results = _connection.TypeMapper.Mappings.ToDictionary(
                k => k.PgTypeName,
                v =>
                {
                    var types = new List<Type> {v.TypeHandlerFactory.DefaultValueType};
                    foreach (var type in v.ClrTypes)
                    {
                        if (!types.Contains(type))
                            types.Add(type);
                    }

                    foreach (var type in TypeExtensions.FindNullableTypes(types))
                    {
                        if (!types.Contains(type))
                            types.Add(type);
                    }

                    return types.ToImmutableArray();
                });

            if (!results.ContainsKey("timestamp without timezone"))
                results["timestamp without time zone"] = (new List<Type> {typeof(DateTime)}).ToImmutableArray();

            return results;
        }
        
        private IEnumerable<string> _keywords()
        {
            const string sql = "SELECT word FROM pg_get_keywords() WHERE catcode <> 'U'";
            var command = new NpgsqlCommand(sql, _connection);
            var results = command.Query<DbKeyword>();
            
            return results.Select(s => s.Word).ToArray();
        }
    }
}

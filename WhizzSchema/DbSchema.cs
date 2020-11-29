using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using WhizzBase.Base;
using WhizzBase.Extensions;
using WhizzBase.Helpers;
using WhizzSchema.Entities;
using WhizzSchema.Interfaces;
using WhizzSchema.Schema;

// ReSharper disable StringLiteralTypo

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
        public ImmutableSortedDictionary<string, SchemaSchema> Schemas { get; private set; }

        public ImmutableSortedSet<string> Keywords { get; private set; }
        private NpgsqlConnection _connection;
 
        public DbSchema(string connectionString)
        {
            _init(connectionString).Wait();
        }

        public bool SchemaExists(string schemaName)
        {
            return Schemas.ContainsKey(schemaName);
        }

        public bool RelationExists(string relationName, string schemaName)
        {
            return SchemaExists(schemaName) && Schemas[schemaName].Relations.ContainsKey(relationName);
        }

        public RelationSchema GetRelationSchema(string relationName, string schemaName)
        {
            return Schemas[schemaName].Relations[relationName];
        }

        public string GetQualifiedRelationName(string relationName, string schemaName)
        {
            return schemaName == DefaultSchema ? Quote(relationName) : $"{Quote(schemaName)}.{Quote(relationName)}";
        }

        public string GetEscapedRelationName(string relationName, string schemaName)
        {
            return schemaName == DefaultSchema ? EscapedQuote(relationName) : $"{EscapedQuote(schemaName)}.{EscapedQuote(relationName)}";
        }

        public string Quote(string value)
        {
            if (Regex.IsMatch(value, "[^a-z0-9_]") || Keywords.Contains(value))
                return $"\"{value}\"";

            return value;
        }
        
        public string EscapedQuote(string value)
        {
            if (Regex.IsMatch(value, "[^a-z0-9_]") || Keywords.Contains(value))
                return $"\\\"{value}\\\"";

            return value;
        }
        
        private async Task _init(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
            await _connection.OpenAsync();

            DatabaseName = await _databaseName();
            SchemaEntities = (await _schemas()).ToImmutableArray();
            RelationEntities = (await _relations()).ToImmutableArray();
            ColumnEntities = (await _columns()).ToImmutableArray();
            UniqueIndexEntities = (await _uniqueIndexes()).ToImmutableArray();
            ForeignKeyEntities = (await _foreignKeys()).ToImmutableArray();
            Keywords = (await _keywords()).ToImmutableSortedSet();
            TypeEntities = (await _types()).ToImmutableDictionary();
            TypeMap = _typesMap().ToImmutableDictionary();
            UsingNamespaces = _usingNamespaces().ToImmutableArray();

            Schemas = SchemaEntities
                .ToImmutableSortedDictionary(
                    d => d.SchemaName,
                    d => new SchemaSchema(this, d.SchemaName));
        }

        private async Task<string> _databaseName()
        {
            const string sql = "SELECT current_database()::text";
            var command = new NpgsqlCommand(sql, _connection);
            return (await command.ExecuteScalarAsync()).ToString();
        }

        private async Task<SchemaEntity[]> _schemas()
        {
            const string sql = @"
SELECT n.nspname AS schema_name,
    pg_get_userbyid(n.nspowner) AS schema_owner,
    pg_catalog.has_schema_privilege(n.nspname, 'USAGE') AS can_use,
    pg_catalog.has_schema_privilege(n.nspname, 'CREATE') AS can_create
FROM pg_catalog.pg_namespace n
WHERE
    n.nspname NOT LIKE 'pg_%' AND n.nspname != 'information_schema'
ORDER BY
    n.nspname;
            ";
            var command = new NpgsqlCommand(sql, _connection);
            var results = await command.QueryAsync<SchemaEntity>();
            
            return results.ToArray();
        }

        private async Task<RelationEntity[]> _relations()
        {
            var sql = @"
SELECT
    n.nspname::text AS schema_name,
    c.relname::text AS relation_name,
    pg_get_userbyid(c.relowner)::text AS relation_owner,
    c.relkind::text AS relation_kind,
    has_table_privilege(quote_ident(n.nspname)||'.'||quote_ident(c.relname), 'SELECT'::text) AS can_select,
    has_table_privilege(quote_ident(n.nspname)||'.'||quote_ident(c.relname), 'INSERT'::text) AS can_insert,
    has_table_privilege(quote_ident(n.nspname)||'.'||quote_ident(c.relname), 'UPDATE'::text) AS can_update,
    has_table_privilege(quote_ident(n.nspname)||'.'||quote_ident(c.relname), 'DELETE'::text) AS can_delete
FROM pg_class c
    LEFT JOIN pg_namespace n ON n.oid = c.relnamespace
    LEFT JOIN pg_tablespace t ON t.oid = c.reltablespace
WHERE
    c.relkind IN ('r', 'p', 'v', 'm')
    AND n.nspname NOT LIKE 'pg_%' AND n.nspname != 'information_schema'
    AND (pg_has_role(c.relowner, 'USAGE'::text) OR has_table_privilege(quote_ident(n.nspname)||'.'||quote_ident(c.relname), 'SELECT'::text))
ORDER BY 
    schema_name,
    relation_name;
            ";

            var command = new NpgsqlCommand(sql, _connection);
            var results = await command.QueryAsync<RelationEntity>();
            
            return results.ToArray();
        }

        private async Task<ColumnEntity[]> _columns()
        {
            var generated = "";
            if (_connection.PostgreSqlVersion.Major >= 12)
                generated = "OR attidentity != '' OR attgenerated != ''";
                
            var sql = @$"
SELECT
    d.nspname::text                                                                                     AS schema_name,
    c.relname::text                                                                                     AS relation_name,
    a.attname::text                                                                                     AS column_name,
    a.attnum                                                                                            AS position,
    COALESCE(td.oid, tb.oid, t.oid)::oid                                                                AS type_oid,
    COALESCE(td.typname, tb.typname, t.typname)::text                                                   AS data_type,
    COALESCE(td.typtype, tb.typtype, t.typtype)::text                                                   AS type_type,
    a.attlen::int                                                                                       AS size,
    a.atttypmod::int                                                                                    AS modifier,
    COALESCE(NULLIF(a.attndims, 0), NULLIF(t.typndims, 0), (t.typcategory='A')::int)                    AS dimension,
    CAST(
             information_schema._pg_char_max_length(information_schema._pg_truetypid(a, t),
            information_schema._pg_truetypmod(a, t))
             AS int
    )                                                                                                   AS character_maximum_length,
    CASE atttypid
        WHEN 21 /*int2*/ THEN 16
        WHEN 23 /*int4*/ THEN 32
        WHEN 20 /*int8*/ THEN 64
        WHEN 1700 /*numeric*/ THEN
            CASE WHEN atttypmod = -1
                THEN null
                ELSE ((atttypmod - 4) >> 16) & 65535
            END
        WHEN 700 /*float4*/ THEN 24 /*FLT_MANT_DIG*/
        WHEN 701 /*float8*/ THEN 53 /*DBL_MANT_DIG*/
        ELSE null
    END::int                                                                                            AS numeric_precision,
    CASE
        WHEN atttypid IN (21, 23, 20) THEN 0
        WHEN atttypid IN (1700) THEN
        CASE
            WHEN atttypmod = -1 THEN null
            ELSE (atttypmod - 4) & 65535
        END
        ELSE null
    END::int                                                                                            AS numeric_scale,
    CASE WHEN COALESCE(td.typtype, tb.typtype, t.typtype) = 'e'::char
        THEN (SELECT array_agg(enumlabel) FROM pg_enum WHERE enumtypid = COALESCE(td.oid, tb.oid, a.atttypid))
        ELSE NULL
    END                                                                                                 AS enum_values,
    CAST(pg_get_expr(ad.adbin, ad.adrelid) AS text)                                                     AS column_default,
    a.attnotnull                                                                                        AS is_not_null,
    coalesce(pg_get_expr(ad.adbin, ad.adrelid) ~ 'nextval',false)
        {generated}
        OR (t.typname = 'uuid' AND CAST(pg_get_expr(ad.adbin, ad.adrelid) AS text) IS NOT NULL)         AS is_generated,
    CASE
        WHEN a.attnum = any (ct.conkey) THEN true
        ELSE false
        END::bool                                                                                       AS is_primary_key,
    CASE
        WHEN (CAST(pg_get_expr(ad.adbin, ad.adrelid) AS text) IS NOT NULL) 
            OR coalesce(pg_get_expr(ad.adbin, ad.adrelid) ~ 'nextval', false)
            {generated} THEN false
        ELSE true
        END::bool                                                                                       AS is_required,
    CASE
        WHEN (c.relkind = ANY (ARRAY ['r', 'p'])) 
            OR (c.relkind = ANY (ARRAY ['v', 'f'])) 
            AND pg_column_is_updatable(c.oid::regclass, a.attnum, false) THEN true
            ELSE false
            END::bool                                                                                   AS is_updatable,
    pg_catalog.col_description(c.oid, a.attnum)                                                         AS column_comment
FROM
    pg_class c
    LEFT JOIN pg_attribute a ON a.attrelid = c.oid
    LEFT JOIN pg_attrdef ad ON a.attrelid = ad.adrelid AND a.attnum = ad.adnum
    LEFT JOIN pg_type t ON a.atttypid = t.oid
    LEFT JOIN pg_type tb ON (a.attndims > 0 OR t.typcategory='A') AND t.typelem > 0 AND t.typelem = tb.oid OR t.typbasetype > 0 AND t.typbasetype = tb.oid
    LEFT JOIN pg_type td ON t.typndims > 0 AND t.typbasetype > 0 AND tb.typelem = td.oid
    LEFT JOIN pg_namespace d ON d.oid = c.relnamespace
    LEFT JOIN pg_constraint ct ON ct.conrelid = c.oid AND ct.contype = 'p'
WHERE
    a.attnum > 0 AND t.typname != '' AND NOT a.attisdropped
    AND c.relkind IN ('r', 'p', 'v', 'm')
    AND d.nspname NOT LIKE 'pg_%' AND d.nspname != 'information_schema'
    AND (pg_has_role(c.relowner, 'USAGE'::text) OR has_table_privilege(quote_ident(d.nspname)||'.'||quote_ident(c.relname), 'SELECT'::text))
ORDER BY
    schema_name,
    relation_name,
    position;
            ";

            var command = new NpgsqlCommand(sql, _connection);
            var results = await command.QueryAsync<ColumnEntity>();
            
            return results.ToArray();
        }

        private async Task<ForeignKeyEntity[]> _foreignKeys()
        {
            var sql = @"
SELECT ns.nspname        AS schema_name,
       c.relname         AS table_name,
       a.attname         AS column_name,
       ct.conname        AS constraint_name,
       fns.nspname::text AS primary_key_schema_name,
       fc.relname::text  AS primary_key_table_name,
       fa.attname::text  AS primary_key_column_name
FROM (SELECT cts.conname,
             cts.conrelid,
             cts.confrelid,
             cts.conkey,
             cts.contype,
             cts.confkey,
             generate_subscripts(cts.conkey, 1) AS s
      FROM pg_constraint cts) ct
         JOIN pg_class c ON c.oid = ct.conrelid
         JOIN pg_namespace ns ON c.relnamespace = ns.oid
         JOIN pg_attribute a ON a.attrelid = ct.conrelid AND a.attnum = ct.conkey[ct.s]
         LEFT JOIN pg_class fc ON fc.oid = ct.confrelid
         LEFT JOIN pg_namespace fns ON fc.relnamespace = fns.oid
         LEFT JOIN pg_attribute fa ON fa.attrelid = ct.confrelid AND fa.attnum = ct.confkey[ct.s]
WHERE ct.contype = 'f'
    AND ns.nspname NOT LIKE 'pg_%' AND ns.nspname != 'information_schema'
    AND (pg_has_role(c.relowner, 'USAGE'::text) OR has_table_privilege(quote_ident(ns.nspname)||'.'||quote_ident(c.relname), 'SELECT'::text))
ORDER BY 
    ns.nspname, 
    c.relname, 
    ct.conname, 
    a.attnum;
            ";

            var command = new NpgsqlCommand(sql, _connection);
            var results = await command.QueryAsync<ForeignKeyEntity>();
            
            return results.ToArray();
        }

        private async Task<UniqueIndexEntity[]> _uniqueIndexes()
        {
            var sql = @"
SELECT ns.nspname::text AS schema_name,
       tc.relname::text AS table_name,
       ic.relname::text AS index_name,
       array_agg(a.attname)::text[] AS column_names
FROM pg_namespace ns
         JOIN pg_class tc ON tc.relnamespace = ns.oid
         JOIN pg_index i ON i.indrelid = tc.oid
         JOIN pg_class ic ON i.indexrelid = ic.oid
         JOIN pg_attribute a ON i.indrelid = a.attrelid AND (a.attnum = ANY (i.indkey::smallint[]))
WHERE NOT i.indisprimary AND i.indisunique
    AND tc.relkind IN ('r', 'p', 'v', 'm')
    AND ns.nspname NOT LIKE 'pg_%' AND ns.nspname != 'information_schema'
    AND (pg_has_role(tc.relowner, 'USAGE'::text) OR has_table_privilege(quote_ident(ns.nspname)||'.'||quote_ident(tc.relname), 'SELECT'::text))
GROUP BY
    ns.nspname::text,
    tc.relname::text,
    ic.relname::text
ORDER BY 
    schema_name, 
    table_name, 
    index_name;
            ";

            var command = new NpgsqlCommand(sql, _connection);
            var results = await command.QueryAsync<UniqueIndexEntity>();
            
            return results.ToArray();
        }

        private async Task<string[]> _keywords()
        {
            const string sql = "SELECT word FROM pg_get_keywords() WHERE catcode <> 'U'";
            var command = new NpgsqlCommand(sql, _connection);
            var results = await command.QueryAsync<DbKeyword>();
            
            return results.Select(s => s.Word).ToArray();
        }
        
        private async Task<ConcurrentDictionary<uint, TypeEntity>> _types()
        {
            var dataTable = _connection.GetSchema("DataTypes");
            var mapper = EntityMapper.GetTableMapper<TypeEntity>(dataTable);
            var dataReader = dataTable.CreateDataReader();
            var types = new ConcurrentDictionary<uint, TypeEntity>();
            var systemCollectionsAssembly = typeof(System.Collections.BitArray).Assembly;
            var systemNetAssembly = typeof(IPAddress).Assembly;
            var systemNetNetworkInformationAssembly = typeof(PhysicalAddress).Assembly;
            var npgsqlAssembly = typeof(NpgsqlBox).Assembly;
            var newtonsoftJsonLinqAssembly = typeof(JObject).Assembly;
            while (await dataReader.ReadAsync())
            {
                var type  = new TypeEntity();
                foreach (var columnName in mapper.ColumnMaps.Keys)
                {
                    var value = dataReader[columnName];
                    if (value is DBNull) continue;
                    mapper.ColumnMaps[columnName].Set(type, value);
                }

                if ((type.TypeName == "OID" || type.TypeName == "xid" || type.TypeName == "cid" || type.TypeName == "tid") && string.IsNullOrEmpty(type.DataType))
                {
                    type.DataType = "System.UInt32";
                    type.IsUnsigned = true;
                }

                if (type.DataType == "String")
                    type.DataType = "System.String";
                else if (type.DataType == "System.Text.Json.JsonDocument")
                    type.DataType = "Newtonsoft.Json.Linq.JObject";
                else if (type.DataType == "System.Text.Json.JsonDocument[]")
                    type.DataType = "Newtonsoft.Json.Linq.JArray";

                if (!string.IsNullOrEmpty(type.DataType))
                {
                    type.Type = Type.GetType(type.DataType);
                    if (type.Type == null)
                    {
                        if (type.DataType.Contains("NpgsqlRange"))
                        {
                            var match = Regex.Match(type.DataType, @"\[[^\.]+\.([A-Za-z0-9]+)");
                            type.DataType = $"NpgsqlRange<{match.Groups[1].Value}>";
                            type.Type = npgsqlAssembly.GetType(type.DataType);
                            type.Using = "NpgsqlTypes";
                        }
                        else if (type.DataType.StartsWith("Npgsql"))
                        {
                            type.Type = npgsqlAssembly.GetType(type.DataType);
                            type.Using = "NpgsqlTypes";
                        }
                        else if (type.DataType.StartsWith("System.Collections"))
                        {
                            type.Type = systemCollectionsAssembly.GetType(type.DataType);
                            type.Using = "System.Collections";
                        }
                        else if (type.DataType.StartsWith("System.Net.NetworkInformation"))
                        {
                            type.Type = systemNetNetworkInformationAssembly.GetType(type.DataType);
                            type.Using = "System.Net.NetworkInformation";
                        }
                        else if (type.DataType.StartsWith("System.Net"))
                        {
                            type.Type = systemNetAssembly.GetType(type.DataType);
                            type.Using = "System.Net";
                        }
                        else if (type.DataType.StartsWith("Newtonsoft.Json.Linq"))
                        {
                            type.Type = newtonsoftJsonLinqAssembly.GetType(type.DataType);
                            type.Using = "Newtonsoft.Json.Linq";
                        }
                    }
                }

                types[type.OID] = type;
            }

            return types;
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

                    foreach (var type in TypeHelper.FindNullableTypes(types))
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
    }
}

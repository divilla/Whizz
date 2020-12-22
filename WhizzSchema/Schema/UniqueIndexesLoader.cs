using System.Collections.Generic;
using Npgsql;
using Utf8Json;
using WhizzSchema.Entities;

namespace WhizzSchema.Schema
{
    public static class UniqueIndexesLoader
    {
        public static IEnumerable<UniqueIndexEntity> Load(NpgsqlConnection connection)
        {
            const string sql = @"
                SELECT json_agg(t)
                FROM (SELECT ns.nspname::text AS ""schemaName"",
                       tc.relname::text AS ""relationName"",
                       ic.relname::text AS ""indexName"",
                       array_agg(a.attname)::text[] AS ""columnNames""
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
                    ns.nspname::text, 
                    tc.relname::text, 
                    ic.relname::text) t;";
            
            var command = new NpgsqlCommand(sql, connection);
            var json = command.ExecuteScalar().ToString();
            
            return JsonSerializer.Deserialize<IEnumerable<UniqueIndexEntity>>(json);
        }
    }
}
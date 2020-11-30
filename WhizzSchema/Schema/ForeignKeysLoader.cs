using System.Collections.Generic;
using Npgsql;
using Utf8Json;
using WhizzSchema.Entities;

namespace WhizzSchema.Schema
{
    public static class ForeignKeysLoader
    {
        public static IEnumerable<ForeignKeyEntity> Load(NpgsqlConnection connection)
        {
            const string sql = @"
                SELECT json_agg(t)
                    FROM (SELECT ns.nspname::text                   AS ""schema_name"",
                           c.relname::text                          AS ""table_name"",
                           a.attname::text                          AS ""column_name"",
                           ct.conname::text                         AS ""constraint_name"",
                           fns.nspname::text                        AS ""primary_key_schema_name"",
                           fc.relname::text                         AS ""primary_key_table_name"",
                           fa.attname::text                         AS ""primary_key_column_name""
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
                        a.attnum) t;";
            
            var command = new NpgsqlCommand(sql, connection);
            var json = command.ExecuteScalar().ToString();
            
            return JsonSerializer.Deserialize<IEnumerable<ForeignKeyEntity>>(json);
        }
    }
}
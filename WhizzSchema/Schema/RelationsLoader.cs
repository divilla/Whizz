using System.Collections.Generic;
using Npgsql;
using Utf8Json;
using WhizzSchema.Entities;

namespace WhizzSchema.Schema
{
    public static class RelationsLoader
    {
        public static IEnumerable<RelationEntity> Load(NpgsqlConnection connection)
        {
            const string sql = @"
                SELECT json_agg(t)
                FROM (SELECT
                        n.nspname::text                                                                          AS ""schemaName"",
                        c.relname::text                                                                          AS ""relationName"",
                        pg_get_userbyid(c.relowner)::text                                                        AS ""owner"",
                        c.relkind::text                                                                          AS ""kind"",
                        has_table_privilege(quote_ident(n.nspname)||'.'||quote_ident(c.relname), 'SELECT'::text) AS ""canSelect"",
                        has_table_privilege(quote_ident(n.nspname)||'.'||quote_ident(c.relname), 'INSERT'::text) AS ""canInsert"",
                        has_table_privilege(quote_ident(n.nspname)||'.'||quote_ident(c.relname), 'UPDATE'::text) AS ""canUpdate"",
                        has_table_privilege(quote_ident(n.nspname)||'.'||quote_ident(c.relname), 'DELETE'::text) AS ""canDelete"",
                        CASE
                            WHEN (c.relkind = ANY (ARRAY ['r', 'p'])) OR
                                (c.relkind = ANY (ARRAY ['v', 'f']))
                                AND (pg_relation_is_updatable(c.oid::regclass, false) & 8) = 8 THEN false
                            ELSE true
                            END::bool                                                                            AS ""isReadonly""
                    FROM pg_class c
                        LEFT JOIN pg_namespace n ON n.oid = c.relnamespace
                    WHERE
                        c.relkind IN ('r', 'p', 'f', 'v', 'm')
                        AND n.nspname NOT LIKE 'pg_%' AND n.nspname != 'information_schema'
                        AND (pg_has_role(c.relowner, 'USAGE'::text) OR has_table_privilege(quote_ident(n.nspname)||'.'||quote_ident(c.relname), 'SELECT'::text))
                    ORDER BY 
                        n.nspname,
                        c.relname) t;";
            
            var command = new NpgsqlCommand(sql, connection);
            var json = command.ExecuteScalar().ToString();
            
            return JsonSerializer.Deserialize<IEnumerable<RelationEntity>>(json);
        }
    }
}
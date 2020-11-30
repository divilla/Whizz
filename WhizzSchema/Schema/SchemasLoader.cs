using System.Collections.Generic;
using Npgsql;
using Utf8Json;
using WhizzSchema.Entities;

namespace WhizzSchema.Schema
{
    public static class SchemasLoader
    {
        public static IEnumerable<SchemaEntity> Load(NpgsqlConnection connection)
        {
            const string sql = @"
                SELECT json_agg(t)
                FROM
                    (SELECT 
                        n.nspname::text                                         AS ""schemaName"",
                        pg_get_userbyid(n.nspowner)                             AS ""schemaOwner"",
                        pg_catalog.has_schema_privilege(n.nspname, 'USAGE')     AS ""canUse"",
                        pg_catalog.has_schema_privilege(n.nspname, 'CREATE')    AS ""canCreate""
                    FROM pg_catalog.pg_namespace n
                    WHERE
                        n.nspname NOT LIKE 'pg_%' AND n.nspname != 'information_schema'
                    ORDER BY
                        n.nspname) t;";
            
            var command = new NpgsqlCommand(sql, connection);
            var json = command.ExecuteScalar().ToString();
            return JsonSerializer.Deserialize<IEnumerable<SchemaEntity>>(json);
        }
    }
}
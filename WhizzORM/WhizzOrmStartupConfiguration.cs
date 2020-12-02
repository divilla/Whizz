using System.Collections.Generic;
using System.Reflection;
using Npgsql;
using WhizzORM.Interfaces;

namespace WhizzORM
{
    public class WhizzOrmStartupConfiguration<TDbContext> where TDbContext : IDbContext
    {
        public NpgsqlConnection Connection { get; set; }
        public TDbContext Context { get; set; }
        public HashSet<Assembly> Assemblies { get; } = new HashSet<Assembly>();
    }
}
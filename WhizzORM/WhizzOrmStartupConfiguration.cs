using System.Collections.Generic;
using System.Reflection;
using Npgsql;
using WhizzORM.Context;
using WhizzORM.Interfaces;

namespace WhizzORM
{
    public class WhizzOrmStartupConfiguration
    {
        public NpgsqlConnection Connection { get; set; }
        public IRepository Repository { get; set; }
        public JsonRepository JsonRepository { get; set; }
        public HashSet<Assembly> Assemblies { get; } = new HashSet<Assembly>();
    }
}
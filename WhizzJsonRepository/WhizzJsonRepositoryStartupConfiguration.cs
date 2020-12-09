using System.Collections.Generic;
using System.Reflection;
using Npgsql;
using WhizzJsonRepository.Interfaces;
using WhizzJsonRepository.Repository;

namespace WhizzJsonRepository
{
    public class WhizzOrmStartupConfiguration
    {
        public NpgsqlConnection Connection { get; set; }
        public IRepository Repository { get; set; }
        public HashSet<Assembly> Assemblies { get; } = new HashSet<Assembly>();
    }
}
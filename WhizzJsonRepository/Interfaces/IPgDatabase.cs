using System;
using System.Threading.Tasks;
using Npgsql;
using WhizzBase.Enums;
using WhizzORM.Interfaces;
using WhizzSchema;

namespace WhizzJsonRepository.Interfaces
{
    public interface IPgDatabase
    {
        string ConnectionString { get; }
        DbSchema Schema { get; }
        Func<string, string> Quote { get; }
        Func<string, string> ToDbCase { get; }
        Func<string, string> ToQuotedDbCase { get; }
        Case JsonCase { get; }
        Func<string, string> ToJsonCase { get; }
        Func<string, string> ToQuotedJsonCase { get; }
        IPgTypeValidator TypeValidator { get; set; }
        PgValidationErrorMessages ErrorMessages { get; set; }
        NpgsqlConnection OpenConnection();
        Task<NpgsqlConnection> OpenConnectionAsync();
    }
}
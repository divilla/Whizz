using System;
using WhizzBase.Enums;
using WhizzORM.Interfaces;
using WhizzSchema;

namespace WhizzJsonRepository.Interfaces
{
    public interface IDatabase
    {
        string ConnectionString { get; }
        DbSchema Schema { get; }
        Func<string, string> Quote { get; }
        Func<string, string> ToDbCase { get; }
        Func<string, string> ToQuotedDbCase { get; }
        Case JsonCase { get; }
        Func<string, string> ToJsonCase { get; }
        Func<string, string> ToQuotedJsonCase { get; }
        ITypeValidator TypeValidator { get; set; }
        ValidationErrorMessages ErrorMessages { get; set; }
        string QuotedRelationName(string relationName, string schemaName);
    }
}
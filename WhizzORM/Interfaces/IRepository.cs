using System;
using Npgsql;
using WhizzBase.Base;
using WhizzBase.Extensions;
using WhizzORM.Context;
using WhizzSchema.Interfaces;

namespace WhizzORM.Interfaces
{
    public interface IRepository
    {
        NpgsqlConnection Connection { get; }
        IDbSchema Schema { get; }
        public virtual Case DbCase => Case.Snake;
        public virtual Func<string, string> DbCaseResolver => (s) => s.ToSnakeCase();
        InsertCommand<TData> Insert<TData>(string relationName, TData data);
        InsertCommand<TData> Insert<TData>(string relationName, string schemaName, TData data);
    }
}
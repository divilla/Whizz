using System;
using Npgsql;
using WhizzBase.Enums;
using WhizzBase.Extensions;
using WhizzJsonRepository.Repository;
using WhizzSchema.Interfaces;

namespace WhizzJsonRepository.Interfaces
{
    public interface IRepository
    {
        NpgsqlConnection Connection { get; }
        IDbSchema Schema { get; }
        public virtual Case DbCase => Case.Snake;
        public virtual Func<string, string> DbCaseResolver => (s) => s.ToSnakeCase();
        public FindJsonInvoker Find(string relationName, string schemaName = IDbSchema.DefaultSchema);
    }
}
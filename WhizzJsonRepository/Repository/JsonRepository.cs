using System;
using System.Linq;
using Npgsql;
using WhizzJsonRepository.Services;
using WhizzSchema.Entities;

namespace WhizzJsonRepository.Repository
{
    public abstract class JsonRepository<TDb>
        where TDb : PgDatabase, new()
    {
        public JsonRepository()
        {
            Init();
        }
        
        public abstract TDb Db { get; }
        public abstract PgConnectionManager<TDb> ConnectionManager { get; }
        public abstract string RelationName { get; }
        public abstract string SchemaName { get; }
        public virtual ColumnEntity[] PrimaryKeyColumns { get; private set; }

        public NpgsqlConnection Connection => ConnectionManager.Connection;

        private void Init()
        {
            if (PrimaryKeyColumns != null && PrimaryKeyColumns.Length > 0)
                return;
            
            PrimaryKeyColumns = Db.Schema
                .GetColumns(RelationName, SchemaName)
                .Where(s => s.IsPrimaryKey)
                .ToArray();
            
            if (PrimaryKeyColumns.Length == 0)
                throw new NotImplementedException("Cannot resolve 'PrimaryKeyColumns', probably because relation is view. Please implement 'PrimaryKeyColumns' manually.");
        }

        public FindJsonInvoker Find()
        {
            return new FindJsonInvoker(this);
        }

        public InsertJsonInvoker InsertInto()
        {
            return new InsertJsonInvoker(this);
        }
    }
}

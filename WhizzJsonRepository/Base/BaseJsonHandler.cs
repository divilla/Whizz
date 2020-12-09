using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using WhizzBase.Enums;
using WhizzJsonRepository.Handlers;
using WhizzJsonRepository.Repository;
using WhizzSchema.Entities;
using WhizzSchema.Interfaces;

namespace WhizzJsonRepository.Base
{
    public abstract class BaseJsonHandler
    {
        protected JsonResponseState State => _state;
        private JsonResponseState _state;
        protected JsonRepository Repository { get; private set; }
        protected Func<string, string> DbQuote => Repository.Schema.Quote;
        protected Func<string, string> ToDbCase => Repository.ToDbCase;
        protected Func<string, string> ToJsonCase => Repository.ToJsonCase;
        protected string RelationName { get; private set; }
        protected string SchemaName { get; private set; }
        protected ImmutableArray<ColumnEntity> Columns { get; private set; }
        protected string QuotedRelationName => Repository.Schema.QuotedRelationName(RelationName, SchemaName);
        protected IEnumerable<string> AllColumnNames => Repository.Schema.GetColumns(RelationName, SchemaName).Select(s => s.ColumnName);
        protected IEnumerable<string> ColumnNames => Columns.Select(s => s.ColumnName);
        protected MandatoryColumns MandatoryColumns { get; private set; }

        public static BaseJsonHandler Init(ref JsonResponseState state, JsonRepository repository, ImmutableArray<ColumnEntity> columns, MandatoryColumns mandatoryColumns = MandatoryColumns.None, string relationName = null, string schemaName = IDbSchema.DefaultSchema)
        {
            return new PrepareJsonRequestHandler()
                .SetRequest(ref state, repository, columns, mandatoryColumns, relationName, schemaName)
                .Handle();
        }
        
        public BaseJsonHandler Next<T>()
            where T : BaseJsonHandler, new()
        {
            if (!State.Continue) 
                return (T) this;
            
            return new T()
                .SetRequest(ref _state, Repository, Columns, MandatoryColumns, RelationName, SchemaName)
                .Handle();
        }
        
        public async Task<BaseJsonHandler> NextAsync<T>()
            where T : BaseJsonHandler, new()
        {
            if (!State.Continue) 
                return (T) this;
            
            return await new T()
                .SetRequest(ref _state, Repository, Columns, MandatoryColumns, RelationName, SchemaName)
                .HandleAsync();
        }

        protected virtual BaseJsonHandler Handle()
        {
            return this;
        }

        protected virtual Task<BaseJsonHandler> HandleAsync()
        {
            return Task<BaseJsonHandler>.Factory.StartNew(() => this);
        }

        protected string AllColumnNamesSelect(string prefix = "")
        {
            if (prefix != "")
                prefix = $"{prefix}.";
            
            var list = new List<string>();
            foreach (var columnName in AllColumnNames)
            {
                var quotedJsonName = DbQuote(ToJsonCase(columnName));
                list.Add(quotedJsonName == columnName ? $"{prefix}{columnName}" : $"{prefix}{DbQuote(columnName)} AS {quotedJsonName}");
            }

            return string.Join(", ", list);
        }
        
        private BaseJsonHandler SetRequest(ref JsonResponseState state, JsonRepository repository, ImmutableArray<ColumnEntity> columns, MandatoryColumns mandatoryColumns, string relationName, string schemaName)
        {
            _state = state;
            Repository = repository;
            Columns = columns;
            MandatoryColumns = mandatoryColumns;
            RelationName = relationName;
            SchemaName = schemaName;
         
            return this;
        }
    }
}

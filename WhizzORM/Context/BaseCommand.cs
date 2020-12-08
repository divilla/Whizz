using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using Utf8Json;
using Utf8Json.Resolvers;
using WhizzBase.Base;
using WhizzBase.Extensions;
using WhizzBase.Reflection;
using WhizzORM.Interfaces;

namespace WhizzORM.Context
{
    public class BaseCommand<TDbCommand, TData>
    {
        public BaseCommand(IRepository repository, TData data)
        {
            Repository = repository;
            Data = data;

            Connection = repository.Connection;
            CancellationToken = new CancellationToken();
            if (Connection.State == ConnectionState.Closed || Connection.State == ConnectionState.Broken)
                Connection.OpenAsync(CancellationToken).Wait();
        }
        
        protected IRepository Repository { get; }
        protected NpgsqlConnection Connection { get; }
        protected NpgsqlCommand Command { get; private set; } 
        protected CancellationToken CancellationToken { get; private set; } 
        protected TData Data { get; }

        protected static ConcurrentDictionary<string, NpgsqlCommand> CommandCache = new ConcurrentDictionary<string, NpgsqlCommand>();
        
        private string _sql;
        private string _refreshTableName;
        private bool _isolationLevel = false;
        private string _retryHandler = null;

        public async Task SetCommand(NpgsqlCommand command)
        {
            Command.Dispose();
            Command = command;
            Command.Connection = Repository.Connection;
            if (!Command.IsPrepared)
                await Command.PrepareAsync();
        }
        
        public async Task<BaseCommand<TDbCommand, TData>> SetSql(string sql)
        {
            if (sql == _sql)
                return this;

            Cancel();
            Reset();
            _sql = sql;
            Command = new NpgsqlCommand(_sql, Connection);
            NpgsqlCommandBuilder.DeriveParameters(Command);
            if (!Command.IsPrepared)
                await Command.PrepareAsync();

            return this;
        }

        protected void BindParameters(Type type)
        {
            var getters = ReflectionFactory.GetterDelegatesByPropertyName(type, Repository.DbCase);
            foreach (NpgsqlParameter commandParameter in Command.Parameters)
            {
                commandParameter.Value = getters[commandParameter.ParameterName](Data);
            }
        }
        
        public void Cancel()
        {
            Command?.Cancel();
        }

        protected void Reset()
        {
            _refreshTableName = null;
            _isolationLevel = false;
            _retryHandler = "";
        }
    }
}
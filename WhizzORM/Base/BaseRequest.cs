using System;
using MediatR;
using WhizzORM.Context;
using WhizzORM.Handlers;
using WhizzORM.Interfaces;

namespace WhizzORM.Base
{
    public class BaseRequest<TEntity>
        where TEntity : class, new()
    {
        public BaseRequest(EntitySchema schema)
        {
            Schema = schema;
        }

        public EntitySchema Schema { get; }
    }
}
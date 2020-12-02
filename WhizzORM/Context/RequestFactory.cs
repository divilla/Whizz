using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using WhizzBase.Helpers;
using WhizzORM.Interfaces;
using WhizzORM.Requests;
using WhizzSchema;
using WhizzSchema.Entities;
using WhizzSchema.Interfaces;

namespace WhizzORM.Context
{
    public class RequestFactory<TEntity>
        where TEntity : class, new()
    {
        public RequestFactory(ref IDbContext dbContext)
        {
            _requestType = typeof(TEntity);
            _dbContext = dbContext;
        }

        private Type _requestType;
        private Type _responseType;
        private IDbContext _dbContext;
        
        public GetAllRequest<List<TEntity>> GetAll()
        {
            return new GetAllRequest<List<TEntity>>();
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using WhizzORM.Base;
using WhizzORM.Context;
using WhizzORM.Handlers;

namespace WhizzORM.Requests
{
    public class GetAllRequest<TEntity> : BaseRequest<TEntity>
        where TEntity : class, new()
    {
        public GetAllRequest(EntitySchema schema) : base(schema)
        {}
        
        public async Task<List<TEntity>> Handle()
        {
            return await GetAllHandler.Handle(this);
        }
    }
}
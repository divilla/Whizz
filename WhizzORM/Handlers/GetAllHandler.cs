using System.Collections.Generic;
using System.Threading.Tasks;
using WhizzORM.Requests;

namespace WhizzORM.Handlers
{
    public static class GetAllHandler
    {
        public static async Task<List<TEntity>> Handle<TEntity>(GetAllRequest<TEntity> request)
            where TEntity : class, new()
        {
            return null;
        }
    }
}
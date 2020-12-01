using WhizzORM.Base;

namespace WhizzORM.Requests
{
    public class GetAllRequest<TContext, TEntity> : BaseRequest<TContext, TEntity>
        where TEntity : class, new()
    {
        public GetAllRequest(ref TContext context) : base(ref context) 
        { }
    }
}
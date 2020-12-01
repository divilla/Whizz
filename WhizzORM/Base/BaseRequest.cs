using System;

namespace WhizzORM.Base
{
    public class BaseRequest<TContext, TEntity>
        where TEntity : class, new()
    {
        public BaseRequest(ref TContext context)
        {
            Context = context;
            EntityType = typeof(TEntity);
        }

        public TContext Context { get; }
        public Type EntityType { get; }
    }
}
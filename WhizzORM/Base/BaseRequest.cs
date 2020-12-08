using WhizzORM.Context;

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
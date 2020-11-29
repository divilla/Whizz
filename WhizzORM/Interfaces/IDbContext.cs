using WhizzORM.Context;

namespace WhizzORM.Interfaces
{
    public interface IDbContext
    {
        EntityMapBuilder<TEntity> Set<TEntity>() where TEntity : class, new();
    }
}
using System.Threading.Tasks;
using WhizzORM.Base;

namespace WhizzORM.Interfaces
{
    public interface IIdRepository<TId> : IRepository
    {
        T Find<T>(TId id) where T : BaseIdEntity<TId>, IAggregateRoot;
        Task<T> FindAsync<T>(TId id) where T : BaseIdEntity<TId>, IAggregateRoot;
    }
}
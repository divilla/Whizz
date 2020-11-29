using System.Threading.Tasks;
using WhizzORM.Base;

namespace WhizzORM.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(BaseDomainEvent domainEvent);
    }
}
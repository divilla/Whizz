using System.Threading.Tasks;
using WhizzORM.Base;

namespace WhizzORM.Interfaces
{
    public interface IEventHandler<in T> where T : BaseDomainEvent
    {
        Task Handle(T domainEvent);
    }
}
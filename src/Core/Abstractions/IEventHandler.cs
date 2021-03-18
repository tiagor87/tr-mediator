using System.Threading;
using System.Threading.Tasks;

namespace TRMediator.Core.Abstractions
{
    public interface IEventHandler<in TEvent> : IHandler where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
    }
}
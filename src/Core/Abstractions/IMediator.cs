using System.Threading;
using System.Threading.Tasks;

namespace TRMediator.Core.Abstractions
{
    public interface IMediator
    {
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
            where TEvent : IEvent;

        Task<TResponse> SendAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
            where TCommand : ICommand<TResponse>;

        Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand;
    }
}
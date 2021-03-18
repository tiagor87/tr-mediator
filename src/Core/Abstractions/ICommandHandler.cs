using System.Threading;
using System.Threading.Tasks;

namespace TRMediator.Core.Abstractions
{
    public interface ICommandHandler<TCommand> : IHandler where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, CancellationToken cancellationToken);
    }
    
    public interface ICommandHandler<TCommand, TResponse> : IHandler where TCommand : ICommand<TResponse>
    {
        Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken);
    }
}
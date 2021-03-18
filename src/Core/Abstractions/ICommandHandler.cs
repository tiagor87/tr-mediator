using System.Threading;
using System.Threading.Tasks;

namespace TRMediator.Core.Abstractions
{
    public interface ICommandHandler<in TCommand> : IHandler where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, CancellationToken cancellationToken);
    }
    
    public interface ICommandHandler<in TCommand, TResponse> : IHandler where TCommand : ICommand<TResponse>
    {
        Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken);
    }
}
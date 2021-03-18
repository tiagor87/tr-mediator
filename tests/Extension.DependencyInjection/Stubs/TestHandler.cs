using System;
using System.Threading;
using System.Threading.Tasks;
using TRMediator.Core.Abstractions;
using TRMediator.Extensions.DependencyInjection.Tests.Abstractions;

namespace TRMediator.Extensions.DependencyInjection.Tests.Stubs
{
    public class TestHandler : IEventHandler<ITestCommandEvent>, ICommandHandler<ITestCommandEvent>, ICommandHandler<ITestCommandEvent, bool>
    {
        Task IEventHandler<ITestCommandEvent>.HandleAsync(ITestCommandEvent @event, CancellationToken cancellationToken)
        {
            return HandleAsync(@event, cancellationToken);
        }

        Task ICommandHandler<ITestCommandEvent>.HandleAsync(ITestCommandEvent command, CancellationToken cancellationToken)
        {
            return HandleAsync(command, cancellationToken);
        }

        public Task<bool> HandleAsync(ITestCommandEvent command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
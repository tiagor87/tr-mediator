using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TRMediator.Core.Abstractions;
using TRMediator.Core.Extensions;

namespace TRMediator.Core
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
        
        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent
        {
            var handlers = (IEnumerable<IEventHandler<TEvent>>) _serviceProvider.GetService(typeof(IEnumerable<IEventHandler<TEvent>>));
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event, cancellationToken);
            }
        }

        public async Task<TResponse> SendAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand<TResponse>
        {
            var handler = (ICommandHandler<TCommand, TResponse>) _serviceProvider.GetService(typeof(ICommandHandler<TCommand, TResponse>));
            handler.EnsureNotNull();
            return await handler.HandleAsync(command, cancellationToken);
        }

        public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand
        {
            var handler = (ICommandHandler<TCommand>) _serviceProvider.GetService(typeof(ICommandHandler<TCommand>));
            handler.EnsureNotNull();
            await handler.HandleAsync(command, cancellationToken);
        }
    }
}
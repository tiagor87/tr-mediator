using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TRMediator.Core.Abstractions;
using TRMediator.Core.Tests.Abstractions;

namespace TRMediator.Core.Tests.Fixtures
{
    public class MediatorFixture : IDisposable
    {
        private readonly ConcurrentBag<MediatorScopeFixture> _scopes;

        public MediatorFixture()
        {
            _scopes = new ConcurrentBag<MediatorScopeFixture>();
        }

        public IMediatorFixture CreateScope()
        {
            var scope = new MediatorScopeFixture();
            _scopes.Add(scope);
            return scope;
        }

        public void Dispose()
        {
            while (!_scopes.IsEmpty)
            {
                if (_scopes.TryTake(out var scope))
                {
                    scope.Dispose();
                }
            }
        }

        private class MediatorScopeFixture : IMediatorFixture
        {
            private readonly IServiceCollection _services;
            private readonly ConcurrentBag<IServiceScope> _scopes;

            public MediatorScopeFixture()
            {
                _scopes = new ConcurrentBag<IServiceScope>();
                _services = new ServiceCollection();
                AddInjections();
                EventHandlerMock = new Mock<IEventHandler<ITestEvent>>(MockBehavior.Strict);
                CommandHandlerMock = new Mock<ICommandHandler<ITestCommand>>(MockBehavior.Strict);
                CommandWithResponseHandlerMock =
                    new Mock<ICommandHandler<ITestWithResponseCommand, bool>>(MockBehavior.Strict);
            }

            public Mock<ICommandHandler<ITestCommand>> CommandHandlerMock { get; }
            public Mock<ICommandHandler<ITestWithResponseCommand, bool>> CommandWithResponseHandlerMock { get; }
            public Mock<IEventHandler<ITestEvent>> EventHandlerMock { get; }

            public IMediator GetMediator()
            {
                var serviceProvider = _services.BuildServiceProvider();
                var scope = serviceProvider.CreateScope();
                _scopes.Add(scope);
                return scope.ServiceProvider.GetService<IMediator>();
            }

            public void Register<T>(T obj) where T : class
            {
                _services.AddScoped(_ => obj);
            }

            private void AddInjections()
            {
                _services
                    .AddScoped(_ => EventHandlerMock.Object)
                    .AddScoped(_ => CommandHandlerMock.Object)
                    .AddScoped(_ => CommandWithResponseHandlerMock.Object)
                    .AddScoped<IMediator, Mediator>();
            }

            public void Dispose()
            {
                while (!_scopes.IsEmpty)
                {
                    if (_scopes.TryTake(out var scope))
                    {
                        scope.Dispose();
                    }
                }
            }
        }
    }
}
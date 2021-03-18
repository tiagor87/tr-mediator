using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TRMediator.Core.Abstractions;
using TRMediator.Core.Exceptions;
using TRMediator.Core.Tests.Abstractions;
using TRMediator.Core.Tests.Fixtures;
using Xunit;

namespace TRMediator.Core.Tests
{
    public class MediatorTests : IClassFixture<MediatorFixture>
    {
        private readonly MediatorFixture _mediatorFixture;
        private readonly IMediator _mediator;
        private readonly IMediatorFixture _mediatorScopeFixture;

        public MediatorTests(MediatorFixture mediatorFixture)
        {
            _mediatorFixture = mediatorFixture;
            _mediatorScopeFixture = mediatorFixture.CreateScope();
            _mediator = _mediatorScopeFixture.GetMediator();
        }

        [Fact]
        public async Task GivenEventWhenPublishShouldCallHandler()
        {
            var eventMock = new Mock<ITestEvent>();
            _mediatorScopeFixture.EventHandlerMock.Setup(x =>
                x.HandleAsync(eventMock.Object, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _mediator.PublishAsync(eventMock.Object, It.IsAny<CancellationToken>());
            
            _mediatorScopeFixture.EventHandlerMock.VerifyAll();
        }
        
        [Fact]
        public async Task GivenEventWhenPublishShouldCallEachRegisteredHandler()
        {
            var handlerMock = new Mock<IEventHandler<ITestEvent>>(MockBehavior.Strict);
            var eventMock = new Mock<ITestEvent>();
            var scope = _mediatorFixture.CreateScope();
            scope.Register(handlerMock.Object);
            var mediator = scope.GetMediator();
            
            handlerMock.Setup(x =>
                    x.HandleAsync(eventMock.Object, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            scope.EventHandlerMock.Setup(x =>
                    x.HandleAsync(eventMock.Object, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await mediator.PublishAsync(eventMock.Object, It.IsAny<CancellationToken>());
            
            handlerMock.VerifyAll();
            scope.EventHandlerMock.VerifyAll();
        }
        
        [Fact]
        public async Task GivenEventWhenPublishAndHandlerNotFoundShouldNotThrow()
        {
            var eventMock = new Mock<INotRegisteredHandlerEvent>();

            Func<Task> action = async () => await _mediator.PublishAsync(eventMock.Object, It.IsAny<CancellationToken>());
            
            await action.Should().NotThrowAsync();
        }
        
        [Fact]
        public async Task GivenCommandWhenSendShouldCallHandler()
        {
            var commandMock = new Mock<ITestCommand>();
            _mediatorScopeFixture.CommandHandlerMock.Setup(x =>
                    x.HandleAsync(commandMock.Object, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _mediator.SendAsync(commandMock.Object, It.IsAny<CancellationToken>());
            
            _mediatorScopeFixture.CommandHandlerMock.VerifyAll();
        }
        
        [Fact]
        public async Task GivenCommandWhenSendAndHandlerNotFoundShouldThrow()
        {
            var commandMock = new Mock<INotRegisteredHandlerCommand>();

            Func<Task> action = async () => await _mediator.SendAsync(commandMock.Object, It.IsAny<CancellationToken>());
            
            await action.Should().ThrowAsync<HandlerNotFoundException<ICommandHandler<INotRegisteredHandlerCommand>>>();
        }
        
        [Fact]
        public async Task GivenCommandWithResponseWhenSendShouldCallHandlerAndReturns()
        {
            var commandMock = new Mock<ITestWithResponseCommand>();
            _mediatorScopeFixture.CommandWithResponseHandlerMock.Setup(x =>
                    x.HandleAsync(commandMock.Object, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .Verifiable();

            var response = await _mediator.SendAsync<ITestWithResponseCommand, bool>(commandMock.Object, It.IsAny<CancellationToken>());

            response.Should().BeTrue();
            _mediatorScopeFixture.CommandHandlerMock.VerifyAll();
        }
        
        [Fact]
        public async Task GivenCommandWithResponseWhenSendAndHandlerNotFoundShouldThrow()
        {
            var commandMock = new Mock<INotRegisteredHandlerWithResponseCommand>();

            Func<Task<bool>> action = async () => await _mediator.SendAsync<INotRegisteredHandlerWithResponseCommand, bool>(commandMock.Object, It.IsAny<CancellationToken>());
            
            await action.Should().ThrowAsync<HandlerNotFoundException<ICommandHandler<INotRegisteredHandlerWithResponseCommand, bool>>>();
        }
    }
}

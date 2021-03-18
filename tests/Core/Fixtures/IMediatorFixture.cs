using Moq;
using TRMediator.Core.Abstractions;
using TRMediator.Core.Tests.Abstractions;

namespace TRMediator.Core.Tests.Fixtures
{
    public interface IMediatorFixture
    {
        void Register<T>(T obj) where T : class;
        IMediator GetMediator();
        Mock<IEventHandler<ITestEvent>> EventHandlerMock { get; }
        Mock<ICommandHandler<ITestCommand>> CommandHandlerMock { get; }
        Mock<ICommandHandler<ITestWithResponseCommand, bool>> CommandWithResponseHandlerMock { get; }
    }
}
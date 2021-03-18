using TRMediator.Core.Abstractions;

namespace TRMediator.Extensions.DependencyInjection.Tests.Abstractions
{
    public interface ITestCommandEvent : IEvent, ICommand, ICommand<bool>
    {
    }
}
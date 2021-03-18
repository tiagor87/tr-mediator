using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TRMediator.Core.Abstractions;
using TRMediator.Extensions.DependencyInjection.Tests.Abstractions;
using Xunit;

namespace TRMediator.Extensions.DependencyInjection.Tests
{
    public class DependencyInjectionTests
    {
        private readonly ServiceProvider _provider;

        public DependencyInjectionTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMediator(Assembly.GetExecutingAssembly());
            _provider = services.BuildServiceProvider();
        }
        
        [Fact]
        public void GivenProviderWhenGetEventHandlerShouldReturnInstance()
        {
            var handler = _provider.GetService<IEventHandler<ITestCommandEvent>>();
            handler.Should().NotBeNull();
        }
        
        [Fact]
        public void GivenProviderWhenGetCommandHandlerShouldReturnInstance()
        {
            var handler = _provider.GetService<ICommandHandler<ITestCommandEvent>>();
            handler.Should().NotBeNull();
        }
        
        [Fact]
        public void GivenProviderWhenGetCommandWithResponseHandlerShouldReturnInstance()
        {
            var handler = _provider.GetService<ICommandHandler<ITestCommandEvent, bool>>();
            handler.Should().NotBeNull();
        }
    }
}

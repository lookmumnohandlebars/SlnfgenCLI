using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;
using Slnfgen.CLI.Presentation.StartUp;

namespace Slnfgen.CLI.UnitTests.Presentation.Startup;

public class DependencyInjectionTests
{
    private ServiceProvider _serviceProvider;

    public DependencyInjectionTests()
    {
        _serviceProvider = new ServiceCollection().AddAllDependencies().BuildServiceProvider();
    }

    [Fact]
    public void AddAllDependencies_should_be_able_to_resolve_every_known_command()
    {
        var requestTypes = typeof(IRequestHandler<>)
            .Assembly.GetTypes()
            .Where(t => t.IsAssignableFrom(typeof(IRequestHandler<>)));

        foreach (var requestType in requestTypes)
        {
            var act = () => _serviceProvider.GetService(requestType);
            act.Should().NotThrow("All implementations of IRequestHandler must be able to be bound");
        }
    }
}

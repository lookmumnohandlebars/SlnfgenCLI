using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Slnfgen.CLI.Application.Common.Requests;
using Slnfgen.CLI.Application.Requests.GenerateAll;
using Slnfgen.CLI.Presentation.Init;

namespace Slnfgen.CLI.UnitTests.Presentation.Init;

public class DependencyInjectionTests
{
    private readonly ServiceProvider _serviceProvider = new ServiceCollection()
        .AddAllDependencies()
        .BuildServiceProvider();

    [Fact]
    public void AddAllDependencies_should_be_able_to_resolve_every_known_command()
    {
        var implementations = FindImplementationsOfGenericInterface(
            typeof(GenerateSolutionFiltersResponse).Assembly,
            typeof(IRequestHandler<,>)
        );
        foreach (var implementation in implementations)
        {
            var act = () => _serviceProvider.GetRequiredService(implementation);
            act.Should().NotThrow($"Should be able to resolve {implementation.FullName} from the service provider");
        }
    }

    private static IEnumerable<Type> FindImplementationsOfGenericInterface(Assembly assembly, Type genericInterface)
    {
        return assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, IsGenericType: false })
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterface))
            .SelectMany(t =>
                t.FindInterfaces(
                    (interfaceType, _) => interfaceType.GetGenericTypeDefinition() == genericInterface,
                    null
                )
            );
    }
}

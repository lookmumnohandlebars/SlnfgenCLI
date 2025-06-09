using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slnfgen.Application.Module;
using Slnfgen.CLI.Domain;

namespace Slnfgen.CLI.Presentation.StartUp;

/// <summary>
///     Extensions for adding dependencies directly to the Services in the application builder
///     of the program
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Add all dependencies to a service collection
    /// </summary>
    /// <param name="services">Service Collection coming from the application builder</param>
    /// <returns>The same service collection with dependencies added</returns>
    public static IServiceCollection AddAllDependencies(this IServiceCollection services)
    {
        services.AddLogging(opt => opt.AddConsole());
        services.AddDomainDependencies();
        services.AddApplicationDependencies();
        return services;
    }
}

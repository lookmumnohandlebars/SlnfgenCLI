using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slnfgen.CLI.Application;
using Slnfgen.CLI.Domain;

namespace Slnfgen.CLI.Presentation.Init;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAllDependencies(this IServiceCollection services)
    {
        services.AddLogging(opt => opt.AddConsole());
        services.AddTransient<ILogger>(s => s.GetRequiredService<ILogger<Program>>());
        services.AddDomainDependencies();
        services.AddApplicationDependencies();
        return services;
    }
}

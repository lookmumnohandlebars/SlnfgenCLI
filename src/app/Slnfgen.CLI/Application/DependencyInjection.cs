using Microsoft.Extensions.DependencyInjection;
using Slnfgen.CLI.Application.Features;
using Slnfgen.CLI.Application.Repository;

namespace Slnfgen.Application.Module;

internal static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddApplicationSolutionFilterDependencies();
        services.AddApplicationRepositoryDependencies();
        return services;
    }
}

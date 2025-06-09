using Microsoft.Extensions.DependencyInjection;
using Slnfgen.CLI.Domain.Solution;

namespace Slnfgen.CLI.Domain;

internal static class DomainModule
{
    public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
    {
        services.AddDomainSolutionDependencies();
        return services;
    }
}

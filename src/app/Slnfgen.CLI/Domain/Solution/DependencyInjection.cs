using Microsoft.Extensions.DependencyInjection;
using Slnfgen.Application.Features.SolutionFilterGeneration;

namespace Slnfgen.CLI.Domain.Solution;

/// <summary>
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDomainSolutionDependencies(this IServiceCollection services)
    {
        services.AddTransient<SolutionFilterGenerator>();
        return services;
    }
}

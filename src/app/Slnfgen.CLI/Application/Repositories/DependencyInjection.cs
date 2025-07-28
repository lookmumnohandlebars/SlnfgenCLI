using Microsoft.Extensions.DependencyInjection;
using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Features.SolutionFilterGeneration;
using Slnfgen.CLI.Application.Repository.Solution.Project;
using Slnfgen.CLI.Domain.Solution.File.Loader;
using Slnfgen.CLI.Domain.Solution.Project.Repositories;

namespace Slnfgen.CLI.Application.Repository;

internal static class DependencyInjection
{
    public static IServiceCollection AddApplicationRepositoryDependencies(this IServiceCollection services)
    {
        services.AddScoped<ISolutionFiltersManifestLoader, SolutionFiltersManifestFileLoader>();
        services.AddScoped<ISolutionLoader, SolutionFileLoader>();
        services.AddScoped<ISolutionFilterWriter, SolutionFilterFileWriter>();
        services.AddScoped<IProjectFileLoader, ProjectFileLoader>();
        return services;
    }
}

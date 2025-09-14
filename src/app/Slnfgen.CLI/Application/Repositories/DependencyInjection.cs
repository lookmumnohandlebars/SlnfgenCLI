using Microsoft.Extensions.DependencyInjection;
using Slnfgen.CLI.Application.Repositories.Manifest.SolutionFiltersManifest;
using Slnfgen.CLI.Application.Repositories.Solution.File;
using Slnfgen.CLI.Application.Repositories.Solution.Filter;
using Slnfgen.CLI.Application.Repositories.Solution.Project;
using Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Repository;
using Slnfgen.CLI.Domain.Solution.File.Repository;
using Slnfgen.CLI.Domain.Solution.Filter.Repository;
using Slnfgen.CLI.Domain.Solution.Project.Repositories;

namespace Slnfgen.CLI.Application.Repositories;

internal static class DependencyInjection
{
    public static IServiceCollection AddApplicationRepositoryDependencies(this IServiceCollection services)
    {
        services.AddScoped<ISolutionFiltersManifestLoader, SolutionFiltersManifestFileLoader>();
        services.AddScoped<ISolutionLoader, SolutionFileLoader>();
        services.AddScoped<ISolutionFilterWriter, SolutionFilterFileWriter>();
        services.AddScoped<IXmlSolutionFileWriter, XmlSolutionFileWriter>();
        services.AddScoped<IProjectFileLoader, ProjectFileLoader>();
        return services;
    }
}

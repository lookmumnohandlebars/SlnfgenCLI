using Microsoft.Extensions.DependencyInjection;
using Slnfgen.CLI;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;
using Slnfgen.CLI.Application.Repository;
using Slnfgen.CLI.Application.Requests.GenerateOne;
using Slnfgen.CLI.Application.Requests.SolutionFilter.Generate;
using Slnfgen.CLI.Application.Services.SolutionFilter;

namespace Slnfgen.Application.Module;

internal static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        AddRequestHandlers(services);
        services.AddApplicationRepositoryDependencies();
        return services;
    }

    private static void AddRequestHandlers(IServiceCollection services)
    {
        services.AddScoped<
            IRequestHandler<GenerateSolutionFiltersRequest, GenerateSolutionFiltersResponse>,
            GenerateSolutionFiltersRequestHandler
        >();
        services.AddScoped<
            IRequestHandler<GenerateSolutionFilterRequest, GenerateSolutionFilterResponse>,
            GenerateSolutionFilterRequestHandler
        >();
    }
}

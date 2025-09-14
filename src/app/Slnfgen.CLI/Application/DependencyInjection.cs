using Microsoft.Extensions.DependencyInjection;
using Slnfgen.CLI.Application.Common.Requests;
using Slnfgen.CLI.Application.Repositories;
using Slnfgen.CLI.Application.Requests.GenerateAll;
using Slnfgen.CLI.Application.Requests.GenerateAll.Solutions;
using Slnfgen.CLI.Application.Requests.GenerateTarget;
using GenerateSolutionFiltersRequestHandler = Slnfgen.CLI.Application.Requests.GenerateAll.GenerateSolutionFiltersRequestHandler;

namespace Slnfgen.CLI.Application;

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
            IRequestHandler<GenerateSolutionsRequest, GenerateSolutionsResponse>,
            GenerateSolutionsRequestHandler
        >();
        services.AddScoped<
            IRequestHandler<GenerateTargetSolutionFilterRequest, GenerateTargetSolutionFilterResponse>,
            GenerateTargetSolutionFilterRequestHandler
        >();
    }
}

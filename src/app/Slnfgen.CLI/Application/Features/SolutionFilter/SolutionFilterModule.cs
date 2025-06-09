using Microsoft.Extensions.DependencyInjection;
using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Features.SolutionFilterGeneration;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;
using Slnfgen.CLI.Application.Services.SolutionFilter;

namespace Slnfgen.CLI.Application.Features;

internal static class SolutionFilterDependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationSolutionFilterDependencies(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<GenerateSolutionFiltersRequest>, SolutionFilterFileGenerationHandler>();
        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using Slnfgen.Application.Features.SolutionFilterGeneration;

namespace Slnfgen.Application.Module;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<SolutionFilterGenerator>();
        services.AddScoped<SolutionFilterWriter>();
        return services;
    }
}
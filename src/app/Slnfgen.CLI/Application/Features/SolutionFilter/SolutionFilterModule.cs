using Microsoft.Extensions.DependencyInjection;
using Slnfgen.Application.Features.SolutionFilterGeneration;

namespace Slnfgen.CLI.Application.Features;

public static class SolutionFilterDependencyInjectionExtensions
{
   public static IServiceCollection AddSolutionFilterDependencies(this IServiceCollection services)
   {
      services.AddScoped<SolutionFilterGenerator>();
      services.AddScoped<SolutionFilterWriter>();
      return services;
   }
}
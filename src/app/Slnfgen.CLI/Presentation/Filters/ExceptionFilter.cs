using Cocona.Filters;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Slnfgen.CLI.Presentation.Filters;

/// <summary>
///     Exception filter for handling unhandled exceptions and validation errors in the CLI application.
/// </summary>
internal class ExceptionFilter : CommandFilterAttribute
{
    private readonly ILogger _logger;

    public ExceptionFilter(ILogger logger)
    {
        _logger = logger;
    }

    public override ValueTask<int> OnCommandExecutionAsync(
        CoconaCommandExecutingContext ctx,
        CommandExecutionDelegate next
    )
    {
        try
        {
            return next(ctx);
        }
        catch (RequestValidationException ex)
        {
            LogValidationException(ex);
            return new ValueTask<int>(2); // Return 2 to indicate validation failure
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Uh Oh! An unhandled exception occurred. Please raise in issue in the GitHub source repository."
            );
            return new ValueTask<int>(1);
        }
    }

    private void LogValidationException(ValidationException exception)
    {
        _logger.LogWarning("Validation failed: {Message}", exception.Message);
        foreach (var error in exception.Errors)
            _logger.LogWarning($"{error.PropertyName}: {error.ErrorMessage}");
    }
}

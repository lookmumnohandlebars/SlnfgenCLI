using Cocona.Filters;
using FluentValidation.Results;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Testing;
using Slnfgen.CLI.Presentation.Filters;

namespace Slnfgen.CLI.UnitTests.Presentation.Filters;

public class ExceptionFilterTests : FilterTestsBase
{
    [Fact]
    public async Task OnCommandExecutionAsync_ReturnsResult_WhenNoException()
    {
        var filter = new ExceptionFilter(NullLogger.Instance);

        var result = await filter.OnCommandExecutionAsync(EmptyCommandContext, EmptyCommandDelegate);

        result.Should().Be(3);
    }

    [Fact]
    public async Task OnCommandExecutionAsync_Returns2_AndLogs_WhenValidationException()
    {
        var fakeLogger = new FakeLogger();
        var filter = new ExceptionFilter(fakeLogger);

        var failures = new[] { new ValidationFailure("Prop1", "Error1"), new ValidationFailure("Prop2", "Error2") };
        var validationException = new RequestValidationException(failures);

        CommandExecutionDelegate next = _ => throw validationException;
        var result = await filter.OnCommandExecutionAsync(EmptyCommandContext, next);
        var logsSnapshot = fakeLogger.Collector.GetSnapshot();

        result.Should().Be(2);

        logsSnapshot
            .First()
            .Message.Should()
            .Be(
                "Validation failed: Invalid request. Please see the validation errors for more details.",
                "Initial message should indicate validation failure"
            );

        logsSnapshot
            .Skip(1)
            .Select(log => log.Message)
            .Should()
            .BeEquivalentTo(
                new List<string> { "Prop1: Error1", "Prop2: Error2" },
                "Subsequent messages should contain validation errors"
            );
    }

    [Fact]
    public async Task OnCommandExecutionAsync_Returns1_AndLogs_WhenGenericException()
    {
        var fakeLogger = new FakeLogger();
        var filter = new ExceptionFilter(fakeLogger);

        var error = new InvalidOperationException("Who programmed this nonsense?");
        CommandExecutionDelegate next = _ => throw error;
        var result = await filter.OnCommandExecutionAsync(EmptyCommandContext, next);
        var logsSnapshot = fakeLogger.Collector.GetSnapshot();

        result.Should().Be(1);

        logsSnapshot
            .First()
            .Message.Should()
            .Be(
                "Uh Oh! An unhandled exception occurred. Please raise in issue in the GitHub source repository.",
                "Should log a fatal error message for unhandled exceptions"
            );
        logsSnapshot.First().Exception.Should().BeEquivalentTo(error);
    }
}

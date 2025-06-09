using Microsoft.Extensions.Logging.Abstractions;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;
using Slnfgen.CLI.Commands;
using Slnfgen.CLI.TestImplementations.Application.Common;

namespace Slnfgen.CLI.UnitTests.Presentation.Commands;

public class GenerateSolutionFiltersCommandTests
{
    public GenerateSolutionFiltersCommand _sut;
    private StubRequestHandler<GenerateSolutionFiltersRequest> _stubRequestHandler;

    public GenerateSolutionFiltersCommandTests()
    {
        _stubRequestHandler = new StubRequestHandler<GenerateSolutionFiltersRequest>();
        _sut = new GenerateSolutionFiltersCommand(
            _stubRequestHandler,
            new NullLogger<GenerateSolutionFiltersCommand>()
        );
    }

    [Fact]
    public void Execute_should_send_a_single_request()
    {
        var request = new GenerateSolutionFiltersRequest("path", "dir");
        _sut.Execute("path", "dir");
        _stubRequestHandler.GetRequestCalls(request).Should().Be(1, "Should send a single request");
    }
}

using Microsoft.Extensions.Logging.Abstractions;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;
using Slnfgen.CLI.Application.Requests.SolutionFilter.Generate;
using Slnfgen.CLI.Presentation.Commands;
using Slnfgen.CLI.TestImplementations.Application.Common;

namespace Slnfgen.CLI.UnitTests.Presentation.Commands;

public class GenerateSolutionFiltersCommandTests
{
    private readonly StubRequestHandler<
        GenerateSolutionFiltersRequest,
        GenerateSolutionFiltersResponse
    > _stubRequestHandler;

    private readonly GenerateSolutionFiltersCommand _sut;

    public GenerateSolutionFiltersCommandTests()
    {
        _stubRequestHandler = new StubRequestHandler<GenerateSolutionFiltersRequest, GenerateSolutionFiltersResponse>();
        _sut = new GenerateSolutionFiltersCommand(
            _stubRequestHandler,
            new NullLogger<GenerateSolutionFiltersCommand>()
        );
    }

    [Fact]
    public void Execute_should_send_a_single_request()
    {
        var request = new GenerateSolutionFiltersRequest("path", "dir");
        var response = new GenerateSolutionFiltersResponse(new List<string> { "filter1.slnf", "filter2.slnf" });
        _stubRequestHandler.AddStubResponse(request, response);
        _sut.Execute("path", "dir");
        _stubRequestHandler.GetRequestCalls(request).Should().Be(1, "Should send a single request");
    }
}

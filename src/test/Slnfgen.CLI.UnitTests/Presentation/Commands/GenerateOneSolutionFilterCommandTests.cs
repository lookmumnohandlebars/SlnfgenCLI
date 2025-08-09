using Microsoft.Extensions.Logging.Abstractions;
using Slnfgen.CLI.Application.Requests.GenerateOne;
using Slnfgen.CLI.Application.Requests.SolutionFilter.Generate;
using Slnfgen.CLI.Presentation.Commands;
using Slnfgen.CLI.TestImplementations.Application.Common;

namespace Slnfgen.CLI.UnitTests.Presentation.Commands;

[Collection(nameof(CommandCollection))]
public class GenerateSolutionFilterCommandTests
{
    private readonly StubRequestHandler<
        GenerateSolutionFilterRequest,
        GenerateSolutionFilterResponse
    > _stubRequestHandler;

    private readonly GenerateOneSolutionFilterCommand _sut;

    public GenerateSolutionFilterCommandTests()
    {
        _stubRequestHandler = new StubRequestHandler<GenerateSolutionFilterRequest, GenerateSolutionFilterResponse>();
        _sut = new GenerateOneSolutionFilterCommand(_stubRequestHandler);
    }

    [Fact]
    public void Execute_should_send_a_single_request()
    {
        var request = new GenerateSolutionFilterRequest("path", "target", "dir");
        var response = new GenerateSolutionFilterResponse("filter1.slnf");
        _stubRequestHandler.AddStubResponse(request, response);
        _sut.Execute("path", "target", "dir");
        _stubRequestHandler.GetRequestCalls(request).Should().Be(1, "Should send a single request");
    }
}

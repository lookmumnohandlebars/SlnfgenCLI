using Slnfgen.CLI.Application.Requests.GenerateTarget;
using Slnfgen.CLI.Presentation.Commands;
using Slnfgen.CLI.UnitTests.Application.TestImplementations;

namespace Slnfgen.CLI.UnitTests.Presentation.Commands;

[Collection(nameof(CommandCollection))]
public class GenerateSolutionFilterCommandTests
{
    private readonly StubRequestHandler<
        GenerateTargetSolutionFilterRequest,
        GenerateTargetSolutionFilterResponse
    > _stubRequestHandler;

    private readonly GenerateOneSolutionFilterCommand _sut;

    public GenerateSolutionFilterCommandTests()
    {
        _stubRequestHandler =
            new StubRequestHandler<GenerateTargetSolutionFilterRequest, GenerateTargetSolutionFilterResponse>();
        _sut = new GenerateOneSolutionFilterCommand(_stubRequestHandler);
    }

    [Fact]
    public void Execute_should_send_a_single_request()
    {
        var request = new GenerateTargetSolutionFilterRequest("path", "target", "dir");
        var response = new GenerateTargetSolutionFilterResponse("filter1.slnf");
        _stubRequestHandler.AddStubResponse(request, response);
        _sut.Execute("path", "target", "dir");
        _stubRequestHandler.GetRequestCalls(request).Should().Be(1, "Should send a single request");
    }
}

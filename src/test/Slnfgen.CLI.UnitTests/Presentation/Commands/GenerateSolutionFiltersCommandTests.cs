using Microsoft.Extensions.Logging.Abstractions;
using Slnfgen.CLI.Application.Requests.GenerateAll;
using Slnfgen.CLI.Application.Requests.GenerateAll.Solutions;
using Slnfgen.CLI.Presentation.Commands;
using Slnfgen.CLI.UnitTests.Application.TestImplementations;

namespace Slnfgen.CLI.UnitTests.Presentation.Commands;

[Collection(nameof(CommandCollection))]
public class GenerateSolutionFiltersCommandTests
{
    private readonly StubRequestHandler<
        GenerateSolutionFiltersRequest,
        GenerateSolutionFiltersResponse
    > _stubFiltersRequestHandler;

    private readonly StubRequestHandler<
        GenerateSolutionsRequest,
        GenerateSolutionsResponse
    > _stubSolutionsRequestHandler;

    private readonly GenerateAllSolutionFiltersCommand _sut;

    public GenerateSolutionFiltersCommandTests()
    {
        _stubFiltersRequestHandler =
            new StubRequestHandler<GenerateSolutionFiltersRequest, GenerateSolutionFiltersResponse>();
        _stubSolutionsRequestHandler = new StubRequestHandler<GenerateSolutionsRequest, GenerateSolutionsResponse>();
        _sut = new GenerateAllSolutionFiltersCommand(
            _stubFiltersRequestHandler,
            _stubSolutionsRequestHandler,
            new NullLogger<GenerateAllSolutionFiltersCommand>()
        );
    }

    [Fact]
    public void Execute_should_send_expected_requests_to_handlers()
    {
        var filtersRequest = new GenerateSolutionFiltersRequest("path", "dir");
        var filtersresponse = new GenerateSolutionFiltersResponse(new List<string> { "filter1.slnf", "filter2.slnf" });
        _stubFiltersRequestHandler.AddStubResponse(filtersRequest, filtersresponse);
        var solutionsRequest = new GenerateSolutionsRequest("path", "dir");
        var solutionsResponse = new GenerateSolutionsResponse(new List<string> { "filter1.slnf", "filter2.slnf" });
        _stubSolutionsRequestHandler.AddStubResponse(solutionsRequest, solutionsResponse);

        _sut.Execute("path", "dir");
        _stubFiltersRequestHandler
            .GetRequestCalls(filtersRequest)
            .Should()
            .Be(1, "Should send a request to generate solution filters");

        _stubSolutionsRequestHandler
            .GetRequestCalls(solutionsRequest)
            .Should()
            .Be(1, "Should send a request to generate solutions");
    }
}

using Microsoft.Extensions.Logging.Testing;
using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Features.SolutionFilterGeneration;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;
using Slnfgen.CLI.Application.Repository.Solution.Project;
using Slnfgen.CLI.Application.Services.SolutionFilter;
using Slnfgen.CLI.Domain.Solution.File.Loader;
using Slnfgen.CLI.TestImplementations.Application.Repository;

namespace Slnfgen.CLI.UnitTests.Application.Requests.SolutionFilter.Generate;

public class GenerateSolutionFiltersRequestHandlerTests
{
    private GenerateSolutionFiltersRequestHandler _sut;
    private FakeSolutionFilterWriter _fakeSolutionFilterWriter = new();

    public GenerateSolutionFiltersRequestHandlerTests()
    {
        _sut = new GenerateSolutionFiltersRequestHandler(
            new SolutionFilterGenerator(new ProjectFileLoader()),
            _fakeSolutionFilterWriter,
            new SolutionFiltersManifestFileLoader(),
            new SolutionFileLoader(),
            new FakeLogger<GenerateSolutionFiltersRequestHandler>()
        );
    }

    [Fact]
    public void Handle_ShouldGenerateSolutionFilters()
    {
        // Arrange
        var request = new GenerateSolutionFiltersRequest(Path.Combine("TestSolution", "monorepo.yml"), ".");

        // Act
        var response = _sut.Handle(request);

        // Assert
        response.Should().NotBeNull();
        response.GeneratedFilters.Should().HaveCount(2);

        var filterOne = _fakeSolutionFilterWriter.Store[Path.Combine(".", "FilterOne")];
        filterOne.Should().NotBeNull();
        filterOne.Solution.Path.Should().Be(@"TestSolution\\TestSolution.slnx");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"ProjA\\ProjA.csproj",
                @"ProjC\\ProjC.csproj",
                @"ProjE\\ProjE.csproj",
                @"ProjF\\ProjF.csproj",
                @"ProjG\\ProjG.csproj"
            );

        var filterTwo = _fakeSolutionFilterWriter.Store[Path.Combine(".", "FilterTwo")];
        filterOne.Should().NotBeNull();
        filterOne.Solution.Path.Should().Be(@"TestSolution\\TestSolution.slnx");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"ProjA\\ProjA.csproj",
                @"ProjC\\ProjC.csproj",
                @"ProjE\\ProjE.csproj",
                @"ProjF\\ProjF.csproj",
                @"ProjG\\ProjG.csproj"
            );
    }

    [Fact]
    public void Handle_ShouldGenerateSolutionFilters_ForLegacySolutionFormat()
    {
        // Arrange
        var request = new GenerateSolutionFiltersRequest(Path.Combine("TestSolution", "monorepoLegacy.yml"), ".");

        // Act
        var response = _sut.Handle(request);

        // Assert
        response.Should().NotBeNull();
        response.GeneratedFilters.Should().HaveCount(2);
        var filterOne = _fakeSolutionFilterWriter.Store[Path.Combine(".", "FilterOne")];
        filterOne.Should().NotBeNull();
        filterOne.Solution.Path.Should().Be(@"TestSolution\\TestSolutionLegacy.sln");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"ProjA\\ProjA.csproj",
                @"ProjC\\ProjC.csproj",
                @"ProjE\\ProjE.csproj",
                @"ProjF\\ProjF.csproj"
            );
    }
}

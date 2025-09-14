using System.Text.Json;
using Microsoft.Extensions.Logging.Testing;
using Slnfgen.CLI.Application.Repositories.Manifest.SolutionFiltersManifest;
using Slnfgen.CLI.Application.Repositories.Solution.File;
using Slnfgen.CLI.Application.Repositories.Solution.Filter;
using Slnfgen.CLI.Application.Repositories.Solution.Project;
using Slnfgen.CLI.Application.Requests.GenerateAll;
using Slnfgen.CLI.Domain.Solution.File.Services;
using Slnfgen.CLI.Domain.Solution.Filter.Services;
using Slnfgen.CLI.UnitTests.Application.TestImplementations;

namespace Slnfgen.CLI.UnitTests.Application.Requests.Generate;

public class GenerateSolutionFiltersRequestHandlerTests
{
    private GenerateSolutionFiltersRequestHandler _sut;
    private FakeSolutionFilterWriter _fakeSolutionFilterWriter = new();

    public GenerateSolutionFiltersRequestHandlerTests()
    {
        _sut = new GenerateSolutionFiltersRequestHandler(
            new SolutionFilterGenerator(new ProjectFileLoader(), new ProjectSuffixFinder()),
            _fakeSolutionFilterWriter,
            new XmlSolutionFileWriter(),
            new SolutionFiltersManifestFileLoader(),
            new SolutionFileLoader(),
            new FakeLogger<GenerateSolutionFiltersRequestHandler>(),
            new SolutionGenerator(new ProjectFileLoader(), new ProjectSuffixFinder())
        );
    }

    [Fact]
    public void Handle_ShouldGenerateSolutionFilters()
    {
        // Arrange
        var request = new GenerateSolutionFiltersRequest(
            Path.Combine("TestSolutions", "BasicSolution", "monorepo.yml"),
            "."
        );

        // Act
        var response = _sut.Handle(request);

        // Assert
        response.Should().NotBeNull();
        response.GeneratedFilters.Should().HaveCount(2);

        var filterOne = _fakeSolutionFilterWriter.Store[Path.Combine(".", "FilterOne")];
        filterOne.Should().NotBeNull();
        filterOne.Solution.Path.Should().Be(@"TestSolutions\BasicSolution\TestSolution.slnx");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"ProjA\ProjA.csproj",
                @"ProjC\ProjC.csproj",
                @"ProjE\ProjE.csproj",
                @"ProjF\ProjF.csproj",
                @"ProjG\ProjG.csproj"
            );

        var filterTwo = _fakeSolutionFilterWriter.Store[Path.Combine(".", "FilterTwo")];
        filterTwo.Should().NotBeNull();
        filterTwo.Solution.Path.Should().Be(@"TestSolutions\BasicSolution\TestSolution.slnx");
        filterTwo
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"Projb\Nested\Projb.csproj",
                @"ProjD\ProjD.csproj",
                @"ProjF\ProjF.csproj",
                @"ProjG\ProjG.csproj"
            );
    }

    [Fact]
    public async Task Handle_ShouldGenerateConsistentJsonSolutionFilter()
    {
        // Arrange
        var request = new GenerateSolutionFiltersRequest(
            Path.Combine("TestSolutions", "BasicSolution", "monorepo.yml"),
            "."
        );

        // Act
        var response = _sut.Handle(request);

        // Assert
        // This is to ensure that the JSON serialization is consistent and especially
        // that ordering and formatting does not change unexpectedly.
        var filterOne = _fakeSolutionFilterWriter.Store[Path.Combine(".", "FilterOne")];
        await Verify(JsonSerializer.Serialize(filterOne, SolutionFilterFileWriter.JsonOptions()), "slnf");
    }

    [Fact]
    public void Handle_ShouldGenerateSolutionFilters_ForLegacySolutionFormat()
    {
        // Arrange
        var request = new GenerateSolutionFiltersRequest(
            Path.Combine("TestSolutions", "BasicSolution", "monorepoLegacy.yml"),
            "."
        );

        // Act
        var response = _sut.Handle(request);

        // Assert
        response.Should().NotBeNull();
        response.GeneratedFilters.Should().HaveCount(2);
        var filterOne = _fakeSolutionFilterWriter.Store[Path.Combine(".", "FilterOne")];
        filterOne.Should().NotBeNull();
        filterOne.Solution.Path.Should().Be(@"TestSolutions\BasicSolution\TestSolutionLegacy.sln");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(@"ProjA\ProjA.csproj", @"ProjC\ProjC.csproj", @"ProjE\ProjE.csproj", @"ProjF\ProjF.csproj");
    }
}

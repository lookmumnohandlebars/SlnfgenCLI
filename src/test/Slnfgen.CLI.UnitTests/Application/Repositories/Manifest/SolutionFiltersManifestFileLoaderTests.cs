using Slnfgen.CLI.Application.Common.Files.Exceptions;
using Slnfgen.CLI.Application.Common.Requests.Validation;
using Slnfgen.CLI.Application.Repositories.Manifest.SolutionFiltersManifest;
using Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Models;

namespace Slnfgen.CLI.UnitTests.Application.Repositories.Manifest;

public class SolutionFiltersManifestFileLoaderTests
{
    private readonly SolutionFiltersManifestFileLoader _sut = new();

    [Fact]
    public void Load_should_parse_test_filters_file_in_yaml_format_with_slnx()
    {
        var expected = new SolutionFiltersManifest(
            "TestSolution.slnx",
            [
                new SolutionFiltersManifestFilterDefinition(
                    "FilterOne",
                    [@"ProjA\ProjA.csproj", @"ProjG\ProjG.csproj"]
                ),
                new SolutionFiltersManifestFilterDefinition(
                    "FilterTwo",
                    [@"Projb\Nested\Projb.csproj", @"ProjD\ProjD.csproj"]
                ),
            ],
            new List<ManifestSolutionDefinition>() { new("SolutionOne", [@"ProjA\ProjA.csproj"]) }
        );

        var filterFilePath = Path.Combine("TestSolutions", "BasicSolution", "monorepo.yml");
        var filtersDefinition = _sut.Load(filterFilePath);

        filtersDefinition.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Load_should_parse_test_filters_file_with_testpatterns_in_yaml_format_with_slnx()
    {
        var expected = new SolutionFiltersManifest(
            "SolutionWithTests.slnx",
            [
                new SolutionFiltersManifestFilterDefinition(
                    "FilterOne",
                    [@"ProjA\ProjA.csproj"],
                    [@"Unit.Tests", "Integration.Tests"]
                ),
                new SolutionFiltersManifestFilterDefinition("FilterTwo", [@"ProjB\ProjB.csproj"], [@"Contract.Tests"]),
            ],
            [],
            autoIncludeSuffixPatterns: [@"Unit.Tests"]
        );

        var filterFilePath = Path.Combine("TestSolutions", "SolutionWithTests", "monorepo.yml");
        var filtersDefinition = _sut.Load(filterFilePath);

        filtersDefinition.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Load_should_parse_test_filters_file_in_json_format_with_slnx()
    {
        var expected = new SolutionFiltersManifest(
            "TestSolution.slnx",
            [
                new SolutionFiltersManifestFilterDefinition(
                    "FilterOne",
                    [@"ProjA/ProjA.csproj", @"ProjG/ProjG.csproj"]
                ),
                new SolutionFiltersManifestFilterDefinition(
                    "FilterTwo",
                    [@"Projb/Projb.csproj", @"ProjD/ProjD.csproj"]
                ),
            ],
            []
        );

        var filterFilePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "TestSolutions",
            "BasicSolution",
            "monorepo.json"
        );
        var filtersDefinition = _sut.Load(filterFilePath);

        filtersDefinition.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Load_should_parse_test_filters_file_in_yaml_format_with_legacy_sln()
    {
        var expected = new SolutionFiltersManifest(
            "TestSolutionLegacy.sln",
            [
                new SolutionFiltersManifestFilterDefinition(
                    "FilterOne",
                    [@"ProjA/ProjA.csproj", @"ProjG/ProjG.csproj"]
                ),
                new SolutionFiltersManifestFilterDefinition(
                    "FilterTwo",
                    [@"Projb/Nested/Projb.csproj", @"ProjD/ProjD.csproj"]
                ),
            ],
            []
        );

        var filterFilePath = Path.Combine("TestSolutions", "BasicSolution", "monorepoLegacy.yml");
        var filtersDefinition = _sut.Load(filterFilePath);

        filtersDefinition.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void FromFile_should_throw_file_not_found_exception_if_valid_extension_with_no_file()
    {
        var filterFilePath = "invalidFile.yml";
        var act = () => _sut.Load(filterFilePath);
        act.Should().Throw<BadRequestException>();
    }

    [Fact]
    public void FromFile_should_throw_file_not_found_exception_if_invalid_extension()
    {
        var filterFilePath = "monorepo.yiml";
        var act = () => _sut.Load(filterFilePath);
        act.Should().Throw<BadRequestException>();
    }

    [Fact]
    public void FromFile_should_throw_file_not_found_exception_if_invalid()
    {
        var filterFilePath = "invalidMonorepo.yml";
        var act = () => _sut.Load(Path.Combine("TestSolutions", "BasicSolution", filterFilePath));
        act.Should().Throw<InvalidFileException>();
    }
}

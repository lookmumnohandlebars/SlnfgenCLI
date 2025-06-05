using Slnfgen.Application.Domain.Filters;

namespace Slnfgen.CLI.UnitTests.Domain.Filters;

public class SolutionFiltersConfigurationTests
{
    [Fact]
    public void FromFile_should_parse_test_filters_file_in_yaml_format_with_slnx()
    {
        var expected = new SolutionFiltersConfiguration(
            "TestSolution.slnx", 
            [
                new("FilterOne", ["ProjA/ProjA.csproj", "ProjG/ProjG.csproj"]),
                new("FilterTwo", ["Projb/Projb.csproj", "ProjD/ProjD.csproj"]),
            ]
        );
        
        var filterFilePath = Path.Combine("TestSolution", "monorepo.yml");
        var filtersDefinition = SolutionFiltersConfiguration.FromFile(filterFilePath);

        filtersDefinition.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void FromFile_should_parse_test_filters_file_in_json_format_with_slnx()
    {
        var expected = new SolutionFiltersConfiguration(
            "TestSolution.slnx", 
            [
                new("FilterOne", ["ProjA/ProjA.csproj", "ProjG/ProjG.csproj"]),
                new("FilterTwo", ["Projb/Projb.csproj", "ProjD/ProjD.csproj"]),
            ]
        );
        
        var filterFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestSolution", "monorepo.json");
        var filtersDefinition = SolutionFiltersConfiguration.FromFile(filterFilePath);

        filtersDefinition.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void FromFile_should_parse_test_filters_file_in_yaml_format_with_legacy_sln()
    {
        var expected = new SolutionFiltersConfiguration(
            "TestSolutionLegacy.sln", 
            [
                new("FilterOne", ["ProjA/ProjA.csproj", "ProjG/ProjG.csproj"]),
                new("FilterTwo", ["Projb/Projb.csproj", "ProjD/ProjD.csproj"]),
            ]
        );
        
        var filterFilePath = Path.Combine("TestSolution", "monorepoLegacy.yml");
        var filtersDefinition = SolutionFiltersConfiguration.FromFile(filterFilePath);

        filtersDefinition.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void FromFile_should_throw_file_not_found_exception_if_valid_extension_with_no_file()
    {
        var filterFilePath = "invalidFile.yml";
        File.Delete(filterFilePath);
        
        var act = () => SolutionFiltersConfiguration.FromFile(filterFilePath);
        act.Should().Throw<FileNotFoundException>();
    }
    
    [Fact]
    public void FromFile_should_throw_file_not_found_exception_if_invalid_extension()
    {
        var filterFilePath = "monorepo.yiml";
        File.Delete(filterFilePath);
        
        var act = () => SolutionFiltersConfiguration.FromFile(filterFilePath);
        act.Should().Throw<NotSupportedException>();
    }
    
}
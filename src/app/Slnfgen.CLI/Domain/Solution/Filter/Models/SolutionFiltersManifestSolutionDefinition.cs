using System.ComponentModel.DataAnnotations;
using Throw;

namespace Slnfgen.CLI.Domain.Solution.Filter.Models;

/// <summary>
///     Solution definition for the solution filter file.
/// </summary>
public class SolutionFiltersSolutionDefinition
{
    /// <inheritdoc cref="SolutionFiltersSolutionDefinition"/>
    /// <param name="path">Path to the solution file from this filter</param>
    /// <param name="projects">Projects included in the solution filter</param>
    public SolutionFiltersSolutionDefinition(string path, string[] projects)
    {
        Path = FormatPathForJson(path.Throw().IfEmpty());
        Projects = projects.Throw().IfContains(string.Empty).Value.Select(FormatPathForJson).ToArray();
    }

    /// <summary>
    ///     Represents the path to the solution file, relative to the solution filters manifest file.
    /// </summary>
    [Required]
    public string Path { get; }

    /// <summary>
    ///     Projects that are part of the solution filter
    /// </summary>
    [Required]
    public string[] Projects { get; }

    private string FormatPathForJson(string path)
    {
        var outputPath = string.Empty;
        for (var i = 0; i < path.Length; i++)
        {
            if (path[i] == '/')
            {
                // swap for backslash
                outputPath += "\\\\";
                continue;
            }

            if (path[i] == '\\') // escape backslash if not already escaped
            {
                var nextChar = i + 1 <= path.Length ? path[i + 1] : '\0';
                var prevChar = i - 1 >= 0 ? path[i - 1] : '\0';
                if (nextChar == '\\' || prevChar == '\\')
                {
                    // already escaped
                    outputPath += '\\';
                    continue;
                }

                outputPath += "\\\\";
                continue;
            }

            outputPath += path[i];
        }

        return outputPath;
    }
}

using Microsoft.Build.Construction;
using Slnfgen.Application.Domain.Project;

namespace Slnfgen.Application.Features.Solution;

public class RootSolutionFile
{
    private string _path;
    private SolutionFile _solutionFile;
    
    public RootSolutionFile(SolutionFile solutionFile, string path)
    {
        _solutionFile = solutionFile;
        _path = path;
    }

    public string Path => _path;
    public IEnumerable<SolutionProject> ProjectsInSolution => 
        _solutionFile
            .ProjectsInOrder
            .Select(proj => 
                new SolutionProject(
                    _solutionFile,
                    proj
                )
            );

    public static RootSolutionFile FromSolutionFilePath(string solutionFilePath)
    {
        if (solutionFilePath.EndsWith(".sln", StringComparison.CurrentCultureIgnoreCase) || solutionFilePath.EndsWith(".slnx", StringComparison.CurrentCultureIgnoreCase))
        {
            var normalizedPath = System.IO.Path.GetFullPath(solutionFilePath);
            return new(SolutionFile.Parse(normalizedPath), normalizedPath);
        }
        throw new ArgumentException($"Invalid solution file path: {solutionFilePath}. Must end with .sln or slnx");
    }
}
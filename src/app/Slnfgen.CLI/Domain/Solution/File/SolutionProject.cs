using Microsoft.Build.Construction;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;

namespace Slnfgen.Application.Domain.Project;

public class SolutionProject : IEquatable<SolutionProject>
{
    private readonly SolutionFile _parentSolutionFile;
    private readonly ProjectInSolution _projectInSolution;
    
    public SolutionProject(
        SolutionFile parentsolutionFile, 
        ProjectInSolution projectInSolution,
        Microsoft.Build.Evaluation.Project project)
    {
        _projectInSolution = projectInSolution;
        _parentSolutionFile = parentsolutionFile;
    }

    public SolutionProject(
        SolutionFile parentsolutionFile,
        ProjectInSolution projectInSolution
    ) : this(
        parentsolutionFile, 
        projectInSolution, 
        Microsoft.Build.Evaluation.Project.FromFile(projectInSolution.AbsolutePath, new ProjectOptions()
        {
            Interactive = false,
            LoadSettings = ProjectLoadSettings.IgnoreMissingImports, })
    )
    {
    }

    public string Name => _projectInSolution.ProjectName;
    public string Path => _projectInSolution.RelativePath;
    
    public bool Equals(SolutionProject? other)
    {
        if (other is null) return false;
        return _projectInSolution.AbsolutePath == other._projectInSolution.AbsolutePath &&
               _projectInSolution.ProjectGuid == other._projectInSolution.ProjectGuid;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((SolutionProject)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_projectInSolution.AbsolutePath);
    }
}
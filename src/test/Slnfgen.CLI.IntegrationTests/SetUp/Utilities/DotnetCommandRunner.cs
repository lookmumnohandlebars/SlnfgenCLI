using System.Diagnostics;

namespace Slnfgen.CLI.IntegrationTests;

public class DotnetCommandRunner
{
    private readonly string _workingDirectory;

    public DotnetCommandRunner(string workingDirectory)
    {
        _workingDirectory = workingDirectory;
    }

    public void CreateNewSolution(string solutionName)
    {
        RunCommand("new", "sln", "-n", solutionName);
    }

    public void CreateProjectAndAddToSolution(string projectName)
    {
        var projectPath = Path.Combine(_workingDirectory, projectName);

        // Create the project
        RunCommand("new", "classlib", "-o", projectName);

        if (!Directory.Exists(projectPath))
        {
            throw new DirectoryNotFoundException($"Project directory was not created at: {projectPath}");
        }

        // Add it to the solution
        RunCommand("sln", "add", projectName);
    }

    public void AddProjectReference(string projectToAdd, string projectToAddTo)
    {
        RunCommand("add", projectToAdd, "reference", projectToAddTo);
    }

    private void RunCommand(params string[] args)
    {
        var commandLine = $"dotnet {string.Join(" ", args)}";
        Console.WriteLine($"Running command: {commandLine} in {_workingDirectory}");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = string.Join(" ", args),
                WorkingDirectory = _workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };

        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        Console.WriteLine($"Output: {output}");

        if (process.ExitCode != 0)
        {
            throw new Exception(
                $"dotnet command '{commandLine}' failed with exit code {process.ExitCode}.\nError: {error}\nOutput: {output}"
            );
        }
    }
}

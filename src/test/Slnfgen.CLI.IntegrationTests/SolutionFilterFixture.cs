using System.Diagnostics;

namespace Slnfgen.CLI.IntegrationTests;

public class SolutionFilterFixture : IDisposable
{
    public string DirectoryOfWork { get; }
    public SolutionFilterFixture()
    {
        var tmpDir = Path.Combine(Path.GetTempPath(), "Slnfgen.CLI.IntegrationTests", Guid.NewGuid().ToString());
        DirectoryOfWork = Directory.CreateDirectory(tmpDir).FullName;
        
        // Create a new solution
        RunDotnetCommand("new", "sln", "-n", "TestSolution");
        
        // Create 10 projects and add them to the solution
        for (int i = 1; i <= 10; i++)
        {
            var projectName = $"Project{i}";
            var projectPath = Path.Combine(DirectoryOfWork, projectName);
            Directory.CreateDirectory(projectPath);
            
            // Create a console project
            RunDotnetCommand("new", "console", "-n", projectName, "-o", projectPath);
            
            // Add the project to the solution
            RunDotnetCommand("sln", "add", projectPath);
        }
    }
    
    private void RunDotnetCommand(params string[] args)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = string.Join(" ", args),
                WorkingDirectory = DirectoryOfWork,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        
        process.Start();
        process.WaitForExit();
        
        if (process.ExitCode != 0)
        {
            throw new Exception($"dotnet command failed with exit code {process.ExitCode}. Error: {process.StandardError.ReadToEnd()}");
        }
    }
    
    public void Dispose()
    {
        
    }
}

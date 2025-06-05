using System.Diagnostics;

namespace Slnfgen.CLI.IntegrationTests.Utlities.CommandLine;

public class DotnetCliCommander
{
    public DotnetCliCommander()
    {
        
    }
    
    private void RunDotnetCommand(string directory, params string[] args)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = string.Join(" ", args),
                WorkingDirectory = directory,
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
}
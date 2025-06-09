using System.Diagnostics;

namespace Slnfgen.CLI.IntegrationTests;

public class CliRunner
{
    private readonly string _workingDirectory;
    private readonly string _cliPath;

    public CliRunner(string workingDirectory, string cliPath)
    {
        _workingDirectory = workingDirectory;
        _cliPath = cliPath;
    }

    public CliRunner()
        : this(
            Directory.GetCurrentDirectory(),
            Path.Combine("..", "..", "..", "..", "..", "app", "Slnfgen.CLI", "Slnfgen.CLI.csproj")
        ) { }

    public string Run(params string[] args)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project {_cliPath} -- {string.Join(" ", args)}",
                WorkingDirectory = _workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };

        process.Start();
        process.WaitForExit(10000);
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        if (!string.IsNullOrEmpty(error))
        {
            throw new Exception(error);
        }

        return output;
    }
}

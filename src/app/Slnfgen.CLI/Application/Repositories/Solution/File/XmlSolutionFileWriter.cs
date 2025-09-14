using System.Xml;
using Slnfgen.CLI.Domain.Solution.File.Models;
using Slnfgen.CLI.Domain.Solution.File.Repository;

namespace Slnfgen.CLI.Application.Repositories.Solution.File;

internal class XmlSolutionFileWriter : IXmlSolutionFileWriter
{
    public string Write(XmlSolutionFile xmlSolutionFile, string outDirectory)
    {
        var xdoc = xmlSolutionFile.ToXmlDocment();
        var filePath = Path.Combine(outDirectory, $"{xmlSolutionFile.Name}.slnx");
        if (!Directory.Exists(outDirectory))
            Directory.CreateDirectory(outDirectory);
        using var writer = XmlWriter.Create(
            filePath,
            new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true }
        );
        xdoc.Save(writer);
        return filePath;
    }

    public IEnumerable<string> WriteMany(IEnumerable<XmlSolutionFile> solutionFiles, string outDirectory) =>
        solutionFiles.Select(solutionFile => Write(solutionFile, outDirectory));
}

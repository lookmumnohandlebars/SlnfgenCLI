using Slnfgen.CLI.Domain.Solution.File.Models;

namespace Slnfgen.CLI.Domain.Solution.File.Repository;

/// <summary>
///
/// </summary>
public interface IXmlSolutionFileWriter
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="solutionFile"></param>
    /// <param name="outDirectory"></param>
    /// <returns></returns>
    public string Write(XmlSolutionFile solutionFile, string outDirectory);

    /// <summary>
    ///
    /// </summary>
    /// <param name="solutionFile"></param>
    /// <param name="outDirectory"></param>
    /// <returns></returns>
    public IEnumerable<string> WriteMany(IEnumerable<XmlSolutionFile> solutionFile, string outDirectory);
}

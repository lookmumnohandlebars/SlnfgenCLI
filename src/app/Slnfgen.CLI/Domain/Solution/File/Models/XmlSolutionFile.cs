using System.Xml;
using System.Xml.Linq;

namespace Slnfgen.CLI.Domain.Solution.File.Models;

/// <summary>
///
/// </summary>
public class XmlSolutionFile
{
    /// <inheritdoc cref="XmlSolutionFile"/>
    /// <param name="name"></param>
    /// <param name="projectPaths"></param>
    public XmlSolutionFile(string name, IEnumerable<string> projectPaths)
    {
        Name = name;
        ProjectPaths = projectPaths;
    }

    /// <summary>
    ///
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<string> ProjectPaths { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public XDocument ToXmlDocment()
    {
        var doc = new XDocument(
            new XElement(
                "Solution",
                ProjectPaths.Select(projectPath => new XElement("Project", new XAttribute("Path", projectPath)))
            )
        );
        return doc;
    }
}

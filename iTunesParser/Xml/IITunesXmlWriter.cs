using System.Xml.Linq;

namespace iTunesSmartParser.Xml;

public interface IITunesXmlWriter
{
    /// <summary>
    /// Replace the library with the given document
    /// </summary>
    Task SaveXDocument(XDocument doc, CancellationToken token);
}

using System.Xml.Linq;

namespace iTunesSmartParser.Xml;

public interface IITunesXmlReader
{
    /// <summary>
    /// Load the library document. Use in conjunction with <see cref="IITunesXmlWriter" />.
    /// </summary>
    Task<XDocument> GetXDocument(CancellationToken token);
}

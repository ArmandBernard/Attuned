using System.Xml.Linq;

namespace iTunesSmartParser.Xml;

public interface IXmlSource
{
    Task<XDocument> GetXDocument(CancellationToken token);
}
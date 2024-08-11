using System.Xml.Linq;

namespace iTunesSmartParser.Xml;

public class XmlSource(string path) : IXmlSource
{
    public async Task<XDocument> GetXDocument(CancellationToken token)
    {
        using var stream = File.OpenText(path);

        return await XDocument.LoadAsync(stream, LoadOptions.None, token);
    }
}
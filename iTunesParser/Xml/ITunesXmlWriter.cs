using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace iTunesSmartParser.Xml;

public partial class ITunesXmlWriter(string path) : IITunesXmlWriter
{
    /// <summary>
    /// Saves safely.
    /// The document is fully written to a temporary file first and then moved over the library, so a
    /// failure part way through never leaves a truncated library. The first time the library is overwritten, the
    /// original is kept next to it with a ".bak" extension.
    /// </summary>
    public async Task SaveXDocument(XDocument doc, CancellationToken token)
    {
        var tempPath = path + ".tmp";
        var backupPath = path + ".bak";

        try
        {
            // UTF-8 without BOM, as iTunes writes it
            await File.WriteAllTextAsync(tempPath, Serialise(doc, DetectLineBreak()), new UTF8Encoding(false), token);

            if (!File.Exists(backupPath))
            {
                File.Copy(path, backupPath);
            }

            File.Move(tempPath, path, overwrite: true);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    /// <summary>
    /// Writes the document the way iTunes does. Saving an unchanged document read by <see cref="ITunesXmlReader"/>
    /// should give back the original file byte for byte.
    /// </summary>
    private static string Serialise(XDocument doc, string lineBreak)
    {
        var root = doc.Root ?? throw new InvalidOperationException("The document has no root element.");

        var builder = new StringBuilder();

        if (doc.Declaration != null)
        {
            builder.Append(doc.Declaration).Append(lineBreak);
        }

        if (doc.DocumentType is { } docType)
        {
            WriteDocType(builder, docType, lineBreak);
        }

        // iTunes escapes with numeric references where .NET uses named ones. They mean the same thing in XML.
        var body = WriteElement(root).Replace("&amp;", "&#38;").Replace("&lt;", "&#60;").Replace("&gt;", "&#62;");

        builder.Append(SelfClosingBooleanRegex().Replace(body, "<$1/>")).Append(lineBreak);

        return builder.ToString();
    }

    /// <summary>
    /// XDocumentType.ToString() adds an empty "[]" internal subset, so write the declaration out by hand
    /// </summary>
    private static void WriteDocType(StringBuilder builder, XDocumentType docType, string lineBreak)
    {
        builder.Append("<!DOCTYPE ").Append(docType.Name);

        if (docType.PublicId != null)
        {
            builder.Append($" PUBLIC \"{docType.PublicId}\" \"{docType.SystemId}\"");
        }
        else if (docType.SystemId != null)
        {
            builder.Append($" SYSTEM \"{docType.SystemId}\"");
        }

        if (!string.IsNullOrEmpty(docType.InternalSubset))
        {
            builder.Append($" [{docType.InternalSubset}]");
        }

        builder.Append('>').Append(lineBreak);
    }

    /// <summary>
    /// The line break the library file currently uses. iTunes on Windows uses "\r\n".
    /// </summary>
    private string DetectLineBreak()
    {
        using var reader = File.OpenText(path);

        int c;
        while ((c = reader.Read()) != -1)
        {
            switch (c)
            {
                case '\r':
                    return "\r\n";
                case '\n':
                    return "\n";
            }
        }

        return Environment.NewLine;
    }

    private static string WriteElement(XElement element)
    {
        var settings = new XmlWriterSettings
        {
            OmitXmlDeclaration = true,
            Indent = false,
            // write line breaks exactly as they are in the document, rather than converting them to this platform's
            NewLineHandling = NewLineHandling.None,
        };

        var output = new StringWriter();

        using (var writer = XmlWriter.Create(output, settings))
        {
            element.WriteTo(writer);
        }

        return output.ToString();
    }

    // .NET writes an empty element as "<true />", iTunes writes "<true/>"
    [GeneratedRegex("<(true|false) />")]
    private static partial Regex SelfClosingBooleanRegex();
}

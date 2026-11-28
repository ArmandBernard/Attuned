using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace iTunesSmartParser.Xml;

public partial class ITunesXmlReader(string path) : IITunesXmlReader
{
    /// <summary>
    /// Reads the XDocument in a way that will preserve formatting when written back
    /// </summary>
    public async Task<XDocument> GetXDocument(CancellationToken token)
    {
        var text = await ReadFile(token);

        // Only replace inside the root
        var rootStart = GetRootStart(text);
        var rootEnd = text.TrimEnd().Length;
        text = text[..rootStart] + ReplaceCarriageReturns(text[rootStart..rootEnd]) + text[rootEnd..];

        return XDocument.Parse(text, LoadOptions.PreserveWhitespace);
    }

    /// <summary>
    /// Try reading with exponential backoff. Mitigates issues with writes.
    /// </summary>
    private async Task<string> ReadFile(CancellationToken token)
    {
        const int maxAttempts = 5;

        for (var attempt = 1; ; attempt++)
        {
            try
            {
                await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read,
                    FileShare.ReadWrite | FileShare.Delete, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan);
                using var reader = new StreamReader(stream);

                return await reader.ReadToEndAsync(token);
            }
            catch (IOException) when (attempt < maxAttempts && File.Exists(path))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(20 * attempt), token);
            }
        }
    }

    private static int GetRootStart(string text)
    {
        var rootMatch = RootElementStartRegex().Match(text);

        return rootMatch.Success
            ? rootMatch.Index
            : throw new Exception("XML file is incorrectly formatted. Root not found.");
    }

    /// <summary>
    /// An XML reader turns every "\r\n" into "\n", as the XML specification says it should. iTunes' files have both
    /// kinds inside values, so to get the file back exactly as it was, each "\r" is read as a character reference,
    /// which the reader leaves alone. <see cref="ITunesXmlWriter"/> writes them back out as they were.
    /// </summary>
    private static string ReplaceCarriageReturns(string text) => text.Replace("\r", "&#13;");

    // the first tag that is not the XML declaration, a DOCTYPE or a comment
    [GeneratedRegex("<[A-Za-z_]")]
    private static partial Regex RootElementStartRegex();
}

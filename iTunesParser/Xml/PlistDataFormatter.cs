using System.Text.RegularExpressions;

namespace iTunesSmartParser.Xml;

/// <summary>
/// Writes the text of a plist &lt;data&gt; element, laid out as iTunes lays it out: base64 split into lines of the same
/// length, indented to match the rest of the file.
/// </summary>
internal static partial class PlistDataFormatter
{
    // iTunes' lines are this long. It is only used if the data being replaced was too short to be on more than a line.
    private const int DEFAULT_LINE_LENGTH = 72;

    /// <summary>
    /// Format bytes as base64 in the same layout as the text of an existing element
    /// </summary>
    /// <param name="bytes">The new content</param>
    /// <param name="existingText">The text that is being replaced, which sets the line length, indentation and line breaks</param>
    public static string Format(byte[] bytes, string existingText)
    {
        var base64 = Convert.ToBase64String(bytes);

        var content = existingText.Trim();

        if (content.Length == 0)
        {
            return base64;
        }

        // whitespace before the first line, and after the last
        var before = existingText[..(existingText.Length - existingText.TrimStart().Length)];
        var after = existingText[existingText.TrimEnd().Length..];

        var lines = LineBreakRegex().Split(content);

        // whitespace between lines is the line break and the indentation
        var separator = lines.Length > 1 ? LineBreakRegex().Match(content).Value : before;
        var lineLength = lines.Length > 1 ? lines.Max(x => x.Length) : DEFAULT_LINE_LENGTH;

        var newLines = Enumerable.Range(0, (base64.Length + lineLength - 1) / lineLength)
            .Select(i => base64.Substring(i * lineLength, Math.Min(lineLength, base64.Length - i * lineLength)));

        return before + string.Join(separator, newLines) + after;
    }

    [GeneratedRegex(@"\s*\r?\n\s*")]
    private static partial Regex LineBreakRegex();
}

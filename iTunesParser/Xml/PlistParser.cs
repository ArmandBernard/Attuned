using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Schema;

namespace iTunesSmartParser.Xml;

/// <summary>
/// Used to parse Apple's proprietary PList format.
/// </summary>
public static class PListParser
{
    public static Dictionary<string, dynamic> ParseDocument(XDocument doc)
    {
        // first dictionary
        var dict = GetTopLevelDictionaryElement(doc);

        if (dict == null)
        {
            throw new XmlSchemaException("Expected plist and / or dict elements not found");
        }

        return ParseDictionary(dict);
    }

    /// <summary>
    /// Parse a dictionary element
    /// </summary>
    /// <param name="dictionaryElement"></param>
    /// <returns></returns>
    public static Dictionary<string, dynamic> ParseDictionary(XElement dictionaryElement)
    {
        // A dictionary element looks like this:
        // <dict>
        //   <key>Track ID</key><integer>630</integer>
        //   <key>Date Modified</key><date>2014-12-26T14:17:09Z</date>
        //   ...
        // </dict>
        // It uses key-value pairs of elements

        using var enumerator = dictionaryElement.Elements().GetEnumerator();

        var dictionary = new Dictionary<string, dynamic>();

        // each loop iteration here deals with one key-value pair, adding it to the dictionary
        while (enumerator.MoveNext())
        {
            var key = enumerator.Current.Value; // key always contains a string

            enumerator.MoveNext();

            var valueElement = enumerator.Current;

            var value = valueElement.PlistParseValue();

            dictionary.Add(key, value);
        }

        return dictionary;
    }

    private static readonly Regex SpaceRegex = new(@"\s");

    /// <summary>
    /// Parse a value element
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static dynamic PlistParseValue(this XElement element)
    {
        // the name of the element is its data type
        var type = element.Name.LocalName;

        // See https://en.wikipedia.org/wiki/Property_list#Format for all tags
        return type switch
        {
            "string" => element.Value,
            "real" => float.Parse(element.Value),
            "integer" => long.Parse(element.Value),
            "true" => true,
            "false" => false,
            "date" => DateTime.Parse(element.Value),
            // get rid of new lines and spaces before parsing base64 string
            "data" => Convert.FromBase64String(SpaceRegex.Replace(element.Value, "")),
            "array" => element.Elements().Select(PlistParseValue),
            "dict" => ParseDictionary(element),
            _ => throw new NotImplementedException($"Unsupported datatype {type}")
        };
    }

    public static XElement? GetTopLevelDictionaryElement(XDocument doc) => doc.Element("plist")?.Element("dict");

    /// <summary>
    /// Get the given dict item by key
    /// </summary>
    /// <param name="element">The dict element</param>
    /// <param name="key">The key to look up</param>
    /// <returns></returns>
    public static XElement? PlistDictGet(this XElement element, string key) =>
        (XElement?) element.Elements("key").FirstOrDefault(x => x.Value == key)?.NextNode;

    public static IEnumerable<XElement> PlistDictKeys(this XElement element) =>
        element.Elements("key").Select(x => (XElement?) x.NextNode).Where(x => x != null).Select(x => x!);
}
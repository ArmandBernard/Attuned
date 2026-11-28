using System.Xml.Linq;

namespace iTunesSmartParser.Xml;

/// <summary>
/// Changes the values in a plist dict, in place, so that the document is changed as little as possible
/// </summary>
internal static class PlistDictWriter
{
    /// <summary>
    /// Set a key in a plist dict to the given value element, or remove the key if the value is null.
    /// </summary>
    public static void SetValue(XElement dict, string key, XElement? newValueElement)
    {
        var keyElement = dict.Elements("key").FirstOrDefault(x => x.Value == key);

        if (newValueElement == null)
        {
            if (keyElement == null)
            {
                return;
            }

            DeleteAtKey(keyElement);
        }
        else if (keyElement != null)
        {
            ReplaceAtKey(keyElement, newValueElement);
        }
        else
        {
            AddAfterLast(dict, key, newValueElement);
        }
    }

    private static void DeleteAtKey(XElement keyElement)
    {
        // the plist format is <key>[Name of property]</key><[type of property]>[value of property]</[type of property]>
        var valueElement = keyElement.NextElement();
        (keyElement.PreviousNode as XText)?.Remove(); // remove the whitespace before the key
        keyElement.Remove();
        valueElement?.Remove();
    }

    private static void ReplaceAtKey(XElement keyElement, XElement newValueElement)
    {
        keyElement.NextElement()!.ReplaceWith(newValueElement);
    }

    private static void AddAfterLast(XElement dict, string key, XElement newValueElement)
    {
        var last = dict.Elements().Last();

        // copy the indentation used by the existing entries
        var indentation = dict.Elements("key").First().PreviousNode as XText;

        if (indentation != null)
        {
            last.AddAfterSelf(new XText(indentation.Value), new XElement("key", key), newValueElement);
        }
        else
        {
            last.AddAfterSelf(new XElement("key", key), newValueElement);
        }
    }
}

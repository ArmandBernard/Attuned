using System.Xml;
using System.Xml.Linq;

namespace iTunesSmartParser;

internal static class Extensions
{
    /// <summary>
    /// Get sub array from array using index + length
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static T[] SubArrayL<T>(this T[] data, int index, int length)
    {
        T[] result = new T[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }

    /// <summary>
    /// Get sub array from array using index + length
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static T[] SubArrayR<T>(this T[] data, int start, int end)
    {
        T[] result = new T[end - start];
        Array.Copy(data, start, result, 0, end - start);
        return result;
    }

    /// <summary>
    /// Create a dictionary of name : value pairs for the node's properties. Specific to iTunes'
    /// weird standard
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ToPropertyDictionary(this XElement element)
    {
        // get child elements
        List<XElement> childElements = element
            // descendants of the node i.e. child nodes
            .Elements()
            // only look at key nodes (not dict or value)
            .Where(n => n.Name == "key")
            .ToList();

        // cycle through child elements and create a properties dictionary (key : value)
        return childElements.ToDictionary(
            // property name
            x => x.Value,
            // property value
            x =>
            {
                // Is this a text node? 
                if (x.NextNode.NodeType == XmlNodeType.Text)
                {
                    return ((XText)x.NextNode).Value;
                }
                else
                {
                    XElement next = ((XElement)x.NextNode);

                    // if the node is empty, likely a boolean node.
                    if (next.IsEmpty)
                    {
                        return next.Name.LocalName;
                    }
                    else
                    {
                        // if it's an array node
                        if (next.Name.LocalName == "array")
                        {
                            // return the XML, so it can be interpretted if necessary
                            return next.ToString();
                        }
                        else
                        {
                            // return the value
                            return next.Value;
                        }
                    }
                }
            });
    }
}

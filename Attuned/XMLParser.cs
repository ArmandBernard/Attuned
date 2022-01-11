using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using iTunesSmartParser;

namespace Attuned
{
    public static class iTunesXMLParser
    {
        #region Tracks
        /// <summary>
        /// Load all tracks from the iTunes library XML
        /// </summary>
        /// <param name="path">The path to the XML</param>
        /// <returns></returns>
        public static List<Track> LoadTracks(string path)
        {
            return LoadTracksAsync(path).Result;
        }

        /// <summary>
        /// Load all tracks from the iTunes library XML using the XDoc method
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Task<List<Track>> LoadTracksAsync(string path)
        {
            XDocument doc = XDocument.Load(path);

            // get the node which contains all tracks
            XElement tracksNode = (XElement)doc
                .Element("plist")
                .Element("dict").Elements()
                .First(e => e.Value == "Tracks")
                .NextNode;

            // get a list of track dict elements
            List<XElement> trackElements = tracksNode.Elements().Where(e => e.Name == "dict").ToList();

            LoadingForm form = new LoadingForm()
            {
                MaxValue = trackElements.Count,
                Message = "Loading Tracks..."
            };

            // create reporting object
            Progress<int> progressTracker = new Progress<int>();
            // update progress variable
            progressTracker.ProgressChanged += (s, e) => 
            {
                form.Value = e;
            };

            form.Show();

            Task<List<Track>> task = Task<List<Track>>.Factory.StartNew(
                () => LoadTracks(trackElements, progressTracker)
                );

            return task;
        }

        private static List<Track> LoadTracks(List<XElement> xElements, IProgress<int> progress = null)
        {
            List<Track> tracks = new List<Track>();

            // Cycle through tracks and parse all properties.
            // The property nodes follow the following pattern:
            //  <key>Track ID</key><integer>640</integer>
            //  <key>Size</key><integer>10774278</integer>
            //  ...
            for (int i = 0; i < xElements.Count; i++)
            {
                // report porgress on the task if the functionality exists
                if (progress != null)
                {
                    progress.Report(i);
                }

                tracks.Add(
                    new Track(
                        // cycle through properties and create a properties dictionary (name : value)
                        xElements[i].ToPropertyDictionary()
                        )
                );
            }

            // report porgress on the task
            progress.Report(xElements.Count);

            return tracks;
        }

        #endregion

        public static List<Playlist> LoadSmartPlaylists(string path)
        {
            XDocument doc = XDocument.Load(path);

            // get the node which contains all playlists
            XElement plNode = (XElement)doc
                .Element("plist")
                .Element("dict").Elements()
                .First(e => e.Value == "Playlists")
                .NextNode;

            // get a list of playlist dict elements
            List<XElement> plElements = plNode.Elements().Where(e => e.Name == "dict").ToList();

            return LoadSmartPlaylists(plElements);
        }

        private static List<Playlist> LoadSmartPlaylists(List<XElement> xElements, IProgress<int> progress = null)
        {
            List<Playlist> playlists = new List<Playlist>();

            // Cycle through tracks and parse all properties.
            // The property nodes follow the following pattern:
            //  <key>Track ID</key><integer>640</integer>
            //  <key>Size</key><integer>10774278</integer>
            //  ...
            for (int i = 0; i < xElements.Count; i++)
            {
                // report porgress on the task if the functionality exists
                if (progress != null)
                {
                    progress.Report(i);
                }

                Dictionary<string, string> properties = xElements[i].ToPropertyDictionary();

                Playlist pl = new Playlist()
                {
                    ID = Convert.ToInt32(properties["Playlist ID"]),
                    Name = properties["Name"]
                };

                // if this is a smart playlist with info and criteria fields
                if (properties.ContainsKey("Smart Info") && properties.ContainsKey("Smart Criteria"))
                {
                    pl.IsSmart = true;

                    try
                    {
                        // try to parse playlist info
                        Parser.PlayListInfo pinfo = Parser.Parse(
                            properties["Smart Info"], properties["Smart Criteria"], true, false
                            );

                        // use filter
                        pl.Filter = pinfo.QueryWhere;
                    }
                    catch (Exception ex)
                    {
                        Logger.Write($"Failed to parse playlist '{pl.Name}'.");
                        Logger.Write(ex);
                    }
                }
                else
                {
                    pl.IsSmart = false;
                }

                // if there is an item list
                if (properties.ContainsKey("Playlist Items"))
                {
                    // parse playlist items
                    XDocument xDoc = XDocument.Parse(properties["Playlist Items"]);

                    // get all track IDs
                    pl.Items = xDoc.Descendants()
                        .Where(x => x.Value == "Track ID")
                        .Select(x => Convert.ToInt32(((XElement)x.NextNode).Value))
                        .ToList();
                }
                // if no item list, assume no items
                else
                {
                    // no items
                    pl.Items = new List<int>();
                }

                playlists.Add(pl);
            }

            return playlists;
        }

        /// <summary>
        /// Create a dictionary of name : value pairs for the node's properties. Specific to iTunes'
        /// weird standard
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private static Dictionary<string, string> ToPropertyDictionary(this XElement element)
        {
            // get child elements
            List<XElement> childElements = element
                // descendents of the node i.e. child nodes
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
}

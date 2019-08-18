using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace Attuned
{
    public partial class ShowData : Form
    {
        // list of tracks
        public List<Dictionary<string, string>> trackList = new List<Dictionary<string, string>>();
        // track i.e. list of properties
        private Dictionary<string, string> trackdic = new Dictionary<string, string>();
        // Table containing items to show in gridview
        private DataTable mainDT = new DataTable();
        // dictionary of types for properties
        public Dictionary<string, string> typeDict = new Dictionary<string, string>();
        // the xml document
        public XmlDocument xmldoc;
        XmlNodeList tlkeys;

        public ShowData()
        {
            InitializeComponent();
        }

        private void ShowData_Load(object sender, EventArgs e)
        {
            // load the xml document
            xmldoc = LoadXML();
            if (xmldoc == null)
            {
                MessageBox.Show("Error: Could not find XML file");
                Environment.Exit(1);
            }

            // get the node which contains all tracks
            XmlNode tracknode = xmldoc["plist"]["dict"]["dict"];

            // get a node list of track UIDs
            tlkeys = tracknode.SelectNodes("key");

            // give the user full choice of properties
            foreach (string property in typeDict.Keys)
            {
                propertyCLB.Items.Add(property);
            }
            // Get the data to put in the gridview
            trackList = GetData();

        }

        /// <summary>
        /// Create a dictionary of the properties and their types
        /// </summary>
        /// <returns></returns>
        private string GetType(XmlNode key, string name)
        {

            string type = "string";

            // if it's already in the list, just return it
            if (typeDict.TryGetValue(name, out type))
            {
                return type;
            }

            // get name of property
            name = key.FirstChild.Value;

            // The boolean nodes are orphaned end nodes, (WHY, APPLE?!)
            if (key.NextSibling.NodeType == XmlNodeType.EndElement)
            {
                if (key.NextSibling.Name == "true")
                {
                    type = "boolean";
                }
                else
                {
                    MessageBox.Show("Error: Unexpected end node '" + key.NextSibling.Name + "'.");
                    Environment.Exit(2);
                }
            }
            else
            {
                // get type from type node
                type = key.NextSibling.Name;
            }

            // add property's name and type into the dictionary
            typeDict.Add(name, type);
            return type;

        }

        /// <summary>
        /// Load the data into the datatable and therefore the gridview
        /// </summary>
        private void LoadGridView()
        {

            List<int> typelist = new List<int>();

            // create columns based on requested properties
            foreach (string item in propertyCLB.SelectedItems)
            {
                // lookup type of variable
                if (!typeDict.TryGetValue(item, out string type))
                {
                    MessageBox.Show("Error: type of following selected item not found: '" + item + "'.");
                    Environment.Exit(3);
                }

                // add the type to the list and create a column
                switch (type)
                {
                    case "string":
                        typelist.Add(0);
                        mainDT.Columns.Add(item, typeof(String));
                        break;
                    case "integer":
                        typelist.Add(1);
                        mainDT.Columns.Add(item, typeof(Int32));
                        break;
                    case "date":
                        typelist.Add(2);
                        mainDT.Columns.Add(item, typeof(DateTime));
                        break;
                    default:
                        MessageBox.Show("Error: unrecognized type '" + type + "'.");
                        Environment.Exit(4);
                        break;
                    
                }
            }

            // cycle through tracks and add their info to the gridview
            foreach (Dictionary<string, string> track in trackList)
            {
                // create a new DataRow to later insert to 
                DataRow dr = mainDT.NewRow();
                string value;

                int i = 0;
                foreach (string item in propertyCLB.SelectedItems)
                {
                    // attempt to fetch the value from the track data
                    if (!track.TryGetValue(item, out value))
                    {
                        // if failed, just set as null
                        value = null;
                    }

                    // only bother writing to datarow if value is not null
                    if (value != null)
                    {
                        // extract the column type
                        switch (typelist[i])
                        {
                            case 0:
                                // string
                                dr.SetField(i, value);
                                break;
                            case 1:
                                // Integer
                                if (!Int32.TryParse(value, out int valint))
                                {
                                    MessageBox.Show("Error: failed to parse '" + value + "' to Int32.");
                                    Environment.Exit(5);
                                    break;
                                }
                                // write to column
                                dr.SetField(i, valint);
                                break;
                            case 2:
                                // DateTime
                                if (!DateTime.TryParse(value, out DateTime valdt))
                                {
                                    MessageBox.Show("Error: failed to parse '" + value + "' to DateTime.");
                                    Environment.Exit(6);
                                    break;
                                }
                                dr.SetField(i, valdt);
                                break;
                        }
                    }
                    // increment column counter
                    i++;
                }
            }
        }

        /// <summary>
        /// Get the all the data from the XML file and put it into dictionaries
        /// </summary>
        /// <returns></returns>
        private List<Dictionary<string, string>> GetData()
        {
            List<Dictionary<string, string>> tracklist = new List<Dictionary<string, string>>();

            // cycle through tracks
            foreach (XmlNode node in tlkeys)
            {
                // cycle through the properties of the track
                foreach (XmlNode key in node.NextSibling.SelectNodes("key"))
                {
                    string name, value = "";
                    // get name of property
                    name = key.FirstChild.Value;

                    // find out the type
                    string type = GetType(key, name);

                    // it seems like all booleans that show up are always true
                    if (type == "boolean") { value = "true"; }


                    // add each property's name and value into the dictionary
                    trackdic.Add(name, value);
                }
                // add track to tracklist
                tracklist.Add(trackdic);
            }

            return tracklist;

        }

        /// <summary>
        /// Load the XML document
        /// </summary>
        /// <returns></returns>
        private XmlDocument LoadXML()
        {
            XmlDocument doc = new XmlDocument();
            // location of library xml
            string xmlfileloc = @"C:\Users\arman\Dropbox\Coding\C\iTunesKnockoff\iTunes Library.xml";

            // see if file exists
            if (!File.Exists(xmlfileloc))
            {
                // if no, return null
                return null;
            }

            // load the document
            doc.Load(xmlfileloc);

            return doc;
        }



    }
}

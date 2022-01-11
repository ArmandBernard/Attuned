using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Attuned
{
    public partial class List : Form
    {
        private List<Track> AllTracks;
        private List<Playlist> SmartPlaylists;

        private DataTable TrackDT;
        private DataView AllTracksDV;
        private DataView PlaylistsDV;

        // Table containing items to show in gridview
        private DataTable mainDT = new DataTable();

        // defaults to show
        List<string> DefaultColumns = new List<string> 
        {
            "Name", "Artist", "Album", "Genre", "Rating", "PlayCount" 
        };

        List<string> SelectedColumns;

        public List()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Triggers when the for is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowData_Load(object sender, EventArgs e)
        {
            // load the xml document
            string path = GetXML();

            // force refesh to make form visible
            Refresh();

            LoadTracks(path);

            LoadPlaylists(path);
        }

        #region Library File

        /// <summary>
        /// Get the XML document file path
        /// </summary>
        /// <returns></returns>
        private string GetXML()
        {
            string userfolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // location of library xml
            RegistryWF.SetValueDefault(
                "LibraryPath",
                Path.Combine(userfolder, @"Music\iTunes\iTunes Music Library.xml")
            );
            string xmlfileloc = RegistryWF.GetValue("LibraryPath");

            // see if file exists
            if (!File.Exists(xmlfileloc))
            {
                MessageBox.Show("Error: Could not find XML file. Please select it in the following dialogue.");

                xmlfileloc = ChangeLibraryFileLocation(xmlfileloc);
            }

            return xmlfileloc;
        }

        private string ChangeLibraryFileLocation(string startingLocation)
        {
            string location = startingLocation;

            // start where expected
            filePickerXML.InitialDirectory = startingLocation;
            // show file picker
            DialogResult result = filePickerXML.ShowDialog();
            // If file picked, continue otherwise exit
            if (result == DialogResult.OK)
            {
                // get file location from picker
                location = filePickerXML.FileName;
                // save the location for next time
                RegistryWF.SetValue("LibraryPath", location);
            }

            return location;
        }

        #endregion

        #region Loading

        /// <summary>
        /// Load track list from the Library XML file
        /// </summary>
        /// <param name="path"></param>
        private async void LoadTracks(string path)
        {
            if (!File.Exists(path)) { return; }

            Task<List<Track>> task = iTunesXMLParser.LoadTracksAsync(path);

            await task;

            AllTracks = task.Result;

            TrackDT = AllTracks.ToDataTable();

            BindDataSources();

            UpdateATRowFilter();

            FilterDGVColumns();

            AllTracksStatsLB.Text = MakePlaylistLengthString(TrackDT);
        }

        /// <summary>
        /// Load Playlists from the Library XML File
        /// </summary>
        /// <param name="path"></param>
        private async void LoadPlaylists(string path)
        {
            if (!File.Exists(path)) { return; }

            SmartPlaylists = iTunesXMLParser.LoadSmartPlaylists(path);

            BindDataSources();
        }

        #endregion

        private void BindDataSources()
        {
            AllTracksDV = new DataView(TrackDT);
            AllTracksDGV.DataSource = TrackDT;

            PlaylistsDV = new DataView(TrackDT);
            PlaylistsDGV.DataSource = PlaylistsDV;

            PlaylistListB.DisplayMember = "LineString";
            PlaylistListB.ValueMember = "ID";
            PlaylistListB.DataSource = SmartPlaylists;
        }

        #region DGV

        private void FilterDGVColumns()
        {
            // fetch columns to load from preferences 
            RegistryWF.SetValueDefault("ColsToShow", string.Join(";", DefaultColumns));
            SelectedColumns = RegistryWF.GetValue("ColsToShow")
                .Split(';')
                .Select(s => s.Trim()).ToList();

            // filter the columns
            FilterColumns(SelectedColumns);
        }

        private void FilterColumns(List<string> filter)
        {
            // show / hide columns selected by the user
            foreach (DataGridViewColumn col in AllTracksDGV.Columns)
            {
                col.Visible = filter.Contains(col.Name);
            }
            // show / hide columns selected by the user
            foreach (DataGridViewColumn col in PlaylistsDGV.Columns)
            {
                col.Visible = filter.Contains(col.Name);
            }
        }

        #endregion

        private void EditTrack(int trackID)
        {
            // find the track
            Track track = AllTracks.FirstOrDefault(t => t.TrackID == trackID);

            // track not found
            if (track == null) { return; }

            // create track details form
            TrackDetailsForm form = new TrackDetailsForm(track);

            form.Show();
        }

        private void OpenTrack(int trackID)
        {
            // find the track
            Track track = AllTracks.FirstOrDefault(t => t.TrackID == trackID);

            // track not found
            if (track == null) { return; }

            // run the file, opening it in the default player
            Process.Start(track.WellFormattedLocation);
        }

        private void UpdateATRowFilter()
        {
            string terms = SearchTB.Text;

            if (AllTracksDGV.DataSource == null) { return; }

            AllTracksDV.RowFilter =
                "[Name] LIKE '%" + terms + "%' OR [Album] LIKE '%" + terms + "%'";
        }

        public string MakePlaylistLengthString(DataTable dt)
        {
            // calculate total duration
            TimeSpan totalLength = TimeSpan.FromMilliseconds(
                dt.AsEnumerable()
                    .Where(r => r["TotalTime"] != DBNull.Value)
                    .Sum(r => ((TimeSpan)r["TotalTime"]).TotalMilliseconds)
                );

            // make a string to describe running length
            string timestring = MakeDurationString(totalLength);

            return $"{dt.Rows.Count} tracks • {timestring}";
        }

        /// <summary>
        /// Make a string representing the length of a timespan
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public string MakeDurationString(TimeSpan ts)
        {
            if (ts.TotalDays >= 1)
            {
                return $"{ts.TotalDays:f1} days";
            }
            else if (ts.TotalHours >= 1)
            {
                return 
                    $"{ts.Hours} {(ts.Hours == 1 ? "hour" : "hours" )}," +
                    $" {ts.Minutes} {(ts.Minutes == 1 ? "minute" : "minutes")}";
            }
            else
            {
                return $"{ts.Minutes} {(ts.Minutes == 1 ? "minute" : "minutes")}";
            }            
        }

        #region Event Handlers

        private void MainDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // get the track ID for the clicked row
            int trackID = (int)((DataRowView)AllTracksDGV.Rows[e.RowIndex].DataBoundItem).Row["TrackID"];

            EditTrack(trackID);
        }

        private void EditColumnsBT_Click(object sender, EventArgs e)
        {
            List<string> allColumns = AllTracksDGV.Columns.Cast<DataGridViewColumn>()
                .Select(c => c.Name).ToList();

            // create a column selection form
            EditColumnsForm form = new EditColumnsForm(allColumns, SelectedColumns);

            // set the main form to refresh columns when this form closes
            form.FormClosed += (s, a) =>
            {
                // change the selected columns based on the user's selection in the form
                SelectedColumns = form.Selected;
                // filter the columns
                FilterColumns(SelectedColumns);
                // save selected columns
                RegistryWF.SetValue("ColsToShow", string.Join(";", SelectedColumns));
            };

            // show the form
            form.Show();

            // make the form show below the edit columns button
            form.Location = EditATColumnsBT.PointToScreen(new Point(0, EditATColumnsBT.Height));
        }

        private void OpenLibraryTSMI_Click(object sender, EventArgs e)
        {
            string location = RegistryWF.GetValue("LibraryPath");

            location = ChangeLibraryFileLocation(location);

            RegistryWF.SetValue("LibraryPath", location);

            LoadTracks(location);
        }

        private void MainDGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                AllTracksDGV.Rows[e.RowIndex].Selected = true;

                // get the track ID for the clicked row
                int trackID = 
                    (int)((DataRowView)AllTracksDGV.Rows[e.RowIndex].DataBoundItem).Row["TrackID"];

                ContextMenuStrip menu = new ContextMenuStrip();

                // add a view details right click option
                menu.Items.Add("View Details", null, (s, a) => EditTrack(trackID));
                menu.Items.Add("Open File", null, (s, a) => OpenTrack(trackID));

                menu.Show(Cursor.Position);
            }
        }

        private void SearchTB_TextChanged(object sender, EventArgs e)
        {
            UpdateATRowFilter();
        }

        private void PlaylistListB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Playlist playlist = (Playlist)PlaylistListB.SelectedItem;

            // show name onscreen
            PlaylistNameLB.Text = playlist.Name;

            bool filterSuccess = false;

            // if this is a smart playlist and it has successfully been parsed
            if (playlist.IsSmart && !string.IsNullOrEmpty(playlist.Filter))
            {
                // clear error
                toolTip.SetToolTip(PLErrorPB, "");
                PLErrorPB.Image = null;
                try
                {
                    PlaylistsDV.RowFilter = playlist.Filter;
                    filterSuccess = true;
                }
                catch (Exception ex)
                {
                    Logger.Write(ex);
                    // show error
                    toolTip.SetToolTip(PLErrorPB, "Failed to apply smart playlist filter");
                    PLErrorPB.Image = SystemIcons.Error.ToBitmap();
                }
            }

            // if there is an item list and filter was not already applied
            if (playlist.Items != null && playlist.Items.Count > 0 && !filterSuccess)
            {
                // create filter using all track IDs
                PlaylistsDV.RowFilter = $"TrackID IN ({string.Join(", ", playlist.Items)})";
                filterSuccess = true;
            }
            else if (playlist.Items.Count == 0)
            {
                PlaylistsDV.RowFilter = "TRUE = FALSE";
                filterSuccess = true;
            }

            if (!filterSuccess)
            {
                PlaylistsDV.RowFilter = "";
            }

            if (PlaylistsDV.Table != null)
            {
                // calculate and show stats
                PlaylistStatsLB.Text = MakePlaylistLengthString(PlaylistsDV.ToTable());
            }
        }

        #endregion
    }
}

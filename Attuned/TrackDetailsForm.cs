using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;



namespace Attuned
{
    public partial class TrackDetailsForm : Form
    {
        public Track Track;

        public TrackDetailsForm(Track track)
        {
            InitializeComponent();

            Track = track;

            LoadFields();
        }

        private void LoadFields()
        {
            HeaderTrackNameLB.Text = Track.Name;
            HeaderArtistLB.Text = Track.Artist;
            HeaderAlbumLB.Text = Track.Album;

            TrackNameTB.Text = Track.Name;
            ArtistTB.Text = Track.Artist;
            AlbumTB.Text = Track.Album;
            ComposerTB.Text = Track.Composer;
            GenreTB.Text = Track.Genre;
            YearTB.Text = Track.Year.HasValue ? Track.Year.Value.ToString() : "";
            TrackUD1.Value = Track.TrackNumber ?? 0;
            TrackUD2.Value = Track.TrackCount ?? 0;
            DiscUD1.Value = Track.DiscNumber ?? 0;
            DiscUD2.Value = Track.DiscCount ?? 0;
            RatingUD.Value = Track.Rating ?? 0;
            BPMTB.Text = Track.BPM.HasValue ? Track.BPM.Value.ToString() : "";
            PlayCountNUD.Value = Track.PlayCount;
            LastPlayedLB.Text = Track.PlayDate.HasValue ? 
                "Last Played: " + Track.PlayDate.Value.ToShortDateString() : "";

            FileKindLB.Text = Track.FileI.Exists ? Track.FileI.Extension.Substring(1) : "";
            DurationLB.Text = 
                Track.TotalTime.HasValue ? Track.TotalTime.Value.ToString("hh':'mm':'ss") : "";
            SizeLB.Text = Track.SizeMB.HasValue ? Track.SizeMB.Value.ToString("f2") + " MB" : "";
            BitrateLB.Text = Track.BitRate.HasValue ? Track.BitRate.Value + " kbps" : "";
            SampleRateLB.Text = Track.SampleRate.HasValue ? (Track.SampleRate.Value / 1000F) + " kHz" : "";
            ChannelsLB.Text = Track.Channels.ToString();
            CodecLB.Text = Track.Codec;
            DateModifiedLB.Text = Track.DateModified.ToString();
            DateAddedLB.Text = Track.DateAdded.ToString();
            LocationLB.Text = Track.WellFormattedLocation;

            // load the image
            Track.LoadImage();
            HeaderArtworkPB.Image = Track.CoverArt;
            ArtworkPB.Image = Track.CoverArt;
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // act as if the title bar is clicked so that the window can be moved
                Win32.SendTitleBarClickMessage(this);
            }
        }

        private void CancelBT_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OKBT_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Track.WellFormattedLocation);
        }

        private void copyLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Track.WellFormattedLocation);
        }

        private void TrackDetailsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // dispose of the image to clear memory
            Track.CoverArt.Dispose();
            Track.CoverArt = null;
        }

        private void ArtworkRemoveBT_Click(object sender, EventArgs e)
        {
            Track.CoverArt = null;
            HeaderArtworkPB.Image = null;
            ArtworkPB.Image = null;
        }

        #region Resizeable Borderless Code

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        /// <summary>
        /// Grip Size
        /// </summary>
        const int GripSize = 10;

        private Rectangle TopEdge { get { return new Rectangle(0, 0, this.ClientSize.Width, GripSize); } }
        private Rectangle LeftEdge { get { return new Rectangle(0, 0, GripSize, this.ClientSize.Height); } }
        private Rectangle BottomEdge { get { return new Rectangle(0, this.ClientSize.Height - GripSize, this.ClientSize.Width, GripSize); } }
        private Rectangle RightEdge { get { return new Rectangle(this.ClientSize.Width - GripSize, 0, GripSize, this.ClientSize.Height); } }

        private Rectangle TopLeft { get { return new Rectangle(0, 0, GripSize, GripSize); } }
        private Rectangle TopRight { get { return new Rectangle(this.ClientSize.Width - GripSize, 0, GripSize, GripSize); } }
        private Rectangle BottomLeft { get { return new Rectangle(0, this.ClientSize.Height - GripSize, GripSize, GripSize); } }
        private Rectangle BottomRight { get { return new Rectangle(this.ClientSize.Width - GripSize, this.ClientSize.Height - GripSize, GripSize, GripSize); } }


        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84) // WM_NCHITTEST
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (TopEdge.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (LeftEdge.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (RightEdge.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (BottomEdge.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }

        #endregion
    }
}



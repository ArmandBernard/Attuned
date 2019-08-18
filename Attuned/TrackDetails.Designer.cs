namespace Attuned
{
    partial class TrackDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerAlbumLB = new System.Windows.Forms.Label();
            this.headerArtistLB = new System.Windows.Forms.Label();
            this.headerTrackNameLB = new System.Windows.Forms.Label();
            this.albumArtPB = new System.Windows.Forms.PictureBox();
            this.propertyTabs = new System.Windows.Forms.TabControl();
            this.detailsTab = new System.Windows.Forms.TabPage();
            this.fileTab = new System.Windows.Forms.TabPage();
            this.CancelB = new System.Windows.Forms.Button();
            this.OKB = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.trackNameTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.artistTB = new System.Windows.Forms.TextBox();
            this.albumTB = new System.Windows.Forms.TextBox();
            this.genreTB = new System.Windows.Forms.TextBox();
            this.yearTB = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.TrackUD1 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.TrackUD2 = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.discUD2 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.discUD1 = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.ratingUD = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.bpmTB = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.playcountTB = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.lastplayedLB2 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.commentsTB = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.composerTB = new System.Windows.Forms.TextBox();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumArtPB)).BeginInit();
            this.propertyTabs.SuspendLayout();
            this.detailsTab.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackUD1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackUD2)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.discUD2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discUD1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ratingUD)).BeginInit();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.headerPanel.Controls.Add(this.headerAlbumLB);
            this.headerPanel.Controls.Add(this.headerArtistLB);
            this.headerPanel.Controls.Add(this.headerTrackNameLB);
            this.headerPanel.Controls.Add(this.albumArtPB);
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(599, 184);
            this.headerPanel.TabIndex = 6;
            // 
            // headerAlbumLB
            // 
            this.headerAlbumLB.AutoSize = true;
            this.headerAlbumLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerAlbumLB.Location = new System.Drawing.Point(176, 120);
            this.headerAlbumLB.Name = "headerAlbumLB";
            this.headerAlbumLB.Size = new System.Drawing.Size(72, 20);
            this.headerAlbumLB.TabIndex = 7;
            this.headerAlbumLB.Text = "<Album>";
            // 
            // headerArtistLB
            // 
            this.headerArtistLB.AutoSize = true;
            this.headerArtistLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerArtistLB.Location = new System.Drawing.Point(176, 80);
            this.headerArtistLB.Name = "headerArtistLB";
            this.headerArtistLB.Size = new System.Drawing.Size(64, 20);
            this.headerArtistLB.TabIndex = 6;
            this.headerArtistLB.Text = "<Artist>";
            // 
            // headerTrackNameLB
            // 
            this.headerTrackNameLB.AutoSize = true;
            this.headerTrackNameLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerTrackNameLB.Location = new System.Drawing.Point(176, 40);
            this.headerTrackNameLB.Name = "headerTrackNameLB";
            this.headerTrackNameLB.Size = new System.Drawing.Size(79, 24);
            this.headerTrackNameLB.TabIndex = 5;
            this.headerTrackNameLB.Text = "<Track>";
            // 
            // albumArtPB
            // 
            this.albumArtPB.Location = new System.Drawing.Point(16, 16);
            this.albumArtPB.Name = "albumArtPB";
            this.albumArtPB.Size = new System.Drawing.Size(150, 150);
            this.albumArtPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.albumArtPB.TabIndex = 4;
            this.albumArtPB.TabStop = false;
            // 
            // propertyTabs
            // 
            this.propertyTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyTabs.Controls.Add(this.detailsTab);
            this.propertyTabs.Controls.Add(this.fileTab);
            this.propertyTabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyTabs.Location = new System.Drawing.Point(0, 192);
            this.propertyTabs.Margin = new System.Windows.Forms.Padding(0);
            this.propertyTabs.Name = "propertyTabs";
            this.propertyTabs.Padding = new System.Drawing.Point(5, 5);
            this.propertyTabs.SelectedIndex = 0;
            this.propertyTabs.Size = new System.Drawing.Size(599, 510);
            this.propertyTabs.TabIndex = 8;
            // 
            // detailsTab
            // 
            this.detailsTab.AutoScroll = true;
            this.detailsTab.Controls.Add(this.tableLayoutPanel1);
            this.detailsTab.Location = new System.Drawing.Point(4, 33);
            this.detailsTab.Margin = new System.Windows.Forms.Padding(0);
            this.detailsTab.Name = "detailsTab";
            this.detailsTab.Size = new System.Drawing.Size(591, 473);
            this.detailsTab.TabIndex = 0;
            this.detailsTab.Text = "Details";
            this.detailsTab.UseVisualStyleBackColor = true;
            // 
            // fileTab
            // 
            this.fileTab.Location = new System.Drawing.Point(4, 33);
            this.fileTab.Margin = new System.Windows.Forms.Padding(0);
            this.fileTab.Name = "fileTab";
            this.fileTab.Size = new System.Drawing.Size(626, 443);
            this.fileTab.TabIndex = 1;
            this.fileTab.Text = "File";
            this.fileTab.UseVisualStyleBackColor = true;
            // 
            // CancelB
            // 
            this.CancelB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelB.Location = new System.Drawing.Point(485, 718);
            this.CancelB.Name = "CancelB";
            this.CancelB.Size = new System.Drawing.Size(96, 31);
            this.CancelB.TabIndex = 9;
            this.CancelB.Text = "Cancel";
            this.CancelB.UseVisualStyleBackColor = true;
            // 
            // OKB
            // 
            this.OKB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKB.Location = new System.Drawing.Point(373, 718);
            this.OKB.Name = "OKB";
            this.OKB.Size = new System.Drawing.Size(96, 32);
            this.OKB.TabIndex = 10;
            this.OKB.Text = "OK";
            this.OKB.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.composerTB, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label14, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.commentsTB, 1, 12);
            this.tableLayoutPanel1.Controls.Add(this.label15, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.lastplayedLB2, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.playcountTB, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.bpmTB, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.ratingUD, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.yearTB, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.genreTB, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.albumTB, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.artistTB, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.trackNameTB, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 6);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(16);
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(589, 422);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(59, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 30);
            this.label2.TabIndex = 12;
            this.label2.Text = "track name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackNameTB
            // 
            this.trackNameTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackNameTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackNameTB.Location = new System.Drawing.Point(149, 19);
            this.trackNameTB.Name = "trackNameTB";
            this.trackNameTB.Size = new System.Drawing.Size(421, 22);
            this.trackNameTB.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(93, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 30);
            this.label1.TabIndex = 14;
            this.label1.Text = "album";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(101, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 30);
            this.label3.TabIndex = 15;
            this.label3.Text = "artist";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(95, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 30);
            this.label4.TabIndex = 16;
            this.label4.Text = "genre";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(104, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 30);
            this.label5.TabIndex = 17;
            this.label5.Text = "year";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Right;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(46, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 30);
            this.label6.TabIndex = 18;
            this.label6.Text = "track number";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Right;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(51, 226);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 30);
            this.label7.TabIndex = 19;
            this.label7.Text = "disc number";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // artistTB
            // 
            this.artistTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.artistTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.artistTB.Location = new System.Drawing.Point(149, 49);
            this.artistTB.Name = "artistTB";
            this.artistTB.Size = new System.Drawing.Size(421, 22);
            this.artistTB.TabIndex = 20;
            // 
            // albumTB
            // 
            this.albumTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.albumTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumTB.Location = new System.Drawing.Point(149, 79);
            this.albumTB.Name = "albumTB";
            this.albumTB.Size = new System.Drawing.Size(421, 22);
            this.albumTB.TabIndex = 21;
            // 
            // genreTB
            // 
            this.genreTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.genreTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genreTB.Location = new System.Drawing.Point(149, 139);
            this.genreTB.Name = "genreTB";
            this.genreTB.Size = new System.Drawing.Size(421, 22);
            this.genreTB.TabIndex = 22;
            // 
            // yearTB
            // 
            this.yearTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.yearTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yearTB.Location = new System.Drawing.Point(149, 169);
            this.yearTB.Name = "yearTB";
            this.yearTB.Size = new System.Drawing.Size(421, 22);
            this.yearTB.TabIndex = 23;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.TrackUD2, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.TrackUD1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(146, 196);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(427, 30);
            this.tableLayoutPanel2.TabIndex = 24;
            // 
            // TrackUD1
            // 
            this.TrackUD1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrackUD1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TrackUD1.Location = new System.Drawing.Point(3, 3);
            this.TrackUD1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.TrackUD1.Name = "TrackUD1";
            this.TrackUD1.Size = new System.Drawing.Size(58, 22);
            this.TrackUD1.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(67, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(26, 30);
            this.label8.TabIndex = 19;
            this.label8.Text = "of";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TrackUD2
            // 
            this.TrackUD2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrackUD2.Enabled = false;
            this.TrackUD2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TrackUD2.Location = new System.Drawing.Point(99, 3);
            this.TrackUD2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.TrackUD2.Name = "TrackUD2";
            this.TrackUD2.Size = new System.Drawing.Size(58, 22);
            this.TrackUD2.TabIndex = 20;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.discUD2, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label9, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.discUD1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(146, 226);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(427, 30);
            this.tableLayoutPanel3.TabIndex = 25;
            // 
            // discUD2
            // 
            this.discUD2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.discUD2.Enabled = false;
            this.discUD2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discUD2.Location = new System.Drawing.Point(99, 3);
            this.discUD2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.discUD2.Name = "discUD2";
            this.discUD2.Size = new System.Drawing.Size(58, 22);
            this.discUD2.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(67, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 30);
            this.label9.TabIndex = 19;
            this.label9.Text = "of";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // discUD1
            // 
            this.discUD1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.discUD1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discUD1.Location = new System.Drawing.Point(3, 3);
            this.discUD1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.discUD1.Name = "discUD1";
            this.discUD1.Size = new System.Drawing.Size(58, 22);
            this.discUD1.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Right;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(96, 256);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 30);
            this.label10.TabIndex = 26;
            this.label10.Text = "rating";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ratingUD
            // 
            this.ratingUD.Dock = System.Windows.Forms.DockStyle.Left;
            this.ratingUD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ratingUD.Location = new System.Drawing.Point(149, 259);
            this.ratingUD.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ratingUD.Name = "ratingUD";
            this.ratingUD.Size = new System.Drawing.Size(58, 22);
            this.ratingUD.TabIndex = 27;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Right;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(105, 286);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 30);
            this.label11.TabIndex = 28;
            this.label11.Text = "bpm";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bpmTB
            // 
            this.bpmTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bpmTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bpmTB.Location = new System.Drawing.Point(149, 289);
            this.bpmTB.Name = "bpmTB";
            this.bpmTB.Size = new System.Drawing.Size(421, 22);
            this.bpmTB.TabIndex = 29;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Right;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(64, 316);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 30);
            this.label12.TabIndex = 30;
            this.label12.Text = "play count";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // playcountTB
            // 
            this.playcountTB.Dock = System.Windows.Forms.DockStyle.Left;
            this.playcountTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playcountTB.Location = new System.Drawing.Point(149, 319);
            this.playcountTB.Name = "playcountTB";
            this.playcountTB.Size = new System.Drawing.Size(152, 22);
            this.playcountTB.TabIndex = 31;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Right;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(58, 346);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 30);
            this.label13.TabIndex = 32;
            this.label13.Text = "last played";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lastplayedLB2
            // 
            this.lastplayedLB2.AutoSize = true;
            this.lastplayedLB2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lastplayedLB2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastplayedLB2.Location = new System.Drawing.Point(149, 346);
            this.lastplayedLB2.Name = "lastplayedLB2";
            this.lastplayedLB2.Size = new System.Drawing.Size(421, 30);
            this.lastplayedLB2.TabIndex = 33;
            this.lastplayedLB2.Text = "<last played>";
            this.lastplayedLB2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Right;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(65, 376);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(78, 30);
            this.label15.TabIndex = 34;
            this.label15.Text = "comments";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // commentsTB
            // 
            this.commentsTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commentsTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commentsTB.Location = new System.Drawing.Point(149, 379);
            this.commentsTB.Name = "commentsTB";
            this.commentsTB.Size = new System.Drawing.Size(421, 22);
            this.commentsTB.TabIndex = 35;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Right;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(66, 106);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 30);
            this.label14.TabIndex = 36;
            this.label14.Text = "composer";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // composerTB
            // 
            this.composerTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.composerTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.composerTB.Location = new System.Drawing.Point(149, 109);
            this.composerTB.Name = "composerTB";
            this.composerTB.Size = new System.Drawing.Size(421, 22);
            this.composerTB.TabIndex = 37;
            // 
            // TrackDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 766);
            this.Controls.Add(this.OKB);
            this.Controls.Add(this.CancelB);
            this.Controls.Add(this.propertyTabs);
            this.Controls.Add(this.headerPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(400, 560);
            this.Name = "TrackDetails";
            this.Text = "Details";
            this.Load += new System.EventHandler(this.Details_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumArtPB)).EndInit();
            this.propertyTabs.ResumeLayout(false);
            this.detailsTab.ResumeLayout(false);
            this.detailsTab.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackUD1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackUD2)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.discUD2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discUD1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ratingUD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label headerAlbumLB;
        private System.Windows.Forms.Label headerArtistLB;
        private System.Windows.Forms.Label headerTrackNameLB;
        private System.Windows.Forms.PictureBox albumArtPB;
        private System.Windows.Forms.TabControl propertyTabs;
        private System.Windows.Forms.TabPage detailsTab;
        private System.Windows.Forms.TabPage fileTab;
        private System.Windows.Forms.Button CancelB;
        private System.Windows.Forms.Button OKB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox trackNameTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox yearTB;
        private System.Windows.Forms.TextBox genreTB;
        private System.Windows.Forms.TextBox albumTB;
        private System.Windows.Forms.TextBox artistTB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown TrackUD1;
        private System.Windows.Forms.NumericUpDown TrackUD2;
        private System.Windows.Forms.NumericUpDown ratingUD;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.NumericUpDown discUD2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown discUD1;
        private System.Windows.Forms.TextBox bpmTB;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lastplayedLB2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox playcountTB;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox commentsTB;
        private System.Windows.Forms.TextBox composerTB;
        private System.Windows.Forms.Label label14;
    }
}
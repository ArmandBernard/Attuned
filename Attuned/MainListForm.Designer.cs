namespace Attuned
{
    partial class List
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.AllTracksDGV = new System.Windows.Forms.DataGridView();
            this.AllTracksLB = new System.Windows.Forms.Label();
            this.filePickerXML = new System.Windows.Forms.OpenFileDialog();
            this.EditATColumnsBT = new System.Windows.Forms.Button();
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenLibraryTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.AllTracksStatsLB = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.AllTracksPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.PlaylistsTab = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.PlaylistListB = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.PlaylistsDGV = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.EditPLColumnsBT = new System.Windows.Forms.Button();
            this.PlaylistStatsLB = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.PlaylistNameLB = new System.Windows.Forms.Label();
            this.PLErrorPB = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.SearchTB = new Attuned.WaterMarkTB();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.AllTracksDGV)).BeginInit();
            this.MainMenuStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.AllTracksPage.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.PlaylistsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlaylistsDGV)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PLErrorPB)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // AllTracksDGV
            // 
            this.AllTracksDGV.AllowUserToAddRows = false;
            this.AllTracksDGV.AllowUserToDeleteRows = false;
            this.AllTracksDGV.AllowUserToOrderColumns = true;
            this.AllTracksDGV.AllowUserToResizeRows = false;
            this.AllTracksDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AllTracksDGV.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.AllTracksDGV.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.AllTracksDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.AllTracksDGV.DefaultCellStyle = dataGridViewCellStyle1;
            this.AllTracksDGV.Location = new System.Drawing.Point(10, 66);
            this.AllTracksDGV.Margin = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.AllTracksDGV.MultiSelect = false;
            this.AllTracksDGV.Name = "AllTracksDGV";
            this.AllTracksDGV.ReadOnly = true;
            this.AllTracksDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AllTracksDGV.Size = new System.Drawing.Size(769, 628);
            this.AllTracksDGV.TabIndex = 0;
            this.AllTracksDGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MainDGV_CellDoubleClick);
            this.AllTracksDGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainDGV_CellMouseClick);
            // 
            // AllTracksLB
            // 
            this.AllTracksLB.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AllTracksLB.AutoSize = true;
            this.AllTracksLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AllTracksLB.Location = new System.Drawing.Point(5, 10);
            this.AllTracksLB.Margin = new System.Windows.Forms.Padding(5, 10, 0, 0);
            this.AllTracksLB.Name = "AllTracksLB";
            this.AllTracksLB.Size = new System.Drawing.Size(107, 25);
            this.AllTracksLB.TabIndex = 1;
            this.AllTracksLB.Text = "All Tracks";
            // 
            // filePickerXML
            // 
            this.filePickerXML.FileName = "iTunes Library.XML";
            this.filePickerXML.Filter = "XML files|*.xml";
            // 
            // EditATColumnsBT
            // 
            this.EditATColumnsBT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditATColumnsBT.AutoSize = true;
            this.EditATColumnsBT.FlatAppearance.BorderSize = 0;
            this.EditATColumnsBT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EditATColumnsBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditATColumnsBT.Location = new System.Drawing.Point(644, 0);
            this.EditATColumnsBT.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.EditATColumnsBT.Name = "EditATColumnsBT";
            this.EditATColumnsBT.Size = new System.Drawing.Size(135, 31);
            this.EditATColumnsBT.TabIndex = 4;
            this.EditATColumnsBT.Text = "Show / Hide Columns";
            this.EditATColumnsBT.UseVisualStyleBackColor = true;
            this.EditATColumnsBT.Click += new System.EventHandler(this.EditColumnsBT_Click);
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileTSMI});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(45, 24);
            this.MainMenuStrip.TabIndex = 5;
            this.MainMenuStrip.Text = "menuStrip1";
            // 
            // FileTSMI
            // 
            this.FileTSMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenLibraryTSMI});
            this.FileTSMI.Name = "FileTSMI";
            this.FileTSMI.Size = new System.Drawing.Size(37, 20);
            this.FileTSMI.Text = "File";
            // 
            // OpenLibraryTSMI
            // 
            this.OpenLibraryTSMI.Name = "OpenLibraryTSMI";
            this.OpenLibraryTSMI.Size = new System.Drawing.Size(172, 22);
            this.OpenLibraryTSMI.Text = "Open Library File...";
            this.OpenLibraryTSMI.Click += new System.EventHandler(this.OpenLibraryTSMI_Click);
            // 
            // AllTracksStatsLB
            // 
            this.AllTracksStatsLB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AllTracksStatsLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.AllTracksStatsLB.Location = new System.Drawing.Point(8, 5);
            this.AllTracksStatsLB.Margin = new System.Windows.Forms.Padding(8, 5, 0, 0);
            this.AllTracksStatsLB.Name = "AllTracksStatsLB";
            this.AllTracksStatsLB.Size = new System.Drawing.Size(314, 26);
            this.AllTracksStatsLB.TabIndex = 7;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.AllTracksPage);
            this.tabControl1.Controls.Add(this.PlaylistsTab);
            this.tabControl1.Location = new System.Drawing.Point(0, 30);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(822, 718);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 8;
            // 
            // AllTracksPage
            // 
            this.AllTracksPage.Controls.Add(this.tableLayoutPanel1);
            this.AllTracksPage.Location = new System.Drawing.Point(23, 4);
            this.AllTracksPage.Name = "AllTracksPage";
            this.AllTracksPage.Padding = new System.Windows.Forms.Padding(3);
            this.AllTracksPage.Size = new System.Drawing.Size(795, 710);
            this.AllTracksPage.TabIndex = 0;
            this.AllTracksPage.Text = "All Tracks";
            this.AllTracksPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.AllTracksDGV, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.AllTracksLB, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(789, 704);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.EditATColumnsBT, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.AllTracksStatsLB, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 35);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(789, 31);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // PlaylistsTab
            // 
            this.PlaylistsTab.Controls.Add(this.splitContainer1);
            this.PlaylistsTab.Location = new System.Drawing.Point(23, 4);
            this.PlaylistsTab.Name = "PlaylistsTab";
            this.PlaylistsTab.Padding = new System.Windows.Forms.Padding(3);
            this.PlaylistsTab.Size = new System.Drawing.Size(795, 710);
            this.PlaylistsTab.TabIndex = 1;
            this.PlaylistsTab.Text = "Playlists";
            this.PlaylistsTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.PlaylistListB);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel4);
            this.splitContainer1.Size = new System.Drawing.Size(789, 704);
            this.splitContainer1.SplitterDistance = 232;
            this.splitContainer1.TabIndex = 2;
            // 
            // PlaylistListB
            // 
            this.PlaylistListB.BackColor = System.Drawing.SystemColors.Control;
            this.PlaylistListB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlaylistListB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlaylistListB.FormattingEnabled = true;
            this.PlaylistListB.ItemHeight = 16;
            this.PlaylistListB.Location = new System.Drawing.Point(0, 0);
            this.PlaylistListB.Name = "PlaylistListB";
            this.PlaylistListB.Size = new System.Drawing.Size(232, 704);
            this.PlaylistListB.TabIndex = 0;
            this.PlaylistListB.SelectedIndexChanged += new System.EventHandler(this.PlaylistListB_SelectedIndexChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.PlaylistsDGV, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(553, 704);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // PlaylistsDGV
            // 
            this.PlaylistsDGV.AllowUserToAddRows = false;
            this.PlaylistsDGV.AllowUserToDeleteRows = false;
            this.PlaylistsDGV.AllowUserToOrderColumns = true;
            this.PlaylistsDGV.AllowUserToResizeRows = false;
            this.PlaylistsDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlaylistsDGV.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.PlaylistsDGV.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.PlaylistsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.PlaylistsDGV.DefaultCellStyle = dataGridViewCellStyle2;
            this.PlaylistsDGV.Location = new System.Drawing.Point(10, 71);
            this.PlaylistsDGV.Margin = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.PlaylistsDGV.MultiSelect = false;
            this.PlaylistsDGV.Name = "PlaylistsDGV";
            this.PlaylistsDGV.ReadOnly = true;
            this.PlaylistsDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PlaylistsDGV.Size = new System.Drawing.Size(533, 623);
            this.PlaylistsDGV.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.Controls.Add(this.EditPLColumnsBT, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.PlaylistStatsLB, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(553, 71);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // EditPLColumnsBT
            // 
            this.EditPLColumnsBT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditPLColumnsBT.AutoSize = true;
            this.EditPLColumnsBT.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EditPLColumnsBT.FlatAppearance.BorderSize = 0;
            this.EditPLColumnsBT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EditPLColumnsBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditPLColumnsBT.Location = new System.Drawing.Point(408, 46);
            this.EditPLColumnsBT.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.EditPLColumnsBT.Name = "EditPLColumnsBT";
            this.EditPLColumnsBT.Size = new System.Drawing.Size(135, 25);
            this.EditPLColumnsBT.TabIndex = 4;
            this.EditPLColumnsBT.Text = "Show / Hide Columns";
            this.EditPLColumnsBT.UseVisualStyleBackColor = true;
            this.EditPLColumnsBT.Click += new System.EventHandler(this.EditColumnsBT_Click);
            // 
            // PlaylistStatsLB
            // 
            this.PlaylistStatsLB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlaylistStatsLB.AutoSize = true;
            this.PlaylistStatsLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.PlaylistStatsLB.Location = new System.Drawing.Point(8, 51);
            this.PlaylistStatsLB.Margin = new System.Windows.Forms.Padding(8, 5, 0, 0);
            this.PlaylistStatsLB.MinimumSize = new System.Drawing.Size(50, 20);
            this.PlaylistStatsLB.Name = "PlaylistStatsLB";
            this.PlaylistStatsLB.Size = new System.Drawing.Size(50, 20);
            this.PlaylistStatsLB.TabIndex = 7;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel5.SetColumnSpan(this.tableLayoutPanel6, 3);
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.55393F));
            this.tableLayoutPanel6.Controls.Add(this.PlaylistNameLB, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.PLErrorPB, 1, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(547, 40);
            this.tableLayoutPanel6.TabIndex = 1;
            // 
            // PlaylistNameLB
            // 
            this.PlaylistNameLB.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PlaylistNameLB.AutoSize = true;
            this.PlaylistNameLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlaylistNameLB.Location = new System.Drawing.Point(5, 12);
            this.PlaylistNameLB.Margin = new System.Windows.Forms.Padding(5, 10, 0, 0);
            this.PlaylistNameLB.Name = "PlaylistNameLB";
            this.PlaylistNameLB.Size = new System.Drawing.Size(167, 25);
            this.PlaylistNameLB.TabIndex = 1;
            this.PlaylistNameLB.Text = "<Playlist Name>";
            // 
            // PLErrorPB
            // 
            this.PLErrorPB.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.PLErrorPB.Location = new System.Drawing.Point(517, 10);
            this.PLErrorPB.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.PLErrorPB.Name = "PLErrorPB";
            this.PLErrorPB.Size = new System.Drawing.Size(30, 30);
            this.PLErrorPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PLErrorPB.TabIndex = 2;
            this.PLErrorPB.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.SearchTB, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(462, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(360, 30);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // SearchTB
            // 
            this.SearchTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchTB.Location = new System.Drawing.Point(183, 5);
            this.SearchTB.Name = "SearchTB";
            this.SearchTB.Size = new System.Drawing.Size(174, 20);
            this.SearchTB.TabIndex = 0;
            this.SearchTB.WatermarkText = "🔍 Search";
            this.SearchTB.TextChanged += new System.EventHandler(this.SearchTB_TextChanged);
            // 
            // List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 750);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.MainMenuStrip);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "List";
            this.Text = "Attuned";
            this.Load += new System.EventHandler(this.ShowData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AllTracksDGV)).EndInit();
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.AllTracksPage.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.PlaylistsTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlaylistsDGV)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PLErrorPB)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView AllTracksDGV;
        private System.Windows.Forms.Label AllTracksLB;
        private System.Windows.Forms.OpenFileDialog filePickerXML;
        private System.Windows.Forms.Button EditATColumnsBT;
        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileTSMI;
        private System.Windows.Forms.ToolStripMenuItem OpenLibraryTSMI;
        private System.Windows.Forms.Label AllTracksStatsLB;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage AllTracksPage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TabPage PlaylistsTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private WaterMarkTB SearchTB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.DataGridView PlaylistsDGV;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button EditPLColumnsBT;
        private System.Windows.Forms.Label PlaylistStatsLB;
        private System.Windows.Forms.Label PlaylistNameLB;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox PlaylistListB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.PictureBox PLErrorPB;
        private System.Windows.Forms.ToolTip toolTip;
    }
}


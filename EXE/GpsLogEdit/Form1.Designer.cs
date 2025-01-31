namespace GpsLogEdit
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            listGpxLog = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            columnHeader7 = new ColumnHeader();
            columnHeader8 = new ColumnHeader();
            menuStrip1 = new MenuStrip();
            ファイルFToolStripMenuItem = new ToolStripMenuItem();
            新規作成NToolStripMenuItem = new ToolStripMenuItem();
            開くOToolStripMenuItem = new ToolStripMenuItem();
            appendToolStripMenuItem = new ToolStripMenuItem();
            読み込んでいるファイル一覧を表示LToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            プロジェクトファイルを開くPToolStripMenuItem = new ToolStripMenuItem();
            プロジェクトファイルを保存SToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            saveToGpxToolStripMenuItem = new ToolStripMenuItem();
            saveToKmlToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            終了XToolStripMenuItem = new ToolStripMenuItem();
            表示VToolStripMenuItem = new ToolStripMenuItem();
            座標表示形式DToolStripMenuItem = new ToolStripMenuItem();
            dEGToolStripMenuItem = new ToolStripMenuItem();
            dMSToolStripMenuItem = new ToolStripMenuItem();
            オプションOToolStripMenuItem = new ToolStripMenuItem();
            saveWindowPosToolStripMenuItem = new ToolStripMenuItem();
            HelpToolStripMenuItem = new ToolStripMenuItem();
            HelpContextToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            gpxEditについてAToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel2 = new TableLayoutPanel();
            mapControl1 = new Mapsui.UI.WindowsForms.MapControl();
            buttonPrevSplitPos = new Button();
            labelMoveSplitPos = new Label();
            buttonNextSplitPos = new Button();
            buttonSplitGpxFile = new Button();
            buttonDeleteGpxFile = new Button();
            buttonMark = new Button();
            buttonSelectToMark = new Button();
            linkLabelMarkPos = new LinkLabel();
            menuStrip1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // listGpxLog
            // 
            listGpxLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listGpxLog.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6, columnHeader7, columnHeader8 });
            tableLayoutPanel2.SetColumnSpan(listGpxLog, 9);
            listGpxLog.FullRowSelect = true;
            listGpxLog.Location = new Point(20, 3);
            listGpxLog.Margin = new Padding(20, 3, 20, 0);
            listGpxLog.Name = "listGpxLog";
            listGpxLog.Size = new Size(824, 292);
            listGpxLog.TabIndex = 0;
            listGpxLog.UseCompatibleStateImageBehavior = false;
            listGpxLog.View = View.Details;
            listGpxLog.VirtualMode = true;
            listGpxLog.RetrieveVirtualItem += listViewGpxLog_RetrieveVirtualItem;
            listGpxLog.SelectedIndexChanged += listViewGpxLog_SelectedIndexChanged;
            listGpxLog.VirtualItemsSelectionRangeChanged += listViewGpxLog_VirtualItemsSelectionRangeChanged;
            listGpxLog.MouseClick += listViewGpxLog_MouseClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "No.";
            columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "緯度";
            columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "経度";
            columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "標高";
            columnHeader4.Width = 90;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "時刻";
            columnHeader5.Width = 160;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "スピード(km/h)";
            columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "ファイル＃";
            // 
            // columnHeader8
            // 
            columnHeader8.Text = "状態";
            columnHeader8.Width = 100;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { ファイルFToolStripMenuItem, 表示VToolStripMenuItem, オプションOToolStripMenuItem, HelpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(864, 24);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            // 
            // ファイルFToolStripMenuItem
            // 
            ファイルFToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 新規作成NToolStripMenuItem, 開くOToolStripMenuItem, appendToolStripMenuItem, 読み込んでいるファイル一覧を表示LToolStripMenuItem, toolStripSeparator4, プロジェクトファイルを開くPToolStripMenuItem, プロジェクトファイルを保存SToolStripMenuItem, toolStripSeparator3, saveToGpxToolStripMenuItem, saveToKmlToolStripMenuItem, toolStripSeparator2, 終了XToolStripMenuItem });
            ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
            ファイルFToolStripMenuItem.Size = new Size(70, 20);
            ファイルFToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // 新規作成NToolStripMenuItem
            // 
            新規作成NToolStripMenuItem.Name = "新規作成NToolStripMenuItem";
            新規作成NToolStripMenuItem.Size = new Size(296, 22);
            新規作成NToolStripMenuItem.Text = "新規作成(&N)";
            新規作成NToolStripMenuItem.Click += menuCreateNew_Click;
            // 
            // 開くOToolStripMenuItem
            // 
            開くOToolStripMenuItem.Name = "開くOToolStripMenuItem";
            開くOToolStripMenuItem.Size = new Size(296, 22);
            開くOToolStripMenuItem.Text = "データファイルを開く(&O)...";
            開くOToolStripMenuItem.Click += menuOpenFile_Click;
            // 
            // appendToolStripMenuItem
            // 
            appendToolStripMenuItem.Name = "appendToolStripMenuItem";
            appendToolStripMenuItem.Size = new Size(296, 22);
            appendToolStripMenuItem.Text = "データファイルを追加で開く(&A)...";
            appendToolStripMenuItem.Click += menuAppendFile_Click;
            // 
            // 読み込んでいるファイル一覧を表示LToolStripMenuItem
            // 
            読み込んでいるファイル一覧を表示LToolStripMenuItem.Name = "読み込んでいるファイル一覧を表示LToolStripMenuItem";
            読み込んでいるファイル一覧を表示LToolStripMenuItem.Size = new Size(296, 22);
            読み込んでいるファイル一覧を表示LToolStripMenuItem.Text = "読み込んでいるデータファイル一覧を表示(&L)...";
            読み込んでいるファイル一覧を表示LToolStripMenuItem.Click += menuShowReadFile_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(293, 6);
            // 
            // プロジェクトファイルを開くPToolStripMenuItem
            // 
            プロジェクトファイルを開くPToolStripMenuItem.Name = "プロジェクトファイルを開くPToolStripMenuItem";
            プロジェクトファイルを開くPToolStripMenuItem.Size = new Size(296, 22);
            プロジェクトファイルを開くPToolStripMenuItem.Text = "プロジェクトファイルを開く(&P)...";
            プロジェクトファイルを開くPToolStripMenuItem.Click += menuOpenProject_Click;
            // 
            // プロジェクトファイルを保存SToolStripMenuItem
            // 
            プロジェクトファイルを保存SToolStripMenuItem.Name = "プロジェクトファイルを保存SToolStripMenuItem";
            プロジェクトファイルを保存SToolStripMenuItem.Size = new Size(296, 22);
            プロジェクトファイルを保存SToolStripMenuItem.Text = "プロジェクトファイルに保存(&S)...";
            プロジェクトファイルを保存SToolStripMenuItem.Click += menuSaveProject_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(293, 6);
            // 
            // saveToGpxToolStripMenuItem
            // 
            saveToGpxToolStripMenuItem.Name = "saveToGpxToolStripMenuItem";
            saveToGpxToolStripMenuItem.Size = new Size(296, 22);
            saveToGpxToolStripMenuItem.Text = "GPXファイルに保存(&G)...";
            saveToGpxToolStripMenuItem.Click += menuSaveGpxFile_Click;
            // 
            // saveToKmlToolStripMenuItem
            // 
            saveToKmlToolStripMenuItem.Name = "saveToKmlToolStripMenuItem";
            saveToKmlToolStripMenuItem.Size = new Size(296, 22);
            saveToKmlToolStripMenuItem.Text = "KMLファイルに保存(&K)...";
            saveToKmlToolStripMenuItem.Click += menuSaveKmlFile_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(293, 6);
            // 
            // 終了XToolStripMenuItem
            // 
            終了XToolStripMenuItem.Name = "終了XToolStripMenuItem";
            終了XToolStripMenuItem.Size = new Size(296, 22);
            終了XToolStripMenuItem.Text = "終了(&X)";
            終了XToolStripMenuItem.Click += menuExit_Click;
            // 
            // 表示VToolStripMenuItem
            // 
            表示VToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 座標表示形式DToolStripMenuItem });
            表示VToolStripMenuItem.Name = "表示VToolStripMenuItem";
            表示VToolStripMenuItem.Size = new Size(61, 20);
            表示VToolStripMenuItem.Text = "表示(&V)";
            // 
            // 座標表示形式DToolStripMenuItem
            // 
            座標表示形式DToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { dEGToolStripMenuItem, dMSToolStripMenuItem });
            座標表示形式DToolStripMenuItem.Name = "座標表示形式DToolStripMenuItem";
            座標表示形式DToolStripMenuItem.Size = new Size(165, 22);
            座標表示形式DToolStripMenuItem.Text = "座標表示形式(&D)";
            // 
            // dEGToolStripMenuItem
            // 
            dEGToolStripMenuItem.Name = "dEGToolStripMenuItem";
            dEGToolStripMenuItem.Size = new Size(121, 22);
            dEGToolStripMenuItem.Text = "DEG(&E)";
            dEGToolStripMenuItem.Click += menuSelectDeg_Click;
            // 
            // dMSToolStripMenuItem
            // 
            dMSToolStripMenuItem.Name = "dMSToolStripMenuItem";
            dMSToolStripMenuItem.Size = new Size(121, 22);
            dMSToolStripMenuItem.Text = "DMS(&M)";
            dMSToolStripMenuItem.Click += menuSelectDms_Click;
            // 
            // オプションOToolStripMenuItem
            // 
            オプションOToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveWindowPosToolStripMenuItem });
            オプションOToolStripMenuItem.Name = "オプションOToolStripMenuItem";
            オプションOToolStripMenuItem.Size = new Size(61, 20);
            オプションOToolStripMenuItem.Text = "設定(&S)";
            // 
            // saveWindowPosToolStripMenuItem
            // 
            saveWindowPosToolStripMenuItem.CheckOnClick = true;
            saveWindowPosToolStripMenuItem.Name = "saveWindowPosToolStripMenuItem";
            saveWindowPosToolStripMenuItem.Size = new Size(221, 22);
            saveWindowPosToolStripMenuItem.Text = "ウインドウの位置を保存する(&P)";
            // 
            // HelpToolStripMenuItem
            // 
            HelpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { HelpContextToolStripMenuItem, toolStripSeparator1, gpxEditについてAToolStripMenuItem });
            HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            HelpToolStripMenuItem.Size = new Size(67, 20);
            HelpToolStripMenuItem.Text = "ヘルプ(&H)";
            // 
            // HelpContextToolStripMenuItem
            // 
            HelpContextToolStripMenuItem.Name = "HelpContextToolStripMenuItem";
            HelpContextToolStripMenuItem.Size = new Size(206, 22);
            HelpContextToolStripMenuItem.Text = "目次(&C)";
            HelpContextToolStripMenuItem.Click += menuHelpContext_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(203, 6);
            // 
            // gpxEditについてAToolStripMenuItem
            // 
            gpxEditについてAToolStripMenuItem.Name = "gpxEditについてAToolStripMenuItem";
            gpxEditについてAToolStripMenuItem.Size = new Size(206, 22);
            gpxEditについてAToolStripMenuItem.Text = "GpsLogEditについて(&A)...";
            gpxEditについてAToolStripMenuItem.Click += menuAbout_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 9;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(mapControl1, 0, 2);
            tableLayoutPanel2.Controls.Add(listGpxLog, 0, 0);
            tableLayoutPanel2.Controls.Add(buttonPrevSplitPos, 2, 1);
            tableLayoutPanel2.Controls.Add(labelMoveSplitPos, 3, 1);
            tableLayoutPanel2.Controls.Add(buttonNextSplitPos, 4, 1);
            tableLayoutPanel2.Controls.Add(buttonSplitGpxFile, 0, 1);
            tableLayoutPanel2.Controls.Add(buttonDeleteGpxFile, 1, 1);
            tableLayoutPanel2.Controls.Add(buttonMark, 5, 1);
            tableLayoutPanel2.Controls.Add(buttonSelectToMark, 7, 1);
            tableLayoutPanel2.Controls.Add(linkLabelMarkPos, 6, 1);
            tableLayoutPanel2.Location = new Point(0, 27);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 48.6486473F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 51.3513527F));
            tableLayoutPanel2.Size = new Size(864, 648);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // mapControl1
            // 
            mapControl1.AutoSize = true;
            mapControl1.BackColor = Color.White;
            mapControl1.BorderStyle = BorderStyle.FixedSingle;
            tableLayoutPanel2.SetColumnSpan(mapControl1, 9);
            mapControl1.Dock = DockStyle.Fill;
            mapControl1.Location = new Point(20, 335);
            mapControl1.Margin = new Padding(20, 0, 20, 20);
            mapControl1.Name = "mapControl1";
            mapControl1.Size = new Size(824, 293);
            mapControl1.TabIndex = 9;
            mapControl1.UpdateInterval = 16;
            // 
            // buttonPrevSplitPos
            // 
            buttonPrevSplitPos.Anchor = AnchorStyles.Right;
            buttonPrevSplitPos.Location = new Point(290, 306);
            buttonPrevSplitPos.Margin = new Padding(3, 8, 3, 3);
            buttonPrevSplitPos.Name = "buttonPrevSplitPos";
            buttonPrevSplitPos.Size = new Size(23, 23);
            buttonPrevSplitPos.TabIndex = 3;
            buttonPrevSplitPos.Text = "▲";
            buttonPrevSplitPos.UseVisualStyleBackColor = true;
            buttonPrevSplitPos.Click += buttonPrevSplitPos_Click;
            // 
            // labelMoveSplitPos
            // 
            labelMoveSplitPos.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            labelMoveSplitPos.AutoSize = true;
            labelMoveSplitPos.Location = new Point(319, 310);
            labelMoveSplitPos.Margin = new Padding(3, 6, 3, 0);
            labelMoveSplitPos.Name = "labelMoveSplitPos";
            labelMoveSplitPos.Size = new Size(89, 15);
            labelMoveSplitPos.TabIndex = 4;
            labelMoveSplitPos.Text = "編集位置へ移動";
            labelMoveSplitPos.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // buttonNextSplitPos
            // 
            buttonNextSplitPos.Anchor = AnchorStyles.Left;
            buttonNextSplitPos.Location = new Point(414, 306);
            buttonNextSplitPos.Margin = new Padding(3, 8, 3, 3);
            buttonNextSplitPos.Name = "buttonNextSplitPos";
            buttonNextSplitPos.Size = new Size(23, 23);
            buttonNextSplitPos.TabIndex = 5;
            buttonNextSplitPos.Text = "▼";
            buttonNextSplitPos.UseVisualStyleBackColor = true;
            buttonNextSplitPos.Click += buttonNextSplitPos_Click;
            // 
            // buttonSplitGpxFile
            // 
            buttonSplitGpxFile.AutoSize = true;
            buttonSplitGpxFile.Location = new Point(20, 303);
            buttonSplitGpxFile.Margin = new Padding(20, 8, 3, 3);
            buttonSplitGpxFile.Name = "buttonSplitGpxFile";
            buttonSplitGpxFile.Size = new Size(103, 25);
            buttonSplitGpxFile.TabIndex = 1;
            buttonSplitGpxFile.Text = "ここで分割／解除";
            buttonSplitGpxFile.UseVisualStyleBackColor = true;
            buttonSplitGpxFile.Click += buttonSplitGpxFile_Click;
            // 
            // buttonDeleteGpxFile
            // 
            buttonDeleteGpxFile.AutoSize = true;
            buttonDeleteGpxFile.Location = new Point(153, 303);
            buttonDeleteGpxFile.Margin = new Padding(3, 8, 3, 3);
            buttonDeleteGpxFile.Name = "buttonDeleteGpxFile";
            buttonDeleteGpxFile.Size = new Size(110, 25);
            buttonDeleteGpxFile.TabIndex = 6;
            buttonDeleteGpxFile.Text = "範囲を削除／解除";
            buttonDeleteGpxFile.UseVisualStyleBackColor = true;
            buttonDeleteGpxFile.Click += buttonDeleteGpxFile_Click;
            // 
            // buttonMark
            // 
            buttonMark.AutoSize = true;
            buttonMark.Location = new Point(464, 303);
            buttonMark.Margin = new Padding(3, 8, 3, 3);
            buttonMark.Name = "buttonMark";
            buttonMark.Size = new Size(95, 25);
            buttonMark.TabIndex = 7;
            buttonMark.Text = "この位置をマーク";
            buttonMark.UseVisualStyleBackColor = true;
            buttonMark.Click += buttomMark_Click;
            // 
            // buttonSelectToMark
            // 
            buttonSelectToMark.AutoSize = true;
            buttonSelectToMark.Location = new Point(675, 303);
            buttonSelectToMark.Margin = new Padding(3, 8, 3, 3);
            buttonSelectToMark.Name = "buttonSelectToMark";
            buttonSelectToMark.Size = new Size(111, 25);
            buttonSelectToMark.TabIndex = 8;
            buttonSelectToMark.Text = "マーク位置まで選択";
            buttonSelectToMark.UseVisualStyleBackColor = true;
            buttonSelectToMark.Click += buttonSelectToMark_Click;
            // 
            // linkLabelMarkPos
            // 
            linkLabelMarkPos.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            linkLabelMarkPos.AutoSize = true;
            linkLabelMarkPos.Location = new Point(562, 310);
            linkLabelMarkPos.Margin = new Padding(0, 6, 0, 0);
            linkLabelMarkPos.Name = "linkLabelMarkPos";
            linkLabelMarkPos.Size = new Size(110, 15);
            linkLabelMarkPos.TabIndex = 10;
            linkLabelMarkPos.TabStop = true;
            linkLabelMarkPos.Text = "linkLabel1";
            linkLabelMarkPos.LinkClicked += linkLabelMarkPos_Clicked;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(864, 675);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(880, 714);
            Name = "Form1";
            Text = "GpsLogEdit";
            FormClosing += form1_FormClosing;
            Load += form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ListView listGpxLog;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem ファイルFToolStripMenuItem;
        private ToolStripMenuItem オプションOToolStripMenuItem;
        private ToolStripMenuItem HelpToolStripMenuItem;
        private ToolStripMenuItem gpxEditについてAToolStripMenuItem;
        private ToolStripMenuItem 終了XToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel2;
        private ToolStripMenuItem HelpContextToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private Button buttonSplitGpxFile;
        private ColumnHeader columnHeader7;
        private ToolStripMenuItem saveToGpxToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private Button buttonPrevSplitPos;
        private Label labelMoveSplitPos;
        private Button buttonNextSplitPos;
        private Button buttonDeleteGpxFile;
        private ToolStripMenuItem 開くOToolStripMenuItem;
        private ToolStripMenuItem appendToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem 表示VToolStripMenuItem;
        private ToolStripMenuItem 座標表示形式DToolStripMenuItem;
        private ToolStripMenuItem dEGToolStripMenuItem;
        private ToolStripMenuItem dMSToolStripMenuItem;
        private ColumnHeader columnHeader8;
        private ToolStripMenuItem 読み込んでいるファイル一覧を表示LToolStripMenuItem;
        private ToolStripMenuItem 新規作成NToolStripMenuItem;
        private ToolStripMenuItem saveToKmlToolStripMenuItem;
        private ToolStripMenuItem saveWindowPosToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem プロジェクトファイルを開くPToolStripMenuItem;
        private ToolStripMenuItem プロジェクトファイルを保存SToolStripMenuItem;
        private Button buttonMark;
        private Mapsui.UI.WindowsForms.MapControl mapControl1;
        private Button buttonSelectToMark;
        private LinkLabel linkLabelMarkPos;
    }
}

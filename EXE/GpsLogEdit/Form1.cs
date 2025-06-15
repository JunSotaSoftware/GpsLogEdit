//
// メイン画面
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

//#define DEBUG_CONSOLE
#define HELP_CONTEXT_NOT_EXIST

using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace GpsLogEdit
{
    /// <summary>
    /// メインフォームのクラス
    /// </summary>
    public partial class Form1 : Form
    {
#if DEBUG_CONSOLE
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
#endif

        private GpsMap gpsMap;
        private EditManager editManager;
        private PositionList positionList;
        private List<string> readFileList;
        private bool modified;
        private List<ListViewItem> listViewItemList;
        private DefaultDataInfo defaultDataInfo;
        private string projectFile;
        private int markPos;

        /// <summary>
        /// メインフォームのコンストラクタ
        /// </summary>
        public Form1()
        {
#if DEBUG_CONSOLE
            AllocConsole();
#endif
            InitializeComponent();
            bool dms = Properties.Settings.Default.Dms;
            dEGToolStripMenuItem.Checked = !dms;
            dMSToolStripMenuItem.Checked = dms;
            buttonSplitGpxFile.Enabled = false;
            buttonDeleteGpxFile.Enabled = false;
            buttonPrevSplitPos.Enabled = false;
            buttonNextSplitPos.Enabled = false;
            labelMoveSplitPos.Enabled = false;
            appendToolStripMenuItem.Enabled = false;
            saveToGpxToolStripMenuItem.Enabled = false;
            saveToKmlToolStripMenuItem.Enabled = false;
            modified = false;
            projectFile = "";
            ClearMarkPos();

#if HELP_CONTEXT_NOT_EXIST
            HelpContextToolStripMenuItem.Enabled = false;
#endif

            positionList = new PositionList();
            editManager = new EditManager();
            readFileList = new List<string>();
            listViewItemList = new List<ListViewItem>();
            defaultDataInfo = new DefaultDataInfo();

            // マップを初期化して表示
            gpsMap = new GpsMap(mapControl1);
            gpsMap.SetEditManager(editManager);

            // マップがクリックされたときのコールバック関数を設定
            gpsMap.SetCallback((index) =>
            {
                if (positionList.GetPositionCount() > 0)
                {
                    SelectGpxLog(index);
                }
            });

            // コマンドラインでファイルが指定されていたら開く
            string[] cmds = Environment.GetCommandLineArgs();
            if (cmds.Length > 1)    // cmds[0] はexeのパス名が入る
            {
                OpenSomething(cmds);
            }
        }

        /// <summary>
        /// フォームのロード時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form1_Load(object sender, EventArgs e)
        {
            // 設定がなかったら古いバージョンの設定を持ってくる
            if (Properties.Settings.Default.F1Size.Width == 0)
            {
                Properties.Settings.Default.Upgrade();
            }

            saveWindowPosToolStripMenuItem.Checked = Properties.Settings.Default.SaveWindowPos;

            if ((Properties.Settings.Default.F1Size.Width != 0) && (Properties.Settings.Default.F1Size.Height != 0))
            {
                // ウインドウの位置を復元
                if (saveWindowPosToolStripMenuItem.Checked)
                {
                    this.Location = Properties.Settings.Default.F1Location;
                }
                // ウインドウのサイズを復元
                this.Size = Properties.Settings.Default.F1Size;
            }
        }

        /// <summary>
        /// コマンドラインで指定された何らかのファイルを開く
        /// </summary>
        /// <param name="cmds">コマンドライン引数</param>
        private void OpenSomething(string[] cmds)
        {
            if (cmds[1].EndsWith(".gedpf", true, null))
            {
                // プロジェクトファイルとして開く
                OpenProjectProcCommon(cmds[1]);
            }
            else
            {
                // GPSデータファイルとして開く
                List<string?> list = cmds.Skip(1).OfType<string?>().ToList();
                ReadProcCommon(list);
            }
        }

        /// <summary>
        /// GPSファイルを開くメニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOpenFile_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                if (CustomMessageBox.Show(this, "データは編集されています。\r\nファイルを開くと全ての編集が取り消されます。\r\nファイルを読み込みますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.No)
                {
                    return;
                }
            }

            FilePicker picker = new FilePicker();
            List<string?> fileList = picker.ShowOpenFileDialog();
            if (fileList.Count > 0)
            {
                positionList.Clear();
                readFileList.Clear();
                listGpxLog.VirtualListSize = 0;
                ReadProcCommon(fileList);
            }
        }

        /// <summary>
        /// GPSファイルを追加で開くメニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAppendFile_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                if (CustomMessageBox.Show(this, "データは編集されています。\r\n追加でファイルを開くと全ての編集が取り消されます。\r\nファイルを追加で読み込みますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.No)
                {
                    return;
                }
            }

            FilePicker picker = new FilePicker();
            List<string?> fileList = picker.ShowOpenFileDialog();
            if (fileList.Count > 0)
            {
                ReadProcCommon(fileList);
            }
        }

        /// <summary>
        /// GPSファイル読み込みの共通処理
        /// </summary>
        /// <param name="fileList">読み込むファイル一覧</param>
        private void ReadProcCommon(List<string?> fileList)
        {
            editManager.Clear();
            foreach (string? file in fileList)
            {
                if (file != null)
                {
                    int fileNumber = readFileList.Count;
                    GpsFileReader reader = new GpsFileReader(file, fileNumber, this);
                    if (reader.Read(positionList))
                    {
                        readFileList.Add(file);
                    }
                }
            }

            FileReadPostProcess();
            modified = false;
            ShowFileListToTitleBar();
            ClearMarkPos();
            if (readFileList.Count > 0)
            {
                appendToolStripMenuItem.Enabled = true;
                saveToGpxToolStripMenuItem.Enabled = true;
                saveToKmlToolStripMenuItem.Enabled = true;
            }
        }

        /// <summary>
        /// GPSファイル読み込みの最終処理
        /// </summary>
        private void FileReadPostProcess()
        {
            if (positionList.GetPositionCount() > 0)
            {
                positionList.Sort();
                SetDataToListView();

                EditProcCommon(positionList.GetReservedDividePointList(), EditType.Divide);
                positionList.ClearReservedDividePoint();

                gpsMap.SetPositionList(positionList);
                gpsMap.ShowGpsTrack();
                gpsMap.ShowGpsDividePoint();
                gpsMap.ShowGpsCurrentPoint(-1);
            }
        }

        /// <summary>
        /// GPSデータをリストビューに登録
        /// </summary>
        private void SetDataToListView()
        {
            if (positionList != null)
            {
                listViewItemList.Clear();
                listGpxLog.VirtualListSize = 0;
                buttonSplitGpxFile.Enabled = false;
                buttonPrevSplitPos.Enabled = false;
                buttonNextSplitPos.Enabled = false;
                labelMoveSplitPos.Enabled = false;

                int positionCount = positionList.GetPositionCount();
                for (int i = 0; i < positionCount; i++)
                {
                    PositionData data = positionList.GetPositionData(i);
                    ListViewItem item = new ListViewItem((i + 1).ToString());
                    if (dMSToolStripMenuItem.Checked)
                    {
                        item.SubItems.Add(DegToDms.Convert(data.latitude, "NS"));
                        item.SubItems.Add(DegToDms.Convert(data.longitude, "EW"));
                    }
                    else
                    {
                        item.SubItems.Add(data.latitude.ToString("F8"));
                        item.SubItems.Add(data.longitude.ToString("F8"));
                    }
                    item.SubItems.Add(data.elevation.ToString("F6"));
                    item.SubItems.Add(data.time.ToString());
                    item.SubItems.Add((data.speed / 0.27777777777777).ToString("F6"));    // m/s --> km/h
                    item.SubItems.Add((data.fileNumber + 1).ToString());
                    item.SubItems.Add("");
                    //item.BackColor = System.Drawing.Color.White;
                    listViewItemList.Add(item);
                }
                // リストビューの全データ更新（一度0にする）
                listGpxLog.VirtualListSize = 0;
                listGpxLog.VirtualListSize = listViewItemList.Count;
            }
        }

        /// <summary>
        /// VirtualモードのListViewが項目を表示するときに呼ばれる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewGpxLog_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = listViewItemList[e.ItemIndex];
        }

        /// <summary>
        /// フォームのタイトルバーにファイル名などを表示
        /// </summary>
        private void ShowFileListToTitleBar()
        {
            string title = "GpsLogEdit";
            string modifiedMark = modified ? "*" : "";
            if (readFileList.Count == 1)
            {
                title = String.Format("{0}{1} - {2}", readFileList[0], modifiedMark, "GpsLogEdit");
            }
            else if (readFileList.Count > 1)
            {
                title = String.Format("{0} (他複数){1} - {2}", readFileList[0], modifiedMark, "GpsLogEdit");
            }
            this.Text = title;
        }

        /// <summary>
        /// 読み込んでいるファイル一覧を表示メニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuShowReadFile_Click(object sender, EventArgs e)
        {
            using (FormShowReadFileList viewer = new FormShowReadFileList())
            {
                viewer.Owner = this;
                viewer.ShowList(readFileList);
            }
        }

        /// <summary>
        /// GPXファイル保存メニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSaveGpxFile_Click(object sender, EventArgs e)
        {
            SaveProcCommon(FileType.Gpx);
        }

        /// <summary>
        /// KMLファイル保存メニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSaveKmlFile_Click(object sender, EventArgs e)
        {
            SaveProcCommon(FileType.Kml);
        }

        /// <summary>
        /// GPSデータ保存の共通処理
        /// </summary>
        /// <param name="type">ファイルタイプ</param>
        private void SaveProcCommon(FileType type)
        {
            if ((positionList != null) && (positionList.GetPositionCount() > 0))
            {
                GpsFileWriter writer = new GpsFileWriter(editManager, positionList, defaultDataInfo, this);

                // 保存の確認を行う
                SaveNotifyInfo info = writer.Notify(type);
                if (info.GetResult() == DialogResult.OK)
                {
                    // ファイル名を入力
                    FilePicker picker = new FilePicker();
                    string file = picker.ShowSaveFileDialog(type, info.GetName());
                    if (file.Length > 0)
                    {
                        // 保存実行
                        if (writer.WriteToFile(file, type, info))
                        {
                            modified = false;
                            ShowFileListToTitleBar();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// DEG座標で表示メニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSelectDeg_Click(object sender, EventArgs e)
        {
            dEGToolStripMenuItem.Checked = true;
            dMSToolStripMenuItem.Checked = false;
            Properties.Settings.Default.Dms = false;
            Properties.Settings.Default.Save();
            UpdateCoordinateSystem(false);
        }

        /// <summary>
        /// DMS座標で表示メニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSelectDms_Click(object sender, EventArgs e)
        {
            dEGToolStripMenuItem.Checked = false;
            dMSToolStripMenuItem.Checked = true;
            Properties.Settings.Default.Dms = true;
            Properties.Settings.Default.Save();
            UpdateCoordinateSystem(true);
        }

        /// <summary>
        /// 座標系を切り替える
        /// </summary>
        /// <param name="dms">true=DMS座標系で表示する</param>
        private void UpdateCoordinateSystem(bool dms)
        {
            if (positionList != null)
            {
                int positionCount = positionList.GetPositionCount();
                for (int i = 0; i < positionCount; i++)
                {
                    PositionData data = positionList.GetPositionData(i);
                    ListViewItem item = listViewItemList[i];
                    if (dms)
                    {
                        item.SubItems[1].Text = DegToDms.Convert(data.latitude, "NS");
                        item.SubItems[2].Text = DegToDms.Convert(data.longitude, "EW");
                    }
                    else
                    {
                        item.SubItems[1].Text = data.latitude.ToString("F8");
                        item.SubItems[2].Text = data.longitude.ToString("F8");
                    }
                }
                listGpxLog.Refresh();
            }
        }

        /// <summary>
        /// Exitメニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExit_Click(object sender, EventArgs e)
        {
            // 終了の確認はMainFormClosingで行う
            // this.Close()でFormClosingイベントが発生する
            this.Close();
        }

        /// <summary>
        /// フォームが閉じられる前に呼ばれる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modified)
            {
                // 編集されているので保存するか確認
                DialogResult result = DialogResult.Cancel;
                using (FormNotifyExit dlg = new FormNotifyExit())
                {
                    dlg.Owner = this;
                    result = dlg.ShowDialog();
                }

                switch (result)
                {
                    case DialogResult.Yes:      // GPXに保存して終了
                        SaveProcCommon(FileType.Gpx);
                        e.Cancel = modified;    // 保存されたらアプリを閉じる
                        break;

                    case DialogResult.Retry:    // KMLに保存して終了
                        SaveProcCommon(FileType.Kml);
                        e.Cancel = modified;    // 保存されたらアプリを閉じる
                        break;

                    case DialogResult.No:       // 保存せずに閉じる
                        e.Cancel = false;
                        break;

                    case DialogResult.Cancel:   // 戻る（閉じない）
                        e.Cancel = true;
                        break;
                }
            }

            if (!e.Cancel)
            {
                // 閉じるときはウインドのサイズと位置を保存
                if (this.WindowState == FormWindowState.Normal)
                {
                    Properties.Settings.Default.F1Location = this.Location;
                    Properties.Settings.Default.F1Size = this.Size;
                }
                else
                {
                    Properties.Settings.Default.F1Location = this.RestoreBounds.Location;
                    Properties.Settings.Default.F1Size = this.RestoreBounds.Size;
                }
                Properties.Settings.Default.SaveWindowPos = saveWindowPosToolStripMenuItem.Checked;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Aboutボタンの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAbout_Click(object sender, EventArgs e)
        {
            using (FormAboutDialog dlg = new FormAboutDialog(this))
            {
                dlg.Owner = this;
                dlg.ShowDialog();
            }
        }

        /// <summary>
        /// Helpメニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHelpContext_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(GpsLogEdit.Properties.Resources.MyWebUrl)
                {
                    UseShellExecute = true      // ShellExecuteを使わないとうまくいかない
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(this, ex.Message, "プロセス起動エラー", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        /// <summary>
        /// 分割/解除ボタンの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSplitGpxFile_Click(object sender, EventArgs e)
        {
            DoEdit(EditType.Divide);
            gpsMap.ShowGpsDividePoint();
            ShowFileListToTitleBar();
            listGpxLog.Select();
        }

        /// <summary>
        /// 削除/解除ボタンの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteGpxFile_Click(object sender, EventArgs e)
        {
            DoEdit(EditType.Delete);
            ShowFileListToTitleBar();
            listGpxLog.Select();
        }

        /// <summary>
        /// 編集を行う
        /// </summary>
        /// <param name="type">行われた編集のタイプ</param>
        private void DoEdit(EditType type)
        {
            if ((positionList != null) && (positionList.GetPositionCount() > 0))
            {
                // ListViewで選ばれている項目からデータ番号のリストを作成
                ListView.SelectedIndexCollection indexes = listGpxLog.SelectedIndices;
                List<int> intIndexes = indexes.Cast<int>().ToList();
                EditProcCommon(intIndexes, type);
            }
        }

        /// <summary>
        /// 編集の共通処理
        /// </summary>
        /// <param name="indexes">編集を行うデータ番号のリスト</param>
        /// <param name="type">編集のタイプ</param>
        private void EditProcCommon(List<int>? indexes, EditType type)
        {
            if (indexes != null)
            {
                foreach (int index in indexes)
                {
                    EditType result = editManager.toggleEditState(index, type);
                    ListViewItem item = listViewItemList[index];
                    if (result == EditType.Divide)
                    {
                        item.BackColor = System.Drawing.Color.Cyan;
                        item.SubItems[7].Text = GpsLogEdit.Properties.Resources.Divided;
                    }
                    else if (result == EditType.Delete)
                    {
                        item.BackColor = System.Drawing.Color.LightGray;
                        item.SubItems[7].Text = "削除";
                    }
                    else if (result == (EditType.Delete | EditType.Divide))
                    {
                        item.BackColor = System.Drawing.Color.LightBlue;
                        item.SubItems[7].Text = "分割＋削除";
                    }
                    else
                    {
                        item.BackColor = System.Drawing.Color.White;
                        item.SubItems[7].Text = "";
                    }
                    modified = true;
                }
                listGpxLog.Refresh();
            }
        }

        /// <summary>
        /// 前の編集位置へ移動ボタンの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPrevSplitPos_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = listGpxLog.SelectedIndices;
            if (indexes.Count > 0)
            {
                int pos = editManager.GetPrevEditedDataNumber(indexes[0]);
                if (pos >= 0)
                {
                    SelectGpxLog(pos);
                }
            }
        }

        /// <summary>
        /// 次の編集位置へ移動ボタンの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNextSplitPos_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = listGpxLog.SelectedIndices;
            if (indexes.Count > 0)
            {
                int pos = editManager.GetNextEditedDataNumber(indexes[0]);
                if (pos >= 0)
                {
                    SelectGpxLog(pos);
                }
            }
        }

        /// <summary>
        /// リストビューの項目が選ばれたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// VirtualモードのListViewでは以下の操作を行ったときにこのメソッドが呼ばれる
        /// ・カーソルを移動したとき
        /// ・マウスクリックで項目を選択したとき
        /// ・CTRL+マウスクリックで項目を複数指定したとき
        /// </remarks>
        private void listViewGpxLog_SelectedIndexChanged(object? sender, EventArgs e)
        {
            SelectChangeCommon();
        }

        /// <summary>
        /// リストビューの項目が範囲選択されたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <remarks>
        /// VirtualモードのListViewでは以下の操作を行ったときにこのメソッドが呼ばれる
        /// ・SHIFTキー＋カーソルキーで項目を範囲選択したとき
        /// ・SHIFTキー＋マウスクリックで項目を範囲選択したとき
        /// </remarks>
        /// <param name="e"></param>
        private void listViewGpxLog_VirtualItemsSelectionRangeChanged(object? sender, ListViewVirtualItemsSelectionRangeChangedEventArgs e)
        {
            SelectChangeCommon();
        }

        /// <summary>
        /// リストビュー項目が選ばれたとkの共通処理
        /// </summary>
        private void SelectChangeCommon()
        {
            if ((positionList != null) && (positionList.GetPositionCount() > 0))
            {
                ListView.SelectedIndexCollection indexes = listGpxLog.SelectedIndices;
                buttonSplitGpxFile.Enabled = (indexes.Count == 1);  // 選択項目が1つだけの時に分割ボタンを有効にする
                buttonDeleteGpxFile.Enabled = (indexes.Count > 0);  // 選択項目が1つ以上の時に削除ボタンを有効にする
                buttonMark.Enabled = (indexes.Count == 1);          // 選択項目が1つだけの時にマークボタンを有効にする
                buttonSelectToMark.Enabled = (indexes.Count == 1) && (markPos != -1) && (markPos != indexes[0]);
                if (indexes.Count > 0)
                {
                    gpsMap.ShowGpsCurrentPoint(indexes[0]);
                    buttonPrevSplitPos.Enabled = (editManager.GetPrevEditedDataNumber(indexes[0]) >= 0);    // 前の編集点があるとき有効
                    buttonNextSplitPos.Enabled = (editManager.GetNextEditedDataNumber(indexes[0]) >= 0);    // 次の編集点があるとき有効
                    labelMoveSplitPos.Enabled = (buttonPrevSplitPos.Enabled || buttonNextSplitPos.Enabled); // 編集点があるときに有効
                }
            }
            else
            {
                buttonSplitGpxFile.Enabled = false;
                buttonDeleteGpxFile.Enabled = false;
                buttonPrevSplitPos.Enabled = false;
                buttonNextSplitPos.Enabled = false;
                labelMoveSplitPos.Enabled = false;
                ClearMarkPos();
            }
        }

        /// <summary>
        /// リストビューの右クリックメニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewGpxLog_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SetListViewContextMenu();
            }
        }

        /// <summary>
        /// リストビューのコンテキストメニューを設定する
        /// </summary>
        private void SetListViewContextMenu()
        {
            ListView.SelectedIndexCollection indexes = listGpxLog.SelectedIndices;
            if (indexes.Count > 0)
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                ToolStripMenuItem item = new ToolStripMenuItem("分割／解除", null, listGpxLogContextMenuStripDivide_Click);
                item.Enabled = (indexes.Count == 1);
                contextMenu.Items.Add(item);
                contextMenu.Items.Add(new ToolStripMenuItem("削除／解除", null, listGpxLogContextMenuStripDelete_Click));
                item = new ToolStripMenuItem("この位置をマーク", null, listGpxLogContextMenuStripMark_Click);
                item.Enabled = (indexes.Count == 1);
                contextMenu.Items.Add(item);
                item = new ToolStripMenuItem("マーク位置まで選択", null, listGpxLogContextMenuStripSelectToMark_Click);
                item.Enabled = (indexes.Count == 1) && (markPos != -1) && (markPos != indexes[0]);
                contextMenu.Items.Add(item);
                listGpxLog.ContextMenuStrip = contextMenu;
                listGpxLog.ContextMenuStrip.Closed += listGpxLogContextMenuStrip_Closed;
            }
        }

        /// <summary>
        /// リストビューのコンテキストメニューがCloseされたあとの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listGpxLogContextMenuStrip_Closed(object? sender, EventArgs e)
        {
            if (listGpxLog.ContextMenuStrip != null)
            {
                listGpxLog.ContextMenuStrip.Items.Clear();
                listGpxLog.ContextMenuStrip = null;
            }
        }

        /// <summary>
        /// リストビューの「分割／解除」コンテキストメニューが選ばれたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listGpxLogContextMenuStripDivide_Click(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                buttonSplitGpxFile_Click(sender, e);
            }
        }

        /// <summary>
        /// リストビューの「削除／解除」コンテキストメニューが選ばれたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listGpxLogContextMenuStripDelete_Click(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                buttonDeleteGpxFile_Click(sender, e);
            }
        }

        /// <summary>
        /// リストビューの「この位置をマーク」コンテキストメニューが選ばれたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listGpxLogContextMenuStripMark_Click(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                buttomMark_Click(sender, e);
            }
        }

        /// <summary>
        /// リストビューの「マーク位置まで選択」コンテキストメニューが選ばれたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listGpxLogContextMenuStripSelectToMark_Click(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                buttonSelectToMark_Click(sender, e);
            }
        }

        /// <summary>
        /// 指定番号のリストビュー項目を選択する
        /// </summary>
        /// <param name="index">項目番号</param>
        private void SelectGpxLog(int index)
        {
            // 選択の全解除のあいだはSelectedChangedのハンドラが呼ばれないようにする
            listGpxLog.SelectedIndexChanged -= listViewGpxLog_SelectedIndexChanged;
            listGpxLog.VirtualItemsSelectionRangeChanged -= listViewGpxLog_VirtualItemsSelectionRangeChanged;

            // ListViewの現在の選択を全て解除する
            ListView.SelectedIndexCollection selectedIndices = listGpxLog.SelectedIndices;
            foreach (int i in selectedIndices)
            {
                listViewItemList[i].Selected = false;
            }

            // 選択の全解除が終わったのでSelectedChangedのハンドラが呼ばれるようにする
            listGpxLog.VirtualItemsSelectionRangeChanged += listViewGpxLog_VirtualItemsSelectionRangeChanged;
            listGpxLog.SelectedIndexChanged += listViewGpxLog_SelectedIndexChanged;

            // 指定された項目を選択
            listGpxLog.EnsureVisible(index);
            listViewItemList[index].Selected = true;
            listViewItemList[index].Focused = true;
            listGpxLog.Select();
        }

        /// <summary>
        /// 新規作成メニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCreateNew_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                if (CustomMessageBox.Show(this, "データは編集されています。\r\n全ての編集を取り消しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.No)
                {
                    return;
                }
            }

            ClearMarkPos();
            modified = false;
            positionList.Clear();
            listGpxLog.VirtualListSize = 0;
            editManager.Clear();
            readFileList.Clear();
            gpsMap.Clear();
            ShowFileListToTitleBar();

            buttonSplitGpxFile.Enabled = false;
            buttonDeleteGpxFile.Enabled = false;
            buttonPrevSplitPos.Enabled = false;
            buttonNextSplitPos.Enabled = false;
            buttonMark.Enabled = false;
            buttonSelectToMark.Enabled = false;
            labelMoveSplitPos.Enabled = false;
            appendToolStripMenuItem.Enabled = false;
            saveToGpxToolStripMenuItem.Enabled = false;
            saveToKmlToolStripMenuItem.Enabled = false;

            projectFile = "";
            defaultDataInfo.Clear();
        }

        /// <summary>
        /// プロジェクトファイルを開くメニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOpenProject_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                if (CustomMessageBox.Show(this, "データは編集されています。\r\nプロジェクトファイルを開くと全ての編集が取り消されます\r\nプロジェクトファイルを読み込みますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.No)
                {
                    return;
                }
            }

            FilePicker picker = new FilePicker();
            string file = picker.ShowOpenProjectDialog();
            OpenProjectProcCommon(file);
        }

        /// <summary>
        /// プロジェクトファイルを開く共通処理
        /// </summary>
        /// <param name="file">ファイル名</param>
        private void OpenProjectProcCommon(string file)
        {
            if (file.Length > 0)
            {
                ProjectManager project = new ProjectManager(this, false);
                if (project.LoadProject(file))
                {
                    ClearMarkPos();
                    positionList.Clear();
                    readFileList.Clear();
                    listGpxLog.VirtualListSize = 0;
                    List<string?>? files = project.GetReadFileList();
                    if ((files != null) && (files.Count > 0))
                    {
                        ReadProcCommon(files);
                        if (positionList.GetPositionCount() != project.GetPoints())
                        {
                            CustomMessageBox.Show(this, "GPSの位置情報の数がプロジェクトを保存したときと異なります。\r\nプロジェクトの読み込みを中断します。", "確認", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                        else
                        {
                            EditProcCommon(project.GetDivide(), EditType.Divide);
                            EditProcCommon(project.GetDelete(), EditType.Delete);
                            gpsMap.ShowGpsDividePoint();
                            List<string>? colorList = project.GetColor();
                            if (colorList != null)
                            {
                                foreach (string color in colorList)
                                {
                                    defaultDataInfo.AddColor(Color.FromArgb(int.Parse(color, NumberStyles.HexNumber)));
                                }
                            }
                            defaultDataInfo.SetSeparate(project.GetSeparate());
                            defaultDataInfo.SetName(project.GetDataName());

                            // プロジェクト保存時のデフォルトファイル名となるように、読み込んだファイル名を記録
                            projectFile = Path.GetFileName(file);
                        }
                        modified = false;
                    }
                }
            }

        }

        /// <summary>
        /// プロジェクトファイルに保存メニューの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSaveProject_Click(object sender, EventArgs e)
        {
            // プロジェクトファイル名がない場合は、データ名をデフォルトのファイル名とする
            if (String.IsNullOrEmpty(projectFile))
            {
                projectFile = defaultDataInfo.GetName();
            }

            FilePicker picker = new FilePicker();
            string file = picker.ShowSaveProjectDialog(projectFile);
            if (file.Length > 0)
            {
                ProjectManager project = new ProjectManager(this, true);
                project.SetReadFileList(readFileList);
                project.SetPoints(positionList.GetPositionCount());
                project.SetDivideAndDelete(editManager);
                project.SetDefaultInfo(defaultDataInfo);
                project.SaveProject(file);
            }
        }

        /// <summary>
        /// この位置をマークボタンの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttomMark_Click(object sender, EventArgs e)
        {
            if ((positionList != null) && (positionList.GetPositionCount() > 0))
            {
                ListView.SelectedIndexCollection indexes = listGpxLog.SelectedIndices;
                if (indexes.Count > 0)
                {
                    ListViewItem item = listViewItemList[indexes[0]];
                    if (markPos != -1)
                    {
                        // 既に付いているマークを解除
                        ListViewItem itemMarked = listViewItemList[markPos];
                        itemMarked.Font = new Font(itemMarked.Font, itemMarked.Font.Style & ~FontStyle.Bold);
                        // 以前のマーク場所と違うなら、新しい場所をマークする
                        if (indexes[0] != markPos)
                        {
                            item.Font = new Font(item.Font, item.Font.Style | FontStyle.Bold);
                            markPos = indexes[0];
                        }
                        else
                        {
                            markPos = -1;
                        }
                    }
                    else
                    {
                        // マークは付いていないので、新しくマークをつける
                        item.Font = new Font(item.Font, item.Font.Style | FontStyle.Bold);
                        markPos = indexes[0];
                    }
                    ShowMarkMessage();
                }
            }
            listGpxLog.Select();
        }

        /// <summary>
        /// マーク位置まで選択ボタンの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectToMark_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = listGpxLog.SelectedIndices;
            int cursor = indexes[0];

            // 選択のあいだはSelectedChangedのハンドラが呼ばれないようにする
            listGpxLog.SelectedIndexChanged -= listViewGpxLog_SelectedIndexChanged;
            listGpxLog.VirtualItemsSelectionRangeChanged -= listViewGpxLog_VirtualItemsSelectionRangeChanged;

            if (cursor < markPos)
            {
                for (int i = cursor; i <= markPos; i++)
                {
                    ListViewItem item = listGpxLog.Items[i];
                    item.Selected = true;

                    // 下の listViewItemList[i].Selected = true;
                    // のやりかただと、広い範囲を選択したとき、選択範囲が途切れ途切れになる
                    // listGpxLog.EnsureVisible(i)
                    // してから Selected=true にするとうまくいくが、恐ろしく遅い
                    // 上の listGpxLog.Items[i] してから item.Selected=true は
                    // うまく動作するし速度も速い
                    // VirtualモードのListViewでも listGpxLog.Item[] は使えるようだ

                    //listGpxLog.EnsureVisible(i);
                    //listViewItemList[i].Focused = true;
                    //listViewItemList[i].Selected = true;
                }
            }
            else
            {
                for (int i = cursor; i >= markPos; i--)
                {
                    ListViewItem item = listGpxLog.Items[i];
                    item.Selected = true;
                }
            }

            // 選択が終わったのでSelectedChangedのハンドラが呼ばれるようにする
            listGpxLog.VirtualItemsSelectionRangeChanged += listViewGpxLog_VirtualItemsSelectionRangeChanged;
            listGpxLog.SelectedIndexChanged += listViewGpxLog_SelectedIndexChanged;

            listGpxLog.Select();
        }

        /// <summary>
        /// マークのメッセージを表示
        /// </summary>
        private void ShowMarkMessage()
        {
            if (markPos == -1)
            {
                linkLabelMarkPos.Text = "マークなし";
                linkLabelMarkPos.LinkArea = new LinkArea(0, 0);
            }
            else
            {
                linkLabelMarkPos.Text = String.Format("マーク位置：{0}", markPos + 1);
                linkLabelMarkPos.LinkArea = new LinkArea(0, linkLabelMarkPos.Text.Length);
            }
        }

        /// <summary>
        /// マークを示すリンクがクリックされたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelMarkPos_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectGpxLog(markPos);
        }

        /// <summary>
        /// マークをクリア
        /// </summary>
        private void ClearMarkPos()
        {
            markPos = -1;
            ShowMarkMessage();
            buttonMark.Enabled = false;
            buttonSelectToMark.Enabled = false;
        }
    }
}

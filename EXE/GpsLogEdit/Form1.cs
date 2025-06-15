//
// ���C�����
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
    /// ���C���t�H�[���̃N���X
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
        /// ���C���t�H�[���̃R���X�g���N�^
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

            // �}�b�v�����������ĕ\��
            gpsMap = new GpsMap(mapControl1);
            gpsMap.SetEditManager(editManager);

            // �}�b�v���N���b�N���ꂽ�Ƃ��̃R�[���o�b�N�֐���ݒ�
            gpsMap.SetCallback((index) =>
            {
                if (positionList.GetPositionCount() > 0)
                {
                    SelectGpxLog(index);
                }
            });

            // �R�}���h���C���Ńt�@�C�����w�肳��Ă�����J��
            string[] cmds = Environment.GetCommandLineArgs();
            if (cmds.Length > 1)    // cmds[0] ��exe�̃p�X��������
            {
                OpenSomething(cmds);
            }
        }

        /// <summary>
        /// �t�H�[���̃��[�h���̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form1_Load(object sender, EventArgs e)
        {
            // �ݒ肪�Ȃ�������Â��o�[�W�����̐ݒ�������Ă���
            if (Properties.Settings.Default.F1Size.Width == 0)
            {
                Properties.Settings.Default.Upgrade();
            }

            saveWindowPosToolStripMenuItem.Checked = Properties.Settings.Default.SaveWindowPos;

            if ((Properties.Settings.Default.F1Size.Width != 0) && (Properties.Settings.Default.F1Size.Height != 0))
            {
                // �E�C���h�E�̈ʒu�𕜌�
                if (saveWindowPosToolStripMenuItem.Checked)
                {
                    this.Location = Properties.Settings.Default.F1Location;
                }
                // �E�C���h�E�̃T�C�Y�𕜌�
                this.Size = Properties.Settings.Default.F1Size;
            }
        }

        /// <summary>
        /// �R�}���h���C���Ŏw�肳�ꂽ���炩�̃t�@�C�����J��
        /// </summary>
        /// <param name="cmds">�R�}���h���C������</param>
        private void OpenSomething(string[] cmds)
        {
            if (cmds[1].EndsWith(".gedpf", true, null))
            {
                // �v���W�F�N�g�t�@�C���Ƃ��ĊJ��
                OpenProjectProcCommon(cmds[1]);
            }
            else
            {
                // GPS�f�[�^�t�@�C���Ƃ��ĊJ��
                List<string?> list = cmds.Skip(1).OfType<string?>().ToList();
                ReadProcCommon(list);
            }
        }

        /// <summary>
        /// GPS�t�@�C�����J�����j���[�̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOpenFile_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                if (CustomMessageBox.Show(this, "�f�[�^�͕ҏW����Ă��܂��B\r\n�t�@�C�����J���ƑS�Ă̕ҏW����������܂��B\r\n�t�@�C����ǂݍ��݂܂����H", "�m�F", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.No)
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
        /// GPS�t�@�C����ǉ��ŊJ�����j���[�̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAppendFile_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                if (CustomMessageBox.Show(this, "�f�[�^�͕ҏW����Ă��܂��B\r\n�ǉ��Ńt�@�C�����J���ƑS�Ă̕ҏW����������܂��B\r\n�t�@�C����ǉ��œǂݍ��݂܂����H", "�m�F", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.No)
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
        /// GPS�t�@�C���ǂݍ��݂̋��ʏ���
        /// </summary>
        /// <param name="fileList">�ǂݍ��ރt�@�C���ꗗ</param>
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
        /// GPS�t�@�C���ǂݍ��݂̍ŏI����
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
        /// GPS�f�[�^�����X�g�r���[�ɓo�^
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
                // ���X�g�r���[�̑S�f�[�^�X�V�i��x0�ɂ���j
                listGpxLog.VirtualListSize = 0;
                listGpxLog.VirtualListSize = listViewItemList.Count;
            }
        }

        /// <summary>
        /// Virtual���[�h��ListView�����ڂ�\������Ƃ��ɌĂ΂��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewGpxLog_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = listViewItemList[e.ItemIndex];
        }

        /// <summary>
        /// �t�H�[���̃^�C�g���o�[�Ƀt�@�C�����Ȃǂ�\��
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
                title = String.Format("{0} (������){1} - {2}", readFileList[0], modifiedMark, "GpsLogEdit");
            }
            this.Text = title;
        }

        /// <summary>
        /// �ǂݍ���ł���t�@�C���ꗗ��\�����j���[�̏���
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
        /// GPX�t�@�C���ۑ����j���[�̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSaveGpxFile_Click(object sender, EventArgs e)
        {
            SaveProcCommon(FileType.Gpx);
        }

        /// <summary>
        /// KML�t�@�C���ۑ����j���[�̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSaveKmlFile_Click(object sender, EventArgs e)
        {
            SaveProcCommon(FileType.Kml);
        }

        /// <summary>
        /// GPS�f�[�^�ۑ��̋��ʏ���
        /// </summary>
        /// <param name="type">�t�@�C���^�C�v</param>
        private void SaveProcCommon(FileType type)
        {
            if ((positionList != null) && (positionList.GetPositionCount() > 0))
            {
                GpsFileWriter writer = new GpsFileWriter(editManager, positionList, defaultDataInfo, this);

                // �ۑ��̊m�F���s��
                SaveNotifyInfo info = writer.Notify(type);
                if (info.GetResult() == DialogResult.OK)
                {
                    // �t�@�C���������
                    FilePicker picker = new FilePicker();
                    string file = picker.ShowSaveFileDialog(type, info.GetName());
                    if (file.Length > 0)
                    {
                        // �ۑ����s
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
        /// DEG���W�ŕ\�����j���[�̏���
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
        /// DMS���W�ŕ\�����j���[�̏���
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
        /// ���W�n��؂�ւ���
        /// </summary>
        /// <param name="dms">true=DMS���W�n�ŕ\������</param>
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
        /// Exit���j���[�̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExit_Click(object sender, EventArgs e)
        {
            // �I���̊m�F��MainFormClosing�ōs��
            // this.Close()��FormClosing�C�x���g����������
            this.Close();
        }

        /// <summary>
        /// �t�H�[����������O�ɌĂ΂��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modified)
            {
                // �ҏW����Ă���̂ŕۑ����邩�m�F
                DialogResult result = DialogResult.Cancel;
                using (FormNotifyExit dlg = new FormNotifyExit())
                {
                    dlg.Owner = this;
                    result = dlg.ShowDialog();
                }

                switch (result)
                {
                    case DialogResult.Yes:      // GPX�ɕۑ����ďI��
                        SaveProcCommon(FileType.Gpx);
                        e.Cancel = modified;    // �ۑ����ꂽ��A�v�������
                        break;

                    case DialogResult.Retry:    // KML�ɕۑ����ďI��
                        SaveProcCommon(FileType.Kml);
                        e.Cancel = modified;    // �ۑ����ꂽ��A�v�������
                        break;

                    case DialogResult.No:       // �ۑ������ɕ���
                        e.Cancel = false;
                        break;

                    case DialogResult.Cancel:   // �߂�i���Ȃ��j
                        e.Cancel = true;
                        break;
                }
            }

            if (!e.Cancel)
            {
                // ����Ƃ��̓E�C���h�̃T�C�Y�ƈʒu��ۑ�
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
        /// About�{�^���̏���
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
        /// Help���j���[�̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHelpContext_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(GpsLogEdit.Properties.Resources.MyWebUrl)
                {
                    UseShellExecute = true      // ShellExecute���g��Ȃ��Ƃ��܂������Ȃ�
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(this, ex.Message, "�v���Z�X�N���G���[", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        /// <summary>
        /// ����/�����{�^���̏���
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
        /// �폜/�����{�^���̏���
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
        /// �ҏW���s��
        /// </summary>
        /// <param name="type">�s��ꂽ�ҏW�̃^�C�v</param>
        private void DoEdit(EditType type)
        {
            if ((positionList != null) && (positionList.GetPositionCount() > 0))
            {
                // ListView�őI�΂�Ă��鍀�ڂ���f�[�^�ԍ��̃��X�g���쐬
                ListView.SelectedIndexCollection indexes = listGpxLog.SelectedIndices;
                List<int> intIndexes = indexes.Cast<int>().ToList();
                EditProcCommon(intIndexes, type);
            }
        }

        /// <summary>
        /// �ҏW�̋��ʏ���
        /// </summary>
        /// <param name="indexes">�ҏW���s���f�[�^�ԍ��̃��X�g</param>
        /// <param name="type">�ҏW�̃^�C�v</param>
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
                        item.SubItems[7].Text = "�폜";
                    }
                    else if (result == (EditType.Delete | EditType.Divide))
                    {
                        item.BackColor = System.Drawing.Color.LightBlue;
                        item.SubItems[7].Text = "�����{�폜";
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
        /// �O�̕ҏW�ʒu�ֈړ��{�^���̏���
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
        /// ���̕ҏW�ʒu�ֈړ��{�^���̏���
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
        /// ���X�g�r���[�̍��ڂ��I�΂ꂽ�Ƃ��̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Virtual���[�h��ListView�ł͈ȉ��̑�����s�����Ƃ��ɂ��̃��\�b�h���Ă΂��
        /// �E�J�[�\�����ړ������Ƃ�
        /// �E�}�E�X�N���b�N�ō��ڂ�I�������Ƃ�
        /// �ECTRL+�}�E�X�N���b�N�ō��ڂ𕡐��w�肵���Ƃ�
        /// </remarks>
        private void listViewGpxLog_SelectedIndexChanged(object? sender, EventArgs e)
        {
            SelectChangeCommon();
        }

        /// <summary>
        /// ���X�g�r���[�̍��ڂ��͈͑I�����ꂽ�Ƃ��̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <remarks>
        /// Virtual���[�h��ListView�ł͈ȉ��̑�����s�����Ƃ��ɂ��̃��\�b�h���Ă΂��
        /// �ESHIFT�L�[�{�J�[�\���L�[�ō��ڂ�͈͑I�������Ƃ�
        /// �ESHIFT�L�[�{�}�E�X�N���b�N�ō��ڂ�͈͑I�������Ƃ�
        /// </remarks>
        /// <param name="e"></param>
        private void listViewGpxLog_VirtualItemsSelectionRangeChanged(object? sender, ListViewVirtualItemsSelectionRangeChangedEventArgs e)
        {
            SelectChangeCommon();
        }

        /// <summary>
        /// ���X�g�r���[���ڂ��I�΂ꂽ��k�̋��ʏ���
        /// </summary>
        private void SelectChangeCommon()
        {
            if ((positionList != null) && (positionList.GetPositionCount() > 0))
            {
                ListView.SelectedIndexCollection indexes = listGpxLog.SelectedIndices;
                buttonSplitGpxFile.Enabled = (indexes.Count == 1);  // �I�����ڂ�1�����̎��ɕ����{�^����L���ɂ���
                buttonDeleteGpxFile.Enabled = (indexes.Count > 0);  // �I�����ڂ�1�ȏ�̎��ɍ폜�{�^����L���ɂ���
                buttonMark.Enabled = (indexes.Count == 1);          // �I�����ڂ�1�����̎��Ƀ}�[�N�{�^����L���ɂ���
                buttonSelectToMark.Enabled = (indexes.Count == 1) && (markPos != -1) && (markPos != indexes[0]);
                if (indexes.Count > 0)
                {
                    gpsMap.ShowGpsCurrentPoint(indexes[0]);
                    buttonPrevSplitPos.Enabled = (editManager.GetPrevEditedDataNumber(indexes[0]) >= 0);    // �O�̕ҏW�_������Ƃ��L��
                    buttonNextSplitPos.Enabled = (editManager.GetNextEditedDataNumber(indexes[0]) >= 0);    // ���̕ҏW�_������Ƃ��L��
                    labelMoveSplitPos.Enabled = (buttonPrevSplitPos.Enabled || buttonNextSplitPos.Enabled); // �ҏW�_������Ƃ��ɗL��
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
        /// ���X�g�r���[�̉E�N���b�N���j���[�̏���
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
        /// ���X�g�r���[�̃R���e�L�X�g���j���[��ݒ肷��
        /// </summary>
        private void SetListViewContextMenu()
        {
            ListView.SelectedIndexCollection indexes = listGpxLog.SelectedIndices;
            if (indexes.Count > 0)
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                ToolStripMenuItem item = new ToolStripMenuItem("�����^����", null, listGpxLogContextMenuStripDivide_Click);
                item.Enabled = (indexes.Count == 1);
                contextMenu.Items.Add(item);
                contextMenu.Items.Add(new ToolStripMenuItem("�폜�^����", null, listGpxLogContextMenuStripDelete_Click));
                item = new ToolStripMenuItem("���̈ʒu���}�[�N", null, listGpxLogContextMenuStripMark_Click);
                item.Enabled = (indexes.Count == 1);
                contextMenu.Items.Add(item);
                item = new ToolStripMenuItem("�}�[�N�ʒu�܂őI��", null, listGpxLogContextMenuStripSelectToMark_Click);
                item.Enabled = (indexes.Count == 1) && (markPos != -1) && (markPos != indexes[0]);
                contextMenu.Items.Add(item);
                listGpxLog.ContextMenuStrip = contextMenu;
                listGpxLog.ContextMenuStrip.Closed += listGpxLogContextMenuStrip_Closed;
            }
        }

        /// <summary>
        /// ���X�g�r���[�̃R���e�L�X�g���j���[��Close���ꂽ���Ƃ̏���
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
        /// ���X�g�r���[�́u�����^�����v�R���e�L�X�g���j���[���I�΂ꂽ�Ƃ��̏���
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
        /// ���X�g�r���[�́u�폜�^�����v�R���e�L�X�g���j���[���I�΂ꂽ�Ƃ��̏���
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
        /// ���X�g�r���[�́u���̈ʒu���}�[�N�v�R���e�L�X�g���j���[���I�΂ꂽ�Ƃ��̏���
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
        /// ���X�g�r���[�́u�}�[�N�ʒu�܂őI���v�R���e�L�X�g���j���[���I�΂ꂽ�Ƃ��̏���
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
        /// �w��ԍ��̃��X�g�r���[���ڂ�I������
        /// </summary>
        /// <param name="index">���ڔԍ�</param>
        private void SelectGpxLog(int index)
        {
            // �I���̑S�����̂�������SelectedChanged�̃n���h�����Ă΂�Ȃ��悤�ɂ���
            listGpxLog.SelectedIndexChanged -= listViewGpxLog_SelectedIndexChanged;
            listGpxLog.VirtualItemsSelectionRangeChanged -= listViewGpxLog_VirtualItemsSelectionRangeChanged;

            // ListView�̌��݂̑I����S�ĉ�������
            ListView.SelectedIndexCollection selectedIndices = listGpxLog.SelectedIndices;
            foreach (int i in selectedIndices)
            {
                listViewItemList[i].Selected = false;
            }

            // �I���̑S�������I������̂�SelectedChanged�̃n���h�����Ă΂��悤�ɂ���
            listGpxLog.VirtualItemsSelectionRangeChanged += listViewGpxLog_VirtualItemsSelectionRangeChanged;
            listGpxLog.SelectedIndexChanged += listViewGpxLog_SelectedIndexChanged;

            // �w�肳�ꂽ���ڂ�I��
            listGpxLog.EnsureVisible(index);
            listViewItemList[index].Selected = true;
            listViewItemList[index].Focused = true;
            listGpxLog.Select();
        }

        /// <summary>
        /// �V�K�쐬���j���[�̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCreateNew_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                if (CustomMessageBox.Show(this, "�f�[�^�͕ҏW����Ă��܂��B\r\n�S�Ă̕ҏW���������܂����H", "�m�F", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.No)
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
        /// �v���W�F�N�g�t�@�C�����J�����j���[�̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOpenProject_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                if (CustomMessageBox.Show(this, "�f�[�^�͕ҏW����Ă��܂��B\r\n�v���W�F�N�g�t�@�C�����J���ƑS�Ă̕ҏW����������܂�\r\n�v���W�F�N�g�t�@�C����ǂݍ��݂܂����H", "�m�F", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.No)
                {
                    return;
                }
            }

            FilePicker picker = new FilePicker();
            string file = picker.ShowOpenProjectDialog();
            OpenProjectProcCommon(file);
        }

        /// <summary>
        /// �v���W�F�N�g�t�@�C�����J�����ʏ���
        /// </summary>
        /// <param name="file">�t�@�C����</param>
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
                            CustomMessageBox.Show(this, "GPS�̈ʒu���̐����v���W�F�N�g��ۑ������Ƃ��ƈقȂ�܂��B\r\n�v���W�F�N�g�̓ǂݍ��݂𒆒f���܂��B", "�m�F", MessageBoxButtons.OK, MessageBoxIcon.None);
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

                            // �v���W�F�N�g�ۑ����̃f�t�H���g�t�@�C�����ƂȂ�悤�ɁA�ǂݍ��񂾃t�@�C�������L�^
                            projectFile = Path.GetFileName(file);
                        }
                        modified = false;
                    }
                }
            }

        }

        /// <summary>
        /// �v���W�F�N�g�t�@�C���ɕۑ����j���[�̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSaveProject_Click(object sender, EventArgs e)
        {
            // �v���W�F�N�g�t�@�C�������Ȃ��ꍇ�́A�f�[�^�����f�t�H���g�̃t�@�C�����Ƃ���
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
        /// ���̈ʒu���}�[�N�{�^���̏���
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
                        // ���ɕt���Ă���}�[�N������
                        ListViewItem itemMarked = listViewItemList[markPos];
                        itemMarked.Font = new Font(itemMarked.Font, itemMarked.Font.Style & ~FontStyle.Bold);
                        // �ȑO�̃}�[�N�ꏊ�ƈႤ�Ȃ�A�V�����ꏊ���}�[�N����
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
                        // �}�[�N�͕t���Ă��Ȃ��̂ŁA�V�����}�[�N������
                        item.Font = new Font(item.Font, item.Font.Style | FontStyle.Bold);
                        markPos = indexes[0];
                    }
                    ShowMarkMessage();
                }
            }
            listGpxLog.Select();
        }

        /// <summary>
        /// �}�[�N�ʒu�܂őI���{�^���̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectToMark_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = listGpxLog.SelectedIndices;
            int cursor = indexes[0];

            // �I���̂�������SelectedChanged�̃n���h�����Ă΂�Ȃ��悤�ɂ���
            listGpxLog.SelectedIndexChanged -= listViewGpxLog_SelectedIndexChanged;
            listGpxLog.VirtualItemsSelectionRangeChanged -= listViewGpxLog_VirtualItemsSelectionRangeChanged;

            if (cursor < markPos)
            {
                for (int i = cursor; i <= markPos; i++)
                {
                    ListViewItem item = listGpxLog.Items[i];
                    item.Selected = true;

                    // ���� listViewItemList[i].Selected = true;
                    // �̂�肩�����ƁA�L���͈͂�I�������Ƃ��A�I��͈͂��r�؂�r�؂�ɂȂ�
                    // listGpxLog.EnsureVisible(i)
                    // ���Ă��� Selected=true �ɂ���Ƃ��܂��������A���낵���x��
                    // ��� listGpxLog.Items[i] ���Ă��� item.Selected=true ��
                    // ���܂����삷�邵���x������
                    // Virtual���[�h��ListView�ł� listGpxLog.Item[] �͎g����悤��

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

            // �I�����I������̂�SelectedChanged�̃n���h�����Ă΂��悤�ɂ���
            listGpxLog.VirtualItemsSelectionRangeChanged += listViewGpxLog_VirtualItemsSelectionRangeChanged;
            listGpxLog.SelectedIndexChanged += listViewGpxLog_SelectedIndexChanged;

            listGpxLog.Select();
        }

        /// <summary>
        /// �}�[�N�̃��b�Z�[�W��\��
        /// </summary>
        private void ShowMarkMessage()
        {
            if (markPos == -1)
            {
                linkLabelMarkPos.Text = "�}�[�N�Ȃ�";
                linkLabelMarkPos.LinkArea = new LinkArea(0, 0);
            }
            else
            {
                linkLabelMarkPos.Text = String.Format("�}�[�N�ʒu�F{0}", markPos + 1);
                linkLabelMarkPos.LinkArea = new LinkArea(0, linkLabelMarkPos.Text.Length);
            }
        }

        /// <summary>
        /// �}�[�N�����������N���N���b�N���ꂽ�Ƃ��̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelMarkPos_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectGpxLog(markPos);
        }

        /// <summary>
        /// �}�[�N���N���A
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

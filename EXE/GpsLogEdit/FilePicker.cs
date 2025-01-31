//
// ファイル選択ダイアログ
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

namespace GpsLogEdit
{
    internal class FilePicker
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FilePicker() { }

        /// <summary>
        /// GPSデータを開くダイアログを表示
        /// </summary>
        /// <returns>開くファイル名のリスト</returns>
        public List<string?> ShowOpenFileDialog()
        {
            List<string?> fileList = new List<string?>();
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = GpsLogEdit.Properties.Resources.GpxFileSelect;
                openFileDialog.Filter = GpsLogEdit.Properties.Resources.GpxFileSelectFilter;
                openFileDialog.Multiselect = true;

                string initialFolder = Properties.Settings.Default.DefaultDirectory;
                if (initialFolder.Length == 0)
                {
                    initialFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                openFileDialog.InitialDirectory = initialFolder;
                openFileDialog.FilterIndex = Properties.Settings.Default.OpenFilterIndex;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveDefaultOpenDirectory(openFileDialog.FileName, openFileDialog.FilterIndex);
                    foreach (string file in openFileDialog.FileNames)
                    {
                        fileList.Add(file);
                    }
                }
            }
            return fileList;
        }

        /// <summary>
        /// 開くダイアログで最初に表示するフォルダ名を保存する
        /// </summary>
        /// <param name="file">開くファイル名</param>
        /// <param name="filterIndex">ダイアログのフィルタのインデックス</param>
        private void SaveDefaultOpenDirectory(string file, int filterIndex)
        {
            string? folder = Path.GetDirectoryName(file);
            if ((folder != null) && (folder.Length > 0))
            {
                Properties.Settings.Default.DefaultDirectory = folder;
            }
            Properties.Settings.Default.OpenFilterIndex = filterIndex;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// GPSデータを保存するダイアログを表示
        /// </summary>
        /// <param name="type">保存するデータのタイプ</param>
        /// <param name="defaultName">ファイル名のデフォルト値</param>
        /// <returns>保存するファイルのファイル名</returns>
        public string ShowSaveFileDialog(FileType type, string defaultName)
        {
            string result = "";
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "名前をつけて保存";
                saveFileDialog.FileName = defaultName;
                switch (type)
                {
                    case FileType.Gpx:
                        saveFileDialog.Filter = "GPXファイル (*.gpx)|*.gpx";
                        break;

                    case FileType.Kml:
                        saveFileDialog.Filter = "KMLファイル (*.kml)|*.kml";
                        break;
                }
                saveFileDialog.AddExtension = true;

                // 次の指定が効果がない
                // saveFileDialog.CheckPathExists = false;

                string initialFolder = Properties.Settings.Default.DefaultDirectory;
                if (initialFolder.Length == 0)
                {
                    initialFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                saveFileDialog.InitialDirectory = initialFolder;
                saveFileDialog.FilterIndex = Properties.Settings.Default.SaveFilterIndex;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveDefaultSaveDirectory(saveFileDialog.FileName, saveFileDialog.FilterIndex);
                    result = saveFileDialog.FileName;
                }
            }
            return result;
        }

        /// <summary>
        /// 保存ダイアログで最初に表示するフォルダ名を保存する
        /// </summary>
        /// <param name="file">保存するファイル名</param>
        /// <param name="filterIndex">ダイアログのフィルタのインデックス</param>
        private void SaveDefaultSaveDirectory(string file, int filterIndex)
        {
            string? folder = Path.GetDirectoryName(file);
            if ((folder != null) && (folder.Length > 0))
            {
                Properties.Settings.Default.DefaultDirectory = folder;
            }
            Properties.Settings.Default.SaveFilterIndex = filterIndex;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// プロジェクトを開くダイアログを表示
        /// </summary>
        /// <returns>開くファイル名</returns>
        public string ShowOpenProjectDialog()
        {
            string result = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "プロジェクトファイルを開く";
                openFileDialog.Filter = "プロジェクトファイル (*.gedpf)|*.gedpf";
                openFileDialog.Multiselect = false;

                string initialFolder = Properties.Settings.Default.DefaultDirectory;
                if (initialFolder.Length == 0)
                {
                    initialFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                openFileDialog.InitialDirectory = initialFolder;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveDefaultProjectDirectory(openFileDialog.FileName);
                    result = openFileDialog.FileName;
                }
            }
            return result;
        }

        /// <summary>
        /// プロジェクトを保存するダイアログを表示
        /// </summary>
        /// <param name="projectFile">デフォルトのファイル名</param>
        /// <returns>保存するファイル名</returns>
        public string ShowSaveProjectDialog(string projectFile)
        {
            string result = "";
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "プロジェクトファイルに保存";
                saveFileDialog.FileName = projectFile;
                saveFileDialog.Filter = "プロジェクトファイル (*.gedpf)|*.gedpf";
                saveFileDialog.AddExtension = true;

                // 次の指定が効果がない
                // saveFileDialog.CheckPathExists = false;

                string initialFolder = Properties.Settings.Default.DefaultDirectory;
                if (initialFolder.Length == 0)
                {
                    initialFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                saveFileDialog.InitialDirectory = initialFolder;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveDefaultProjectDirectory(saveFileDialog.FileName);
                    result = saveFileDialog.FileName;
                }
            }
            return result;
        }

        /// <summary>
        /// プロジェクトを開く/保存ダイアログで最初に表示するフォルダ名を保存する
        /// </summary>
        /// <param name="file">ファイル名</param>
        private void SaveDefaultProjectDirectory(string file)
        {
            string? folder = Path.GetDirectoryName(file);
            if ((folder != null) && (folder.Length > 0))
            {
                Properties.Settings.Default.DefaultDirectory = folder;
            }
            Properties.Settings.Default.Save();
        }

    }
}

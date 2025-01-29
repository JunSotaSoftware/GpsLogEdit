using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GpsLogEdit
{
    internal class FilePicker
    {
        public FilePicker() { }

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

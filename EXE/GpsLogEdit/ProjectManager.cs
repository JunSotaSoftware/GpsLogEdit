//
// プロジェクトファイルのマネージャ
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

using System.Xml;
using System.Xml.Serialization;

namespace GpsLogEdit
{
    /// <summary>
    /// プロジェクトファイルの構造(XML)を示したクラス 
    /// </summary>

    [XmlRoot("project")]
    public class ProjectXmlData
    {
        [XmlElement("name")]
        public XmlCDataSection? Name { get; set; }

        [XmlElement("separate")]
        public bool Separate { get; set; }

        [XmlElement("points")]
        public int Points { get; set; }

        [XmlArray("file")]
        [XmlArrayItem("read")]
        public List<XmlCDataSection>? Read { get; set; }

        [XmlArray("divide")]
        [XmlArrayItem("line")]
        public List<int>? Divide { get; set; }

        [XmlArray("delete")]
        [XmlArrayItem("line")]
        public List<int>? Delete { get; set; }

        [XmlArray("color")]
        [XmlArrayItem("argb")]
        public List<string>? Color { get; set; }
    }

    /// <summary>
    /// プロジェクトファイルマネージャクラス
    /// </summary>
    internal class ProjectManager
    {
        private Form ownerForm;
        private ProjectXmlData project;
        private ProjectXmlData deserializedProject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="owner">エラーダイアログを表示するときのオーナーフォーム</param>
        /// <param name="save">true=保存の処理を行う</param>
#pragma warning disable CS8618
        public ProjectManager(Form owner, bool save) 
        {
            ownerForm = owner;
            if (save)
            {
                project = new ProjectXmlData();
            }
        }

        /// <summary>
        /// プロジェクトをファイルに保存する
        /// </summary>
        /// <param name="projectFileName">プロジェクトファイル名</param>
        /// <returns>true=保存成功</returns>
        public bool SaveProject(string projectFileName)
        {
            bool success = false;
            try
            {
                using (var writer = new StreamWriter(projectFileName, false))
                {
                    var serializer = new XmlSerializer(typeof(ProjectXmlData));
                    serializer.Serialize(writer, project);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ownerForm, ex.Message, "プロジェクトファイル書き込みエラー", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            return success;
        }

        /// <summary>
        /// 読み込んだファイルのリストを受け取る（保存実行前に行う）
        /// </summary>
        /// <param name="fileList">読み込んだファイルのリスト</param>
        public void SetReadFileList(List<string> fileList)
        {
            project.Read = new List<XmlCDataSection>();
            foreach (string filename in fileList)
            {
                project.Read.Add(new XmlDocument().CreateCDataSection(filename));
            }
        }

        /// <summary>
        /// GPSのデータ数を受け取る（保存実行前に行う）
        /// </summary>
        /// <param name="points">データ数</param>
        public void SetPoints(int points)
        {
            project.Points = points;
        }

        /// <summary>
        /// 編集情報を受け取る（保存実行前に行う）
        /// </summary>
        /// <param name="editManager">編集マネージャ</param>
        public void SetDivideAndDelete(EditManager editManager)
        {
            project.Divide = new List<int>();
            int editCount = editManager.GetEditPositionCount(EditType.Divide);
            for (int i = 0; i < editCount; i++)
            {
                int number = editManager.GetEditedDataNumber(i, EditType.Divide);
                project.Divide.Add(number);
            }

            project.Delete = new List<int>();
            editCount = editManager.GetEditPositionCount(EditType.Delete);
            for (int i = 0; i < editCount; i++)
            {
                int number = editManager.GetEditedDataNumber(i, EditType.Delete);
                project.Delete.Add(number);
            }
        }

        /// <summary>
        /// GPSデータに書き込む情報（データ名、トラックの城指定など）を受け取る（保存実行前に行う）
        /// </summary>
        /// <param name="defaultInfo">情報</param>
        public void SetDefaultInfo(DefaultDataInfo defaultInfo)
        {
            project.Name = new XmlDocument().CreateCDataSection(defaultInfo.GetName());
            project.Separate = defaultInfo.GetSeparate();
            project.Color = new List<string>();
            for (int i = 0; i < defaultInfo.GetColorCount(); i++)
            {
                Color color = defaultInfo.GetColor(i);
                project.Color.Add(color.ToArgb().ToString("X8"));
            }
        }

        /// <summary>
        /// プロジェクトファイルを読み込む
        /// </summary>
        /// <param name="projectFileName">プロジェクトファイル名</param>
        /// <returns>true=読み込み成功</returns>
        public bool LoadProject(string projectFileName)
        {
            bool success = false;
            try
            {
                using (var reader = new StreamReader(projectFileName, true))
                {
                    var serializer = new XmlSerializer(typeof(ProjectXmlData));
                    ProjectXmlData? tmp = (ProjectXmlData?)serializer.Deserialize(reader);
                    if (tmp != null)
                    {
                        deserializedProject = tmp;
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ownerForm, ex.Message, "プロジェクトファイル読み込みエラー", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            return success;
        }

        /// <summary>
        /// プロジェクトファイルに記録してあったファイルリストを返す
        /// </summary>
        /// <returns>ファイルリスト</returns>
        public List<string?>? GetReadFileList()
        {
            if (deserializedProject.Read == null)
            {
                return null;
            }
            return deserializedProject.Read.Select(cdata => cdata.Value).ToList();
        }

        /// <summary>
        /// プロジェクトファイルに記録してあったGPSデータ数を返す
        /// </summary>
        /// <returns>データ数</returns>
        public int GetPoints()
        {
            return deserializedProject.Points;
        }

        /// <summary>
        /// プロジェクトファイルに記録してあった分割位置情報リストを返す
        /// </summary>
        /// <returns>分割位置情報リスト</returns>
        public List<int>? GetDivide()
        {
            return deserializedProject.Divide;
        }

        /// <summary>
        /// プロジェクトファイルに記録してあった削除位置情報リストを返す
        /// </summary>
        /// <returns>削除位置情報リスト</returns>
        public List<int>? GetDelete()
        {
            return deserializedProject.Delete;
        }

        /// <summary>
        /// プロジェクトファイルに記録してあった色リストを返す
        /// </summary>
        /// <returns>色リスト（ARGBの文字列）</returns>
        public List<string>? GetColor()
        {
            return deserializedProject.Color;
        }

        /// <summary>
        /// プロジェクトファイルに記録してあったファイル分割フラグを返す
        /// </summary>
        /// <returns>true=トラック毎にファイルを分割する</returns>
        public bool GetSeparate()
        {
            return deserializedProject.Separate;
        }

        /// <summary>
        /// プロジェクトファイルに記録してあったデータ名を帰す
        /// </summary>
        /// <returns>データ名</returns>
        public string? GetDataName()
        {
            if (deserializedProject.Name == null)
            {
                return null;
            }
            return deserializedProject.Name.Data;
        }
    }
}

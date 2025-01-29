using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GpsLogEdit
{

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



    internal class ProjectManager
    {
        private Form ownerForm;
        private ProjectXmlData project;
        private ProjectXmlData deserializedProject;

#pragma warning disable CS8618
        public ProjectManager(Form owner, bool save) 
        {
            ownerForm = owner;
            if (save)
            {
                project = new ProjectXmlData();
            }
        }

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

        public void SetReadFileList(List<string> fileList)
        {
            project.Read = new List<XmlCDataSection>();
            foreach (string filename in fileList)
            {
                project.Read.Add(new XmlDocument().CreateCDataSection(filename));
            }
        }

        public void SetPoints(int points)
        {
            project.Points = points;
        }

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

        public List<string?>? GetReadFileList()
        {
            if (deserializedProject.Read == null)
            {
                return null;
            }
            return deserializedProject.Read.Select(cdata => cdata.Value).ToList();
        }

        public int GetPoints()
        {
            return deserializedProject.Points;
        }

        public List<int>? GetDivide()
        {
            return deserializedProject.Divide;
        }

        public List<int>? GetDelete()
        {
            return deserializedProject.Delete;
        }

        public List<string>? GetColor()
        {
            return deserializedProject.Color;
        }

        public bool GetSeparate()
        {
            return deserializedProject.Separate;
        }

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

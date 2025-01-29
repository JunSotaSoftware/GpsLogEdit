using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpsLogEdit
{




    public class SaveNotifyOneTrack
    {
        private DateTime startTime;
        private DateTime lastTime;

        public SaveNotifyOneTrack(DateTime start, DateTime last)
        {
            startTime = start;
            lastTime = last;
        }

        public DateTime GetStartTime()
        {
            return startTime;
        }

        public DateTime GetLastTime()
        {
            return lastTime;
        }
    }



    public class SaveNotifyInfo
    {
        private List<SaveNotifyOneTrack> trackList;
        private DialogResult result;
        private FileType fileType;
        private DefaultDataInfo defaultInfo;

        public SaveNotifyInfo(DefaultDataInfo defaultInfo)
        {
            trackList = new List<SaveNotifyOneTrack>();
            this.defaultInfo = defaultInfo;
        }

        public void AddTrack(DateTime start, DateTime last)
        {
            SaveNotifyOneTrack info = new SaveNotifyOneTrack(start, last);
            trackList.Add(info);
        }

        public void SetName(string name)
        {
            defaultInfo.SetName(name);
        }

        public string GetName()
        {
            return defaultInfo.GetName();
        }

        public void SetColor(int index, Color color)
        {
            defaultInfo.SetColor(index, color);
        }

        public Color GetColor(int index)
        {
            return defaultInfo.GetColor(index);
        }

        public void SetSeparate(bool separate)
        {
            defaultInfo.SetSeparate(separate);
        }

        public bool GetSeparate()
        {
            return defaultInfo.GetSeparate();
        }

        public void SetResult(DialogResult result)
        {
            this.result = result;
        }

        public DialogResult GetResult()
        {
            return result;
        }

        public SaveNotifyOneTrack GetOneTrack(int index)
        {
            return trackList[index];
        }

        public int GetTrackCount()
        {
            return trackList.Count;
        }

        public void SetFileType(FileType type)
        {
            fileType = type;
        }

        public FileType GetFileType()
        {
            return fileType;
        }
    }





}

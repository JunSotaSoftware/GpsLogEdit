using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpsLogEdit
{


    /// <summary>
    /// 位置情報データ
    /// </summary>
    internal class PositionData : IComparable<PositionData>
    {
        public double latitude;
        public double longitude;
        public double elevation;
        public DateTime time;
        public double speed;
        public int fileNumber;

        /// <summary>
        /// 位置情報データのコンストラクタ1
        /// </summary>
        public PositionData()
        {
            this.latitude = 0;
            this.longitude = 0;
            this.elevation = 0;
            this.speed = 0;
            this.fileNumber = 0;
        }

        /// <summary>
        /// 位置情報データのコンストラクタ2
        /// </summary>
        /// <param name="top">GPXファイル上での先頭行番号</param>
        /// <param name="last">GPXファイル上での最終行番号</param>
        /// <param name="latitude">緯度</param>
        /// <param name="longitude">経度</param>
        /// <param name="elevation">高度</param>
        /// <param name="time">時刻</param>
        public PositionData(double latitude, double longitude, double elevation, DateTime time, double speed, int fileNumber)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.elevation = elevation;
            this.time = time;
            this.speed = speed;
            this.fileNumber = fileNumber;
        }

        public int CompareTo(PositionData? data)
        {
            if (data == null)
            {
                return 1;
            }
            return time.CompareTo(data.time);
        }
    }



    internal class PositionList
    {
        private List<PositionData> positionList;
        private List<DateTime> reservedDividePoint;

        public PositionList()
        {
            positionList = new List<PositionData>();
            reservedDividePoint = new List<DateTime>();
        }




        /// <summary>
        /// 読み込んだファイルを解析する
        /// </summary>
        /// <returns>true=解析成功</returns>
        public bool Analize(FileType type, int fileNumber, string[]? gpxLines)
        {
            bool success = false;
            switch (type)
            {
                case FileType.Gpx:
                    GpxFileAnalyzer gpxAnalyzer = new GpxFileAnalyzer();
                    success = gpxAnalyzer.Analyze(gpxLines, fileNumber, this);
                    break;

                case FileType.Nmea:
                    NmeaFileAnalyzer nmeaAnalyzer = new NmeaFileAnalyzer();
                    success = nmeaAnalyzer.Analyze(gpxLines, fileNumber, this);
                    break;
            }
            return success;
        }


        public void Add(PositionData data)
        {
            positionList.Add(data);
        }


        /// <summary>
        /// 位置情報の数を返す
        /// </summary>
        /// <returns>位置情報の数</returns>
        public int GetPositionCount()
        {
            return positionList.Count;
        }

        /// <summary>
        /// 指定番号の位置情報を返す
        /// </summary>
        /// <param name="index">データ番号</param>
        /// <returns>位置情報宇</returns>
        public PositionData GetPositionData(int index)
        {
            PositionData data = positionList[index];
            return data;
        }

        public void Clear()
        {
            positionList.Clear();
        }

        public void Sort()
        {
            positionList.Sort();
        }

        public void ReserveDivideHere(DateTime dateTime)
        {
            reservedDividePoint.Add(dateTime);
        }

        public List<int> GetReservedDividePointList()
        {
            List<int> list = new List<int>();
            foreach (var dateTime in reservedDividePoint)
            {
                int index = positionList.FindIndex(x => x.time == dateTime);
                list.Add(index);
            }
            return list;
        }

        public void ClearReservedDividePoint()
        {
            reservedDividePoint.Clear();
        }


    }
}

//
// 位置情報リスト
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

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
        /// <param name="latitude">緯度</param>
        /// <param name="longitude">経度</param>
        /// <param name="elevation">高度</param>
        /// <param name="time">時刻</param>
        /// <param name="speed">速度</param>
        /// <param name="fileNumber">ファイル番号</param>
        public PositionData(double latitude, double longitude, double elevation, DateTime time, double speed, int fileNumber)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.elevation = elevation;
            this.time = time;
            this.speed = speed;
            this.fileNumber = fileNumber;
        }

        /// <summary>
        /// コンパレータ（時刻で比較）
        /// </summary>
        /// <param name="data">比較対象のデータ</param>
        /// <returns>比較結果</returns>
        public int CompareTo(PositionData? data)
        {
            if (data == null)
            {
                return 1;
            }
            return time.CompareTo(data.time);
        }
    }

    /// <summary>
    /// 位置情報リスト
    /// </summary>
    internal class PositionList
    {
        private List<PositionData> positionList;        // 位置情報のリスト
        private List<DateTime> reservedDividePoint;     // GPXファイル読み込みの時にトラックの分割があったときに、分割すべき位置

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PositionList()
        {
            positionList = new List<PositionData>();
            reservedDividePoint = new List<DateTime>();
        }

        /// <summary>
        /// 読み込んだファイル(GPX/NMEA)を解析する
        /// </summary>
        /// <param name="type">ファイルのタイプ</param>
        /// <param name="fileNumber">ファイル番号</param>
        /// <param name="gpxLines">読み込んだファイルのデータ</param>
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

        /// <summary>
        /// 位置情報をリストに追加
        /// </summary>
        /// <param name="data">位置情報</param>
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
        /// <returns>位置情報</returns>
        public PositionData GetPositionData(int index)
        {
            PositionData data = positionList[index];
            return data;
        }

        /// <summary>
        /// 位置情報リストをクリア
        /// </summary>
        public void Clear()
        {
            positionList.Clear();
        }

        /// <summary>
        /// 位置情報リストを時刻でソート
        /// </summary>
        public void Sort()
        {
            positionList.Sort();
        }

        /// <summary>
        /// GPXファイル読み込み時にこの時刻で分割する指示を残す
        /// </summary>
        /// <param name="dateTime">時刻</param>
        public void ReserveDivideHere(DateTime dateTime)
        {
            reservedDividePoint.Add(dateTime);
        }

        /// <summary>
        /// GPXファイル読み込み時の分割位置指定を、データ番号のリストにして返す
        /// </summary>
        /// <returns>データ番号リスト</returns>
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

        /// <summary>
        /// GPXファイル読み込み時の分割位置指定リストをクリア
        /// </summary>
        public void ClearReservedDividePoint()
        {
            reservedDividePoint.Clear();
        }

    }
}

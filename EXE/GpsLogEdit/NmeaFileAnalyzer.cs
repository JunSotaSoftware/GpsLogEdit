//
// NMEAファイル解析
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

using System.Text.RegularExpressions;

namespace GpsLogEdit
{
    /// <summary>
    /// NMEAファイル解析クラス
    /// </summary>
    internal class NmeaFileAnalyzer
    {
        private string[] positionData;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NmeaFileAnalyzer()
        {
            positionData = new string[9];
        }

        /// <summary>
        /// ファイル全体を解析する
        /// </summary>
        /// <param name="gpxLines">読み込んだファイルの内容（行単位で分割）</param>
        /// <param name="fileNumber">ファイル番号</param>
        /// <param name="list">データリスト（これに追加していく）</param>
        /// <returns>true=解析成功</returns>
        public bool Analyze(string[]? gpxLines, int fileNumber, PositionList list)
        {
            bool success = false;

            if (gpxLines != null)
            {
                ClearPositionData();
                for (int i = 0; i < gpxLines.Length; i++)
                {
                    string[] column = gpxLines[i].Split(',');
                    if (StringArrayMatch(column, 0, @"^(\$GPRMC|\$GLRMC|\$GARMC|\$GBRMC|\$GQRMC|\$GNRMC|\$BDRMC)$"))    // $GPRMC ?
                    {
                        if (StringArrayMatch(column, 2, @"A"))     // データ有効？
                        {
                            string time = StringArrayGet(column, 1);
                            if (AddPositionIfComplete(list, time, fileNumber))
                            {
                                success = true;                                
                            }
                            positionData[(int)DataId.Time] = time;
                            positionData[(int)DataId.Latitude] = StringArrayGet(column, 3);
                            positionData[(int)DataId.NS] = StringArrayGet(column, 4);
                            positionData[(int)DataId.Longitude] = StringArrayGet(column, 5);
                            positionData[(int)DataId.EW] = StringArrayGet(column, 6);
                            positionData[(int)DataId.Speed] = StringArrayGet(column, 7);
                            positionData[(int)DataId.Date] = StringArrayGet(column, 9);
                        }
                    }
                    else if (StringArrayMatch(column, 0, @"^(\$GPGGA|\$GLGGA|\$GAGGA|\$GBGGA|\$GQGGA|\$GNGGA|\$BDGGA)$")) // $GPGGA
                    {
                        if (StringArrayNotMatch(column, 6, @"0"))    // データ有効？
                        {
                            string time = StringArrayGet(column, 1);
                            if (AddPositionIfComplete(list, time, fileNumber))
                            {
                                success = true;
                            }
                            positionData[(int)DataId.Time] = time;
                            positionData[(int)DataId.Latitude] = StringArrayGet(column, 2);
                            positionData[(int)DataId.NS] = StringArrayGet(column, 3);
                            positionData[(int)DataId.Longitude] = StringArrayGet(column, 4);
                            positionData[(int)DataId.EW] = StringArrayGet(column, 5);
                            positionData[(int)DataId.Elevation] = StringArrayGet(column, 9);
                            positionData[(int)DataId.ElevationUnit] = StringArrayGet(column, 10);
                        }
                    }
                }

                if (AddPositionIfComplete(list, "", fileNumber))
                {
                    success = true;
                }
            }
            return success;
        }

        /// <summary>
        /// GPS1データ分の情報が揃ったら登録
        /// </summary>
        /// <param name="list">データリスト（これに追加していく）</param>
        /// <param name="time">$GPGGA もしくは $GPRMC に記録されている時刻</param>
        /// <remarks>
        /// NMEAの情報は $GPGGA と ＄GPRMC の両方を読まないとデータが揃わない。
        /// 例えば、時刻の情報は両方にあるが、日付のデータは $GPRMC のほうにしかないなど。
        /// そこで、$GPGGA もしくは $GPRMC のどちらか先に現れた方の時刻を time に入れておき、
        /// 後から現れた方の時刻と比較し、一致すればその2つを1つの位置情報として扱っている。
        /// 時刻が違うときは、別の位置情報となる。
        /// </remarks>
        /// <param name="fileNumber">ファイル番号</param>
        /// <returns>true=登録した</returns>
        private bool AddPositionIfComplete(PositionList list, string time, int fileNumber)
        {
            bool success = false;

            if ((positionData[(int)DataId.Date].Length > 0) &&
                (positionData[(int)DataId.Time].Length > 0) &&
                (!positionData[(int)DataId.Time].Equals(time)))
            {
                string tmp = positionData[(int)DataId.Date] + positionData[(int)DataId.Time] + "Z"; // ZはUTC
                DateTime dateTime = new DateTime();
                try
                {
                    dateTime = DateTime.ParseExact(tmp, "ddMMyyHHmmss.FFK", null);
                }
                catch (Exception) { }
                double dLatitude = DmmToDeg.Convert(positionData[(int)DataId.Latitude], positionData[(int)DataId.NS]);
                double dLongitude = DmmToDeg.Convert(positionData[(int)DataId.Longitude], positionData[(int)DataId.EW]);
                double dSpeed = 0;
                try
                {
                    dSpeed = double.Parse(positionData[(int)DataId.Speed]) * 0.5144;     // knot --> m/s
                }
                catch (Exception) { }
                double dElevation = 0;
                try
                {
                    dElevation = double.Parse(positionData[(int)DataId.Elevation]);
                }
                catch (Exception) { }

                PositionData data = new PositionData(dLatitude, dLongitude, dElevation, dateTime, dSpeed, fileNumber);
                list.Add(data);
                ClearPositionData();
                success = true;
            }
            return success;
        }

        /// <summary>
        /// 読み込みデータをクリア
        /// </summary>
        private void ClearPositionData()
        {
            positionData[(int)DataId.Latitude] = "";
            positionData[(int)DataId.Longitude] = "";
            positionData[(int)DataId.Elevation] = "";
            positionData[(int)DataId.Time] = "";
            positionData[(int)DataId.Speed] = "";
            positionData[(int)DataId.Date] = "";
            positionData[(int)DataId.NS] = "";
            positionData[(int)DataId.EW] = "";
            positionData[(int)DataId.ElevationUnit] = "";
        }

        /// <summary>
        /// 文字列がマッチするかチェック
        /// </summary>
        /// <param name="array">CSVの列ごとのデータアレイ</param>
        /// <param name="column">どの列と比較するか</param>
        /// <param name="match">正規表現の文字列</param>
        /// <returns>true=マッチした</returns>
        private bool StringArrayMatch(string[] array, int column, string match)
        {
            bool status = ((array.Length > column) && Regex.IsMatch(array[column], match));
            return status;
        }
        /// <summary>
        /// 文字列がマッチしないかチェック
        /// </summary>
        /// <param name="array">CSVの列ごとのデータアレイ</param>
        /// <param name="column">どの列と比較するか</param>
        /// <param name="match">正規表現の文字列</param>
        /// <returns>true=マッチしなかった</returns>
        private bool StringArrayNotMatch(string[] array, int column, string match)
        {
            bool status = ((array.Length > column) && !Regex.IsMatch(array[column], match));
            return status;
        }

        /// <summary>
        /// CSVの列のデータを取得
        /// </summary>
        /// <param name="array">CSVの列ごとのデータアレイ</param>
        /// <param name="column">列番号</param>
        /// <returns>列の文字列</returns>
        private string StringArrayGet(string[] array, int column)
        {
            string ret = "";
            if (array.Length > column)
            {
                ret = array[column];
            }
            return ret;
        }

    }
}

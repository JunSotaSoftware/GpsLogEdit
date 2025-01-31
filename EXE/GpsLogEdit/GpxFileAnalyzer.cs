//
// GPXファイル解析
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

using System.Text.RegularExpressions;

namespace GpsLogEdit
{
    /// <summary>
    /// GPXファイル解析クラス
    /// </summary>
    internal class GpxFileAnalyzer
    {
        private string[] positionData;  // 1データ分の情報（緯度、経度、高度、時刻、速度）
        private bool nowInTrkpt;        // 現在 <trackpt></trackpt> 内であるか
        private bool nowInExtensions;   // 現在 <extensions></extensions> 内であるか
        private int trksegNumber;       // いくつ目の <trkseg> か
        private bool divideHere;        // ここでこのデータで分割する指示

        private string[] expression =
        {
            "lat=\"(-?[0-9.]+)\"",
            "lon=\"(-?[0-9.]+)\"",
            "<ele>(-?[0-9.]+)</ele>",
            "<time>([0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9.]{2,}Z?[\\-+:0-9]*)</time>",
            "<speed>([0-9.]+)</speed>"
        };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GpxFileAnalyzer()
        {
            nowInTrkpt = false;
            nowInExtensions = false;
            trksegNumber = -1;
            divideHere = false;
            positionData = new string[expression.Length];
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
                for (int i = 0; i < gpxLines.Length; i++)
                {
                    if (AnalizeOneLine(gpxLines[i]))
                    {
                        string latitude = positionData[(int)DataId.Latitude];
                        string longitude = positionData[(int)DataId.Longitude];
                        string elevation = positionData[(int)DataId.Elevation];
                        string time = positionData[(int)DataId.Time];
                        string speed = positionData[(int)DataId.Speed];
                        try
                        {
                            double dLatitude = double.Parse(latitude);
                            double dLongitude = double.Parse(longitude);
                            double dElevation = 0;
                            DateTime dateTime = new DateTime(0);
                            double dSpeed = 0;
                            if (elevation.Length > 0)
                            {
                                dElevation = double.Parse(elevation);
                            }
                            if (time.Length > 0)
                            {
                                dateTime = DateTime.Parse(time);
                            }
                            if (speed.Length > 0)
                            {
                                dSpeed = double.Parse(speed);
                            }
                            
                            PositionData data = new PositionData(dLatitude, dLongitude, dElevation, dateTime, dSpeed, fileNumber);
                            list.Add(data);
                            if (divideHere)
                            {
                                list.ReserveDivideHere(dateTime);
                                divideHere = false;
                            }
                            success = true;
                        }
                        catch (Exception) { }
                    }
                }
            }
            return success;
        }

        /// <summary>
        /// GPXファイルの1行分のデータを解析
        /// </summary>
        /// <param name="line">1行分のデータ</param>
        /// <returns>true=GPS1データ分の解析が終わった</returns>
        public bool AnalizeOneLine(string line)
        {
            bool ready = false;

            if (!nowInTrkpt)
            {
                if (line.IndexOf("<trkseg>") >= 0)
                {
                    if (trksegNumber >= 0)
                    {
                        divideHere = true;  // 複数のトラックに分割されている
                    }
                    trksegNumber++;
                }

                if ((line.IndexOf("<trkpt") >= 0) || (line.IndexOf("<wpt") >= 0))
                {
                    nowInTrkpt = true;
                    positionData[(int)DataId.Latitude] = "";
                    positionData[(int)DataId.Longitude] = "";
                    positionData[(int)DataId.Elevation] = "";
                    positionData[(int)DataId.Time] = "";
                    positionData[(int)DataId.Speed] = "";
                }
            }

            if (nowInTrkpt)
            {
                if (line.IndexOf("<extensions>") >= 0)
                {
                    nowInExtensions = true;
                }
                if (line.IndexOf("</extensions>") >= 0)
                {
                    nowInExtensions = false;
                }

                if (!nowInExtensions)
                {
                    for (int i = 0; i < expression.Length; i++)
                    {
                        try
                        {
                            if (Regex.IsMatch(line, expression[i]))
                            {
                                Match match = Regex.Match(line, expression[i]);
                                positionData[i] = match.Groups[1].Value;
                            }
                        }
                        catch (Exception) { }
                    }

                    if ((line.IndexOf("</trkpt>") >= 0) || (line.IndexOf("</wpt>") >= 0))
                    {
                        nowInTrkpt = false;
                        ready = true;
                    }
                }
            }
            return ready;
        }
    }
}

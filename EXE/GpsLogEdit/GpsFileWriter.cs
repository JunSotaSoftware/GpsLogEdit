//
// GPSデータファイルライター
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

namespace GpsLogEdit
{
    /// <summary>
    /// トラックデータクラス
    /// </summary>
    internal class TrackData
    {
        public int startDataNumber;
        public int lastDataNumber;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="start">トラックの開始データ番号</param>
        /// <param name="last">トラックの末尾データ番号</param>
        public TrackData(int start, int last)
        {
            startDataNumber = start;
            lastDataNumber = last;
        }
    }


    /// <summary>
    /// GPSファイルライタークラス
    /// </summary>
    internal class GpsFileWriter
    {
        private EditManager manager;
        private Form ownerForm;
        private PositionList positionList;
        private List<TrackData> trackList;
        private DefaultDataInfo defaultDataInfo;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="manager">編集マネージャ</param>
        /// <param name="list">GPSデータリスト</param>
        /// <param name="info">ファイルに書き込むデータ名などの情報</param>
        /// <param name="owner">エラーダイアログを表示するときのオーナーフォーム</param>
        public GpsFileWriter(EditManager manager, PositionList list, DefaultDataInfo info, Form owner)
        {
            this.manager = manager;
            this.ownerForm = owner;
            positionList = list;
            defaultDataInfo = info;
            trackList = new List<TrackData>();
        }

        /// <summary>
        /// 書き込み前の確認画面を表示
        /// </summary>
        /// <param name="type">ファイルタイプ</param>
        /// <returns>確認情報</returns>
        public SaveNotifyInfo Notify(FileType type)
        {
            SaveNotifyInfo info = MakeDivideInformation();
            using (FormSaveNotifyDialogEx notify = new FormSaveNotifyDialogEx())
            {
                notify.Owner = ownerForm;
                string msg = "以下のように分割してGPXファイルを保存します";
                if (type == FileType.Kml)
                {
                    msg = "以下のように分割してKMLファイルを保存します";
                }
                info.SetFileType(type);
                info = notify.ShowDialog(info, msg);
            }
            return info;
        }

        /// <summary>
        /// GPSファイルの分割情報を作成する
        /// </summary>
        /// <returns>確認情報</returns>
        private SaveNotifyInfo MakeDivideInformation()
        {
            int divisionCount = manager.GetEditPositionCount(EditType.Divide);
            int start = 0;
            int next = 0;
            TrackData trackData;

            // 分割位置に従って各ファイル毎のデータ範囲を作成
            for (int i = 0; i < divisionCount; i++)
            {
                next = manager.GetEditedDataNumber(i, EditType.Divide);  // 分割位置を取得
                if (next > 0)   // 先頭データの分割は無視
                {
                    trackData = new TrackData(start, next - 1);
                    trackList.Add(trackData);
                    start = next;
                }
            }
            // 残りのデータ範囲を作成
            next = positionList.GetPositionCount();
            trackData = new TrackData(start, next - 1);
            trackList.Add(trackData);

            SaveNotifyInfo info = new SaveNotifyInfo(defaultDataInfo);
            foreach (TrackData p in trackList)
            {
                PositionData startData = positionList.GetPositionData(p.startDataNumber);
                PositionData lastData = positionList.GetPositionData(p.lastDataNumber);
                info.AddTrack(startData.time, lastData.time);
            }
            return info;
        }

        /// <summary>
        /// ファイルへの書き込み
        /// </summary>
        /// <param name="pathName">ファイル名</param>
        /// <param name="type">ファイルタイプ</param>
        /// <param name="info">確認情報</param>
        /// <returns>true=書き込み成功</returns>
        public bool WriteToFile(string pathName, FileType type, SaveNotifyInfo info)
        {
            bool status = false;
            switch (type)
            {
                case FileType.Gpx:
                    if (info.GetSeparate())
                    {
                        status = WriteToSeparateGpxFile(pathName, info);
                    }
                    else
                    {
                        status = WriteToOneGpxFile(pathName, info);
                    }
                    break;

                case FileType.Kml:
                    if (info.GetSeparate())
                    {
                        status = WriteToSeparateKmlFile(pathName, info);
                    }
                    else
                    {
                        status = WriteToOneKmlFile(pathName, info);
                    }
                    break;
            }
            return status;
        }

        /// <summary>
        /// GPXファイルに書き込む（トラック毎に別ファイル）
        /// </summary>
        /// <param name="pathName">ファイル名（ファイル名の末尾に part? と入る）</param>
        /// <param name="info">確認情報</param>
        /// <returns>true=書き込み成功</returns>
        private bool WriteToSeparateGpxFile(string pathName, SaveNotifyInfo info)
        {
            bool status = false;
            string savedNames = "";
            int trackMax = trackList.Count;
            try
            {
                string dirName = Path.GetDirectoryName(pathName) ?? "";
                string fileName = Path.GetFileNameWithoutExtension(pathName);
                string extName = Path.GetExtension(pathName) ?? "";
                bool endSeparator = Path.EndsInDirectorySeparator(dirName);
                for (int track = 0; track < trackMax; track++)
                {
                    string path = String.Format("{0}{1}{2}(track{3}){4}", dirName, endSeparator ? "" : "\\", fileName, track + 1, extName);
                    using StreamWriter writer = new StreamWriter(path);

                    // GPXファイルのヘッダ部分を書き込む
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    writer.WriteLine("<gpx version=\"v1.0\" creator=\"GpsLogEdit windows application\">");
                    writer.WriteLine("<trk>");
                    writer.WriteLine("<name><![CDATA[{0} (track{1})]]></name>", info.GetName(), (track+1).ToString());
                    writer.WriteLine("<trkseg>");

                    // GPXファイルの本体を書き込む
                    for (int i = trackList[track].startDataNumber; i <= trackList[track].lastDataNumber; i++)
                    {
                        if (!manager.IsEditedLine(i, EditType.Delete))  // 削除されていなければ書き込む
                        {
                            PositionData data = positionList.GetPositionData(i);
                            string line = String.Format("<trkpt lat=\"{0}\" lon=\"{1}\"><ele>{2}</ele><time>{3}Z</time><speed>{4}</speed></trkpt>",
                                data.latitude.ToString("F8"),
                                data.longitude.ToString("F8"),
                                data.elevation.ToString("F6"),
                                data.time.ToUniversalTime().ToString("s"),
                                data.speed.ToString("F6"));
                            writer.WriteLine(line);
                        }
                    }

                    // GPXファイルのフッター部分を書き込む
                    writer.WriteLine("</trkseg>");
                    writer.WriteLine("</trk>");
                    writer.WriteLine("</gpx>");

                    writer.Close();
                    savedNames += String.Format("{0}\n", path);
                }
                CustomMessageBox.Show(ownerForm, "保存しました\n" + savedNames, "GPXファイルの保存", MessageBoxButtons.OK, MessageBoxIcon.None);
                status = true;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(ownerForm, e.Message, "書き込みエラー", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            return status;
        }

        /// <summary>
        /// GPXファイルへ書き込む（1つのファイル内でトラックを分ける）
        /// </summary>
        /// <param name="pathName">ファイル名</param>
        /// <param name="info">確認情報</param>
        /// <returns>true=書き込み成功</returns>
        private bool WriteToOneGpxFile(string pathName, SaveNotifyInfo info)
        {
            bool status = false;
            string savedNames = "";
            int trackMax = trackList.Count;
            try
            {
                using StreamWriter writer = new StreamWriter(pathName);
                for (int track = 0; track < trackMax; track++)
                {
                    // GPXファイルのヘッダ部分を書き込む
                    if (track == 0)
                    {
                        writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        writer.WriteLine("<gpx version=\"v1.0\" creator=\"GpsLogEdit windows application\">");
                    }
                    writer.WriteLine("<trk>");
                    writer.WriteLine("<name><![CDATA[{0}]]></name>", info.GetName());
                    writer.WriteLine("<trkseg>");

                    // GPXファイルの本体を書き込む
                    for (int i = trackList[track].startDataNumber; i <= trackList[track].lastDataNumber; i++)
                    {
                        if (!manager.IsEditedLine(i, EditType.Delete))  // 削除されていなければ書き込む
                        {
                            PositionData data = positionList.GetPositionData(i);
                            string line = String.Format("<trkpt lat=\"{0}\" lon=\"{1}\"><ele>{2}</ele><time>{3}Z</time><speed>{4}</speed></trkpt>",
                                data.latitude.ToString("F8"),
                                data.longitude.ToString("F8"),
                                data.elevation.ToString("F6"),
                                data.time.ToUniversalTime().ToString("s"),
                                data.speed.ToString("F6"));
                            writer.WriteLine(line);
                        }
                    }

                    // GPXファイルのフッター部分を書き込む
                    writer.WriteLine("</trkseg>");
                    writer.WriteLine("</trk>");
                    if (track == trackMax-1)
                    {
                        writer.WriteLine("</gpx>");
                    }
                }
                writer.Close();
                savedNames += String.Format("{0}\n", pathName);
                CustomMessageBox.Show(ownerForm, "保存しました\n" + savedNames, "GPXファイルの保存", MessageBoxButtons.OK, MessageBoxIcon.None);
                status = true;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(ownerForm, e.Message, "書き込みエラー", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            return status;
        }

        /// <summary>
        /// KMLファイルに書き込む（トラック毎に別ファイル）
        /// </summary>
        /// <param name="pathName">ファイル名（ファイル名の末尾に part? と入る）</param>
        /// <param name="info">確認情報</param>
        /// <returns>true=書き込み成功</returns>
        private bool WriteToSeparateKmlFile(string pathName, SaveNotifyInfo info)
        {
            bool status = false;
            string savedNames = "";
            int trackMax = trackList.Count;
            try
            {
                string dirName = Path.GetDirectoryName(pathName) ?? "";
                string fileName = Path.GetFileNameWithoutExtension(pathName);
                string extName = Path.GetExtension(pathName) ?? "";
                bool endSeparator = Path.EndsInDirectorySeparator(dirName);
                for (int track = 0; track < trackMax; track++)
                {
                    string path = String.Format("{0}{1}{2}(track{3}){4}", dirName, endSeparator ? "" : "\\", fileName, track + 1, extName);
                    using StreamWriter writer = new StreamWriter(path);

                    // KMLファイルのヘッダ部分を書き込む
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    writer.WriteLine("<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
                    writer.WriteLine("<Placemark>");
                    writer.WriteLine("<name><![CDATA[{0} (track{1})]]></name>", info.GetName(), (track+1).ToString());
                    writer.WriteLine("<description>GpsLogEdit</description>");
                    writer.WriteLine("<Style>");
                    writer.WriteLine("<LineStyle>");
                    Color color = info.GetColor(track);
                    writer.WriteLine("<color>FF{0:X2}{1:X2}{2:X2}</color>", color.B, color.G, color.R);
                    writer.WriteLine("<width>3</width>");
                    writer.WriteLine("</LineStyle>");
                    writer.WriteLine("</Style>");
                    writer.WriteLine("<LineString>");
                    writer.WriteLine("<coordinates>");

                    // KMLファイルの本体を書き込む
                    for (int i = trackList[track].startDataNumber; i <= trackList[track].lastDataNumber; i++)
                    {
                        if (!manager.IsEditedLine(i, EditType.Delete))  // 削除されていなければ書き込む
                        {
                            PositionData data = positionList.GetPositionData(i);
                            string line = String.Format("{0},{1},{2}",
                                data.longitude.ToString("F8"),
                                data.latitude.ToString("F8"),
                                data.elevation.ToString("F6"));
                            writer.WriteLine(line);
                        }
                    }

                    // KMLファイルのフッター部分を書き込む
                    writer.WriteLine("</coordinates>");
                    writer.WriteLine("</LineString>");
                    writer.WriteLine("</Placemark>");
                    writer.WriteLine("</kml>");

                    writer.Close();
                    savedNames += String.Format("{0}\n", path);
                }
                CustomMessageBox.Show(ownerForm, "保存しました\n" + savedNames, "KMLファイルの保存", MessageBoxButtons.OK, MessageBoxIcon.None);
                status = true;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(ownerForm, e.Message, "書き込みエラー", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            return status;
        }

        /// <summary>
        /// KMLファイルに書き込む（1つのファイル内でトラックを分割）
        /// </summary>
        /// <param name="pathName">ファイル名</param>
        /// <param name="info">確認情報</param>
        /// <returns>true=書き込み成功</returns>
        private bool WriteToOneKmlFile(string pathName, SaveNotifyInfo info)
        {
            bool status = false;
            string savedNames = "";
            int trackMax = trackList.Count;
            try
            {
                using StreamWriter writer = new StreamWriter(pathName);
                for (int track = 0; track < trackMax; track++)
                {
                    // KMLファイルのヘッダ部分を書き込む
                    if (track == 0)
                    {
                        writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        writer.WriteLine("<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
                        writer.WriteLine("<Document>");
                        writer.WriteLine("<description><![CDATA[{0}]]></description>", info.GetName());
                    }
                    writer.WriteLine("<Placemark>");
                    writer.WriteLine("<name><![CDATA[{0} (track{1})]]></name>", info.GetName(), (track+1).ToString());
                    writer.WriteLine("<description>GpsLogEdit</description>");
                    writer.WriteLine("<Style>");
                    writer.WriteLine("<LineStyle>");
                    Color color = info.GetColor(track);
                    writer.WriteLine("<color>FF{0:X2}{1:X2}{2:X2}</color>", color.B, color.G, color.R);
                    writer.WriteLine("<width>3</width>");
                    writer.WriteLine("</LineStyle>");
                    writer.WriteLine("</Style>");
                    writer.WriteLine("<LineString>");
                    writer.WriteLine("<coordinates>");

                    // KMLファイルの本体を書き込む
                    for (int i = trackList[track].startDataNumber; i <= trackList[track].lastDataNumber; i++)
                    {
                        if (!manager.IsEditedLine(i, EditType.Delete))  // 削除されていなければ書き込む
                        {
                            PositionData data = positionList.GetPositionData(i);
                            string line = String.Format("{0},{1},{2}",
                                data.longitude.ToString("F8"),
                                data.latitude.ToString("F8"),
                                data.elevation.ToString("F6"));
                            writer.WriteLine(line);
                        }
                    }

                    // KMLファイルのフッター部分を書き込む
                    writer.WriteLine("</coordinates>");
                    writer.WriteLine("</LineString>");
                    writer.WriteLine("</Placemark>");
                    if (track == trackMax-1)
                    {
                        writer.WriteLine("</Document>");
                        writer.WriteLine("</kml>");
                    }
                }
                writer.Close();
                savedNames += String.Format("{0}\n", pathName);
                CustomMessageBox.Show(ownerForm, "保存しました\n" + savedNames, "KMLファイルの保存", MessageBoxButtons.OK, MessageBoxIcon.None);
                status = true;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(ownerForm, e.Message, "書き込みエラー", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            return status;
        }


    }
}

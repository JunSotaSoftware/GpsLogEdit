//
// GPX/NMEAファイル書き込み確認のための情報作成
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

namespace GpsLogEdit
{
    /// <summary>
    /// 1トラック分のデータクラス
    /// </summary>
    public class SaveNotifyOneTrack
    {
        private DateTime startTime;
        private DateTime lastTime;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="start">トラックの開始時刻</param>
        /// <param name="last">トラックの末尾時刻</param>
        public SaveNotifyOneTrack(DateTime start, DateTime last)
        {
            startTime = start;
            lastTime = last;
        }

        /// <summary>
        /// トラックの開始時刻を返す
        /// </summary>
        /// <returns>開始時刻</returns>
        public DateTime GetStartTime()
        {
            return startTime;
        }

        /// <summary>
        /// トラックの末尾時刻を返す
        /// </summary>
        /// <returns>末尾時刻</returns>
        public DateTime GetLastTime()
        {
            return lastTime;
        }
    }

    /// <summary>
    /// 保存確認データクラス
    /// </summary>
    public class SaveNotifyInfo
    {
        private List<SaveNotifyOneTrack> trackList;
        private DialogResult result;
        private FileType fileType;
        private DefaultDataInfo defaultInfo;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="defaultInfo">確認情報</param>
        public SaveNotifyInfo(DefaultDataInfo defaultInfo)
        {
            trackList = new List<SaveNotifyOneTrack>();
            this.defaultInfo = defaultInfo;
        }

        /// <summary>
        /// トラックを追加
        /// </summary>
        /// <param name="start">トラックの開始時刻</param>
        /// <param name="last">トラックの末尾時刻</param>
        public void AddTrack(DateTime start, DateTime last)
        {
            SaveNotifyOneTrack info = new SaveNotifyOneTrack(start, last);
            trackList.Add(info);
        }

        /// <summary>
        /// データ名を指定
        /// </summary>
        /// <param name="name">データ名</param>
        public void SetName(string name)
        {
            defaultInfo.SetName(name);
        }

        /// <summary>
        /// データ名を帰す
        /// </summary>
        /// <returns>データ名</returns>
        public string GetName()
        {
            return defaultInfo.GetName();
        }

        /// <summary>
        /// トラック色を指定
        /// </summary>
        /// <param name="index">トラック番号</param>
        /// <param name="color">色</param>
        public void SetColor(int index, Color color)
        {
            defaultInfo.SetColor(index, color);
        }

        /// <summary>
        /// トラック色を返す
        /// </summary>
        /// <param name="index">トラック番号</param>
        /// <returns>色</returns>
        public Color GetColor(int index)
        {
            return defaultInfo.GetColor(index);
        }

        /// <summary>
        /// トラック毎にファイルを分割するかを指定
        /// </summary>
        /// <param name="separate">true=ファイルを分割する</param>
        public void SetSeparate(bool separate)
        {
            defaultInfo.SetSeparate(separate);
        }

        /// <summary>
        /// トラック毎にファイルを分割するかを返す
        /// </summary>
        /// <returns>true=ファイルを分割する</returns>
        public bool GetSeparate()
        {
            return defaultInfo.GetSeparate();
        }

        /// <summary>
        /// 確認ダイアログの戻り値を指定する
        /// </summary>
        /// <param name="result">戻り値</param>
        public void SetResult(DialogResult result)
        {
            this.result = result;
        }

        /// <summary>
        /// 確認ダイアログの戻り値を返す
        /// </summary>
        /// <returns>戻り値</returns>
        public DialogResult GetResult()
        {
            return result;
        }

        /// <summary>
        /// 1トラックの情報を返す
        /// </summary>
        /// <param name="index">トラック番号</param>
        /// <returns>トラックの情報</returns>
        public SaveNotifyOneTrack GetOneTrack(int index)
        {
            return trackList[index];
        }

        /// <summary>
        /// トラックの数を返す
        /// </summary>
        /// <returns>トラックの数</returns>
        public int GetTrackCount()
        {
            return trackList.Count;
        }

        /// <summary>
        /// ファイルタイプを指定する
        /// </summary>
        /// <param name="type">ファイルタイプ</param>
        public void SetFileType(FileType type)
        {
            fileType = type;
        }

        /// <summary>
        /// ファイルタイプを返す
        /// </summary>
        /// <returns>ファイルタイプ</returns>
        public FileType GetFileType()
        {
            return fileType;
        }
    }


}

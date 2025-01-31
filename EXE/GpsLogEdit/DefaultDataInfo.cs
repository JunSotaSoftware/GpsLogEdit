//
// GPX/KMLファイルに書き込むデータの管理
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

namespace GpsLogEdit
{
    /// <summary>
    /// GPX/KMLに書き込むデータ管理クラス
    /// </summary>
    public class DefaultDataInfo
    {
        private string dataName;
        private bool separateFile;
        private List<Color> colorList;
        private Color[] defaultColor = { Color.Red, Color.Blue, Color.Green, Color.HotPink, Color.Cyan };

        /// <summary>
        /// インストラクタ
        /// </summary>
        public DefaultDataInfo()
        {
            dataName = "";
            separateFile = Properties.Settings.Default.SaveSeparateFile;
            colorList = new List<Color>();
        }

        /// <summary>
        /// データ名をセット
        /// </summary>
        /// <param name="name">データ名</param>
        public void SetName(string? name)
        {
            if (name != null)
            {
                dataName = name;
            }
        }

        /// <summary>
        /// データ名を返す
        /// </summary>
        /// <returns>データ名</returns>
        public string GetName()
        {
            return dataName;
        }

        /// <summary>
        /// 分割されたトラックを複数ファイルに分けるかどうかをセット
        /// </summary>
        /// <param name="separate">true=複数ファイルに分ける</param>
        public void SetSeparate(bool separate)
        {
            separateFile = separate;
            Properties.Settings.Default.SaveSeparateFile = separate;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 分割されたトラックを複数ファイルに分けるかどうかを返す
        /// </summary>
        /// <returns>true=複数ファイルに分ける</returns>
        public bool GetSeparate()
        {
            return separateFile;
        }

        /// <summary>
        /// トラックの表示色を追加
        /// </summary>
        /// <param name="color">色</param>
        public void AddColor(Color color)
        {
            colorList.Add(color);
        }

        /// <summary>
        /// 指定番号のトラックの表示色をセット
        /// </summary>
        /// <param name="index">トラック番号</param>
        /// <param name="color">色</param>
        public void SetColor(int index, Color color)
        {
            if (index < colorList.Count)
            {
                colorList[index] = color;   // 色リストに登録済みのインデックスなら書き換え
            }
            else
            {
                for (int i = colorList.Count; i < index; i++)   // 指定インデックス-1まで色リストに登録
                {
                    AddColor(GetColor(i));
                }
                AddColor(color);    // 指定インデックスに色を登録
            }
        }

        /// <summary>
        /// 指定番号のトラック表示色を返す
        /// </summary>
        /// <param name="index">番号</param>
        /// <returns>色</returns>
        public Color GetColor(int index)
        {
            Color color;
            if (index < colorList.Count)
            {
                color = colorList[index];   // 色リストにあればその色を
            }
            else if (index < defaultColor.Length)
            {
                color = defaultColor[index];    // 色リストにない -> デフォルト色にあればその色を
            }
            else
            {
                index = index % defaultColor.Length;    // デフォルト色を循環して色を選ぶ
                color = defaultColor[index];
            }
            return color;
        }

        /// <summary>
        /// 色が指定されているトラックの数を返す
        /// </summary>
        /// <returns>数</returns>
        public int GetColorCount()
        {
            return colorList.Count;
        }

        /// <summary>
        /// GPX/KMLに書き込むデータをクリア
        /// </summary>
        public void Clear()
        {
            dataName = "";
            colorList.Clear();
        }
    }
}

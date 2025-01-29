using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpsLogEdit
{
    public class DefaultDataInfo
    {
        private string dataName;
        private bool separateFile;
        private List<Color> colorList;
        private Color[] defaultColor = { Color.Red, Color.Blue, Color.Green, Color.HotPink, Color.Cyan };

        public DefaultDataInfo()
        {
            dataName = "";
            separateFile = Properties.Settings.Default.SaveSeparateFile;
            colorList = new List<Color>();
        }

        public void SetName(string? name)
        {
            if (name != null)
            {
                dataName = name;
            }
        }

        public string GetName()
        {
            return dataName;
        }

        public void SetSeparate(bool separate)
        {
            separateFile = separate;
            Properties.Settings.Default.SaveSeparateFile = separate;
            Properties.Settings.Default.Save();
        }

        public bool GetSeparate()
        {
            return separateFile;
        }

        public void AddColor(Color color)
        {
            colorList.Add(color);
        }

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

        public int GetColorCount()
        {
            return colorList.Count;
        }

        public void Clear()
        {
            dataName = "";
            colorList.Clear();
        }
    }
}

//
// DEG座標をDMS座標に変換
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

namespace GpsLogEdit
{
    /// <summary>
    /// DMM座標->DEG座標変換クラス
    /// </summary>
    internal class DmmToDeg
    {
        /// <summary>
        /// DMM座標をDEG座標に変換する
        /// </summary>
        /// <param name="dmm">DMM座標</param>
        /// <param name="nsew">E/W/N/Sを示す文字</param>
        /// <returns>DEG座標</returns>
        public static double Convert(string dmm, string nsew)
        {
            int start = 2;
            if (dmm.IndexOf('.') == 5)
            {
                start = 3;
            }
            double deg = 0;
            try
            {
                string d = dmm.Substring(0, start);
                string m1 = dmm.Substring(start, 2);
                string m2 = dmm.Substring(dmm.IndexOf('.') + 1);
                deg = double.Parse(d) + double.Parse(m1) / 60 + double.Parse("0." + m2) / 60;
            }
            catch (Exception) { }
            if (nsew.Equals("S") || nsew.Equals("W"))
            {
                deg = -deg;
            }
            return deg;
        }
    }
}

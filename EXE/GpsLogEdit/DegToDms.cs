//
// DEG座標をDMS座標に変換
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

namespace GpsLogEdit
{
    /// <summary>
    /// DEG座標->DMS座標変換クラス
    /// </summary>
    internal class DegToDms
    {
        /// <summary>
        /// DEG座標をDMS座標に変換
        /// </summary>
        /// <param name="deg">DEG座標</param>
        /// <param name="ew">EWまたはNSの文字列</param>
        /// <returns>DMS座標系の文字列</returns>
        public static string Convert(double deg, string ew)
        {
            int ewSel = 0;
            if (deg < 0)
            {
                ewSel = 1;
                deg = Math.Abs(deg);
            }
            int degrees = (int)Math.Floor(deg);
            deg = (deg - degrees) * 60;
            int minutes = (int)Math.Floor(deg);
            deg = (deg - minutes) * 60;
            return String.Format("{0}°{1}'{2}\"{3}", degrees, minutes, deg.ToString("F2"), ew.Substring(ewSel, 1));
        }

    }
}

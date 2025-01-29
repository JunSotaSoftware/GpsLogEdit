namespace GpsLogEdit
{
    internal class DegToDms
    {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpsLogEdit
{
    internal class DmmToDeg
    {
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

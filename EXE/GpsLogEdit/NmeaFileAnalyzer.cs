using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GpsLogEdit
{
    internal class NmeaFileAnalyzer
    {
        private string[] positionData;

        public NmeaFileAnalyzer()
        {
            positionData = new string[9];
        }


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

        private bool StringArrayMatch(string[] array, int column, string match)
        {
            bool status = ((array.Length > column) && Regex.IsMatch(array[column], match));
            return status;
        }

        private bool StringArrayNotMatch(string[] array, int column, string match)
        {
            bool status = ((array.Length > column) && !Regex.IsMatch(array[column], match));
            return status;
        }

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

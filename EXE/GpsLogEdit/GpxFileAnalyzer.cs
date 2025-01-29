using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GpsLogEdit
{
    internal class GpxFileAnalyzer
    {
        private string[] positionData;
        private bool nowInTrkpt;
        private bool nowInExtensions;
        private int trksegNumber;
        private bool divideHere;

        private string[] expression =
        {
            "lat=\"(-?[0-9.]+)\"",
            "lon=\"(-?[0-9.]+)\"",
            "<ele>(-?[0-9.]+)</ele>",
            "<time>([0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9.]{2,}Z?[\\-+:0-9]*)</time>",
            "<speed>([0-9.]+)</speed>"
        };

        public GpxFileAnalyzer()
        {
            nowInTrkpt = false;
            nowInExtensions = false;
            trksegNumber = -1;
            divideHere = false;
            positionData = new string[expression.Length];
        }



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

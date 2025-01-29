using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpsLogEdit
{
    public enum DataId
    {
        Latitude,
        Longitude,
        Elevation,
        Time,
        Speed,
        Date,
        NS,
        EW,
        ElevationUnit
    }


    public enum FileType
    {
        Gpx,
        Nmea,
        Kml
    }

    public enum EditType
    {
        None    = 0b_0000_0000,
        Divide  = 0b_0000_0001,
        Delete  = 0b_0000_0010,
        All     = 0b_1111_1111
    }

}

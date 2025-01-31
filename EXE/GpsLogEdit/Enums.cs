//
// 各種ENUM
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

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

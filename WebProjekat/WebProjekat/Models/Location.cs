﻿namespace WebProjekat.Models
{
    public class Location
    {
        // latitude
        public double X { get; set; }

        // longitude
        public double Y { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string AreaCode { get; set; }

        public Location()
        {
            X = 0;
            Y = 0;
            StreetNumber = 0;
        }

    }

    public struct Address
    {
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string AreaCode { get; set; }
    }
}
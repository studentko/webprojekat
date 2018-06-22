using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTO
{
    public class CreateRideDTO
    {
        public Location Location { get; set; }
        public CarType CarType { get; set; }

        public int DriverId { get; set; }
    }
}
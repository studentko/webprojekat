using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTO
{
    public class RideChangeDTO
    {

    }

    public class CustomerRideChangeDTO : RideChangeDTO
    {
        public Location Location { get; set; }
        public CarType? CarType { get; set; }
    }

    public class DispatcherRideChangeDTO : RideChangeDTO
    {
        public int AssignDriverId { get; set; }
    }

    public class DriverRideChangeDTO : RideChangeDTO
    {
        public RideStatus NewStatus { get; set; }


        // potrabno ako je status kompletiran
        public Location DestinationLocation { get; set; }

        public int Amount { get; set; }
    }
}
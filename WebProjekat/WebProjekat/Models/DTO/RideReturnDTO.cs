using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTO
{
    public class RideReturnDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Ride Ride { get; set; }
    }
}
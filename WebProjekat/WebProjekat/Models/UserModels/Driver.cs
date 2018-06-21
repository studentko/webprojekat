using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class Driver: User
    {
        public int CarId { get; set; }
        public virtual Location Location{ get; set; }
        public virtual List<Ride> Rides { get; set; }

        public virtual Car Car { get; set; }

        public Driver()
        {
            this.Role = Role.Driver;
        }
    }
}
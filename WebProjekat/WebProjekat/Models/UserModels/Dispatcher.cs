using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class Dispatcher: User
    {
        public virtual List<Ride> Rides { get; set; }
        public Dispatcher()
        {
            this.Role = Role.Dispatcher;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class Customer: User
    {
        public virtual List<Ride> Rides { get; set; }
        public Customer()
        {
            this.Role = Role.Customer;
        }
    }
}
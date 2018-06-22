using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTO
{
    public class GetRidesFilterDTO
    {
        public bool OnlyAssigned { get; set; }

        public RideStatus? StatusFilter { get; set; }

        public DateTime? FromOrderDate { get; set; }
        public DateTime? ToOrderDate { get; set; }

        public int? FromRate { get; set; }
        public int? ToRate { get; set; }

        public int? FromPrice { get; set; }
        public int? ToPrice { get; set; }

        public RideFilterSort SortFilter { get; set; }
    }

    public class GetDispatcherRidesFilterDTO : GetRidesFilterDTO
    {
        public string CustomerName { get; set; }
        public string CustomerLastName { get; set; }

        public string DriverName { get; set; }
        public string DriverLastName { get; set; }
    }

    public enum RideFilterSort
    {
        NoSort = 0,
        ByDate,
        ByRate,
    }
    
}
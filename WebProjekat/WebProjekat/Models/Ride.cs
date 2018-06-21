using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProjekat.Models
{
    public class Ride
    {
        [Key]
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? DriverId { get; set; }
        public int? DispatcherId { get; set; }
        public DateTime Date { get; set; }
        public virtual Location StartLocation { get; set; }
        public virtual Location Destination { get; set; }
        public int Amount { get; set; }
        public CarType CarType { get; set; }
        public RideStatus Status { get; set; }
        public Comment Comment { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("DriverId")]
        public virtual Driver Driver { get; set; }
        [ForeignKey("DispatcherId")]
        public virtual Dispatcher Dispatcher { get; set; }
    }

    public enum RideStatus
    {
        Created = 0,
        Formed,
        Processed,
        Accepted,
        Canceled,
        Successful,
        Unsuccessful
    }
}
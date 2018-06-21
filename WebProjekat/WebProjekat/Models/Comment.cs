using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProjekat.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RideId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
        
        public virtual Customer Customer { get; set; }
        public virtual Ride Ride { get; set; }
    }
}
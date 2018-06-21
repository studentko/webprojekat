using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProjekat.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Driver")]
        public int DriverId { get; set; }
        public int ModelYear { get; set; }
        public string RegistrationNumber { get; set; }
        public CarType CarType { get; set; }

        public virtual Driver Driver { get; set; }
    }

    public enum CarType
    {
        Car = 0,
        Van
    }
}
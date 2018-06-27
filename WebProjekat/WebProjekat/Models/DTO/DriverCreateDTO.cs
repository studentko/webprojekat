using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTO
{
    public class DriverCreateDTO
    {
        public RegisterUserDTO DriverInfo { get; set; }

        public int CarModelYear { get; set; }
        public string CarRegistrationNumber { get; set; }
        public CarType CarType { get; set; }

        public string CheckInput()
        {
            string errors = DriverInfo.CheckInput();

            if (CarModelYear < 1900 || CarModelYear > DateTime.Now.Year)
            {
                errors += "Car year isn't valid\n";
            }

            return errors;
        }
    }
}
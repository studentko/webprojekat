using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebProjekat.Models.DTO
{
    public class RegisterUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string JMBG { get; set; }
        public string PhoneNumber { get; set; }

        public string CheckInput()
        {
            string errors = "";

            if (!(Email.Contains("@") && Email.Contains(".")))
            {
                errors = $"Email is not valid\n";
            }

            Regex jmbgRegex = new Regex(@"^\d{13}$");

            if (!jmbgRegex.IsMatch(JMBG))
            {
                errors += $"JMBG is not valid\n";
            }

            Regex phoneRegex = new Regex(@"^\+?[-/\d]+$");

            if (!phoneRegex.IsMatch(PhoneNumber))
            {
                errors += $"Phone number is not valid\n";
            }

            return errors;
        }
    }
}
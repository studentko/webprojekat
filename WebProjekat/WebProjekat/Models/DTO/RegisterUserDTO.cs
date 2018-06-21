using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
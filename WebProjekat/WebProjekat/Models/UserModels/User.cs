using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WebProjekat.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        [ScriptIgnore]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string JMBG { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }

    }

    public enum Gender
    {
        Male = 0,
        Female
    }

    public enum Role
    {
        Customer = 0,
        Driver,
        Dispatcher
    }
}
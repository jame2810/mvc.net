using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace project1.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please select employee.")]
        public int UserOfEmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}

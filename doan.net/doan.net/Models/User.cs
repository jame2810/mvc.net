using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doan.net.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserOfEmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}

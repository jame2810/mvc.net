using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace doan.net.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public IList<DepartmentEmployee> DepartmentEmployees { get; set; }
        public User User { get; set; }
        public WorkingTime WorkingTime { get; set; }
    }
}

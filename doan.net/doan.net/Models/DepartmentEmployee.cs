using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.net.Models
{
    public class DepartmentEmployee
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Empployee { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}

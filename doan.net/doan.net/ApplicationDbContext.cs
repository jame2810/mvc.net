using doan.net.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doan.net.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=EMS;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepartmentEmployee>().HasKey(de => new { de.EmployeeId, de.DepartmentId });

            modelBuilder.Entity<DepartmentEmployee>()
                .HasOne<Employee>(de => de.Empployee)
                .WithMany(s => s.DepartmentEmployees)
                .HasForeignKey(de => de.EmployeeId);


            modelBuilder.Entity<DepartmentEmployee>()
                .HasOne<Department>(de => de.Department)
                .WithMany(s => s.DepartmentEmployees)
                .HasForeignKey(de => de.DepartmentId);

            modelBuilder.Entity<Employee>()
                .HasOne<User>(u => u.User)
                .WithOne(e => e.Employee)
                .HasForeignKey<User>(e => e.UserOfEmployeeId);

            modelBuilder.Entity<Employee>()
                .HasOne<WorkingTime>(w => w.WorkingTime)
                .WithOne(e => e.Employee)
                .HasForeignKey<WorkingTime>(w => w.WorkingTimeOfEmployeeId);
        }
        //entities
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentEmployee> DepartmentEmployees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WorkingTime> WorkingTimes { get; set; }
    }
}

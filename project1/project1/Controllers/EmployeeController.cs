using project1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project1.Models;

namespace project1.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string search, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["EmailSortParm"] = String.IsNullOrEmpty(sortOrder) ? "email_desc" : sortOrder == "email_asc" ? "email_desc" : "email_asc";
            ViewData["PhoneSortParm"] = String.IsNullOrEmpty(sortOrder) ? "phone_desc" : sortOrder == "phone_asc" ? "phone_desc" : "phone_asc";
            ViewData["SalarySortParm"] = String.IsNullOrEmpty(sortOrder) ? "salary_desc" : sortOrder == "salary_asc" ? "salary_desc" : "salary_asc";

            if (search != null)
            {
                pageNumber = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewData["CurrentFilter"] = search;
            var employees = from e in _context.Employees
                            select e;

            if (!String.IsNullOrEmpty(search))
            {
                employees = employees.Where(e => e.Name.Contains(search)
                                         || e.Phone.ToString().Contains(search)
                                         || e.Email.Contains(search)
                                         || e.Salary.ToString() == search);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.Name);
                    break;
                case "email_desc":
                    employees = employees.OrderByDescending(e => e.Email);
                    break;
                case "phone_desc":
                    employees = employees.OrderByDescending(e => e.Phone);
                    break;
                case "salary_desc":
                    employees = employees.OrderByDescending(e => e.Salary);
                    break;
                case "name_asc":
                    employees = employees.OrderBy(e => e.Name);
                    break;
                case "email_asc":
                    employees = employees.OrderBy(e => e.Email);
                    break;
                case "phone_asc":
                    employees = employees.OrderBy(e => e.Phone);
                    break;
                case "salary_asc":
                    employees = employees.OrderBy(e => e.Salary);
                    break;
                default:
                    employees = employees.OrderBy(e => e.Name);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Employee>.CreateAsync(employees.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
    }
}

using project1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

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

		     if (HttpContext.Session.GetString("userId") != null)
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
             var employees = from e in _context.Employees.Include(e => e.WorkingTime)
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
	        else
	        {
	            return RedirectToAction("Login", "Auth");
	        }            
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.WorkingTime)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
        [Bind("Name,Phone,Address,Email,Salary, Gender")] Employee employee, string Type, string Gender)
        {
            
            if (ModelState.IsValid)
            {
                employee.Gender = Int32.Parse(Gender);
                _context.Add(employee);
                await _context.SaveChangesAsync();
                _context.WorkingTimes.Add(new WorkingTime
                {
                    Type = Int32.Parse(Type),
                    WorkingTimeOfEmployeeId = employee.EmployeeId
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = employee.EmployeeId.ToString() });
            }

            ViewBag.message = "Insert failed!";
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Gender = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Text = "Female", Value = "2" },
                    new SelectListItem { Text = "Male", Value = "1"}, 
                }, "Value", "Text");
            ViewBag.WorkingTime = new SelectList(
               new List<SelectListItem>
               {
                    new SelectListItem { Text = "Part Time", Value = "1" },
                    new SelectListItem { Text = "Full Time", Value = "2"},
               }, "Value", "Text");
            var employee = await _context.Employees
                .Include(e => e.WorkingTime)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Update([Bind("EmployeeId,Name,Phone,Address,Email,Salary, Gender")] Employee employee, string Type)
        {

            if (ModelState.IsValid)
            {
                var data = _context.Employees
                    .Include(e => e.WorkingTime)
                    .Where(e => e.EmployeeId == employee.EmployeeId)
                    .Single();

                data.Name = employee.Name;
                data.Email = employee.Email;
                data.Phone = employee.Phone;
                data.Salary = employee.Salary;
                data.Address = employee.Address;
                data.Gender = employee.Gender;
                data.WorkingTime.Type = Int32.Parse(Type);              
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            var dataEdit = _context.Employees.Where(s => s.EmployeeId == employee.EmployeeId).FirstOrDefault();
            return View(dataEdit);
        }

        public ActionResult Delete(int? id)
        {
            Employee employee = _context.Employees
                .Where(e => e.EmployeeId == id)
                .Include(w => w.WorkingTime)
                .FirstOrDefault();
            _context.Remove(employee);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}

using project1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace doan.net.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Check if user is logged in
            // If yes, render list employee
            // Otherwise, redirect to login page
            if (HttpContext.Session.GetString("userId") != null)
            {
                return View(await _context.Employees.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }          
        }
    }
}

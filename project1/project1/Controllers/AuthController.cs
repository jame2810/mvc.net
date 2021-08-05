using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using project1.Data;
using project1.Models;
using System.Security.Cryptography;
using System.Text;

namespace project1.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginPayload)
        {
            if (ModelState.IsValid)
            {
                var md5Password = GetMD5(loginPayload.Password);
                var data = _context.Users.Where(user => user.Username.Equals(loginPayload.Username) && user.Password.Equals(md5Password)).ToList();

                if (data.Count() > 0)
                {
                    byte[] userId = Encoding.UTF8.GetBytes(data.FirstOrDefault().UserId.ToString());
                    byte[] username = Encoding.UTF8.GetBytes(data.FirstOrDefault().Username.ToString());
                    HttpContext.Session.Set("userId", userId);
                    HttpContext.Session.Set("username", username);

                    return RedirectToAction("Index", "Employee");
                }
            }

            if (!String.IsNullOrEmpty(loginPayload.Username) && !String.IsNullOrEmpty(loginPayload.Password))
            {
                ViewData["InvalidUser"] = "Invalid username or password.";
            }

            return View();
        }

        public async Task<IActionResult> Register()
        {
            return View(await _context.Employees.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Register(User userPost)
        {
            if (ModelState.IsValid)
            {
                var check = _context.Users.FirstOrDefault(user => user.Username == userPost.Username);
                var invalidEmp = _context.Users.FirstOrDefault(user => user.UserOfEmployeeId == userPost.UserOfEmployeeId);

                if (invalidEmp != null)
                {
                    ViewData["InvalidEmp"] = "Employee already has an account.";
                    return View(await _context.Employees.ToListAsync());
                }

                if (check == null)
                {
                    userPost.Password = GetMD5(userPost.Password);
                    // _context.Configuration.ValidateOnSaveEnabled = false;

                    _context.Users.Add(userPost);
                    _context.SaveChanges();

                    byte[] userId = Encoding.UTF8.GetBytes(userPost.UserId.ToString());
                    byte[] username = Encoding.UTF8.GetBytes(userPost.Username.ToString());
                    HttpContext.Session.Set("userId", userId);
                    HttpContext.Session.Set("username", username);

                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    ViewData["InvalidUsername"] = "Username already exists.";
                    return View(await _context.Employees.ToListAsync());
                }
            }

            return View(await _context.Employees.ToListAsync());
        }

        // LOGOUT
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


        public static string GetMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] fromData = new UTF8Encoding().GetBytes(str);

            byte[] targetData = md5.ComputeHash(fromData);

            StringBuilder encryptData = new StringBuilder();

            for (int i = 0; i < targetData.Length; i++)
            {
                encryptData.Append(targetData[i].ToString());
            }

            return encryptData.ToString();
        }
    }
}

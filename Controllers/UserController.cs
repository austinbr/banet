using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using banetexam2.Models;
using banetexam2.Factories;

namespace banetexam.Controllers
{
    public class UserController : Controller
    {
        private readonly UserFactory userFactory;
        public UserController(UserFactory connection)
        {
            userFactory = connection;
        }
        
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("currUser", 0);
            if(HttpContext.Session.GetString("errors") != "")
            {
                ViewBag.errors = HttpContext.Session.GetString("errors");
                HttpContext.Session.SetString("errors", "");
            }
            if(HttpContext.Session.GetString("success") != "")
            {
                ViewBag.success = HttpContext.Session.GetString("success");
                HttpContext.Session.SetString("success", "");
            }
            return View("Index");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserViewModel model)
        {
            User newUser = new User{
                Name = model.Name,
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
            };

            if(ModelState.IsValid)
            {
                if(userFactory.FindByUsername(newUser.Username) == null)
                {
                    userFactory.Add(newUser);
                    HttpContext.Session.SetString("success", "User registered - Please log in!");
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.errors = "User already exists!";
                    return View("Index", model);
                }
            }
            else
            {
                return View("Index", model);
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password)
        {
            User currUser = userFactory.FindByEmail(Email);
            if(currUser == null || currUser.Id == 0 || currUser.Password != Password)
            {
                HttpContext.Session.SetString("errors", "Please enter a valid Username/Password!");
                return RedirectToAction("Index");
            }
            else
            {
                HttpContext.Session.SetInt32("currUser", currUser.Id);
                HttpContext.Session.SetString("currUsername", currUser.Name);
                return RedirectToAction("Dashboard", "Idea");
            }
        }

        [HttpGet]
        [Route("user/{UserId}")]
        public IActionResult user(int UserId)
        {
            if(HttpContext.Session.GetInt32("currUser") == null || (int)HttpContext.Session.GetInt32("currUser") == 0)
            {
               return RedirectToAction("Index"); 
            }
            else
            {
                ViewBag.user = userFactory.FindById(UserId);
                return View("User");
            }
        }
    }
}

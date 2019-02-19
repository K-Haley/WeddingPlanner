using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private GuestContext dbContext;
     
        // here we can "inject" our context service into the constructor
        public HomeController(GuestContext context)
        {
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    dbContext.Add(newUser);
                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else{
                return View("Index");
            }
        }
        [HttpPost("login")]
        public IActionResult Login(LoginUser verifyUser)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == verifyUser.LEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(verifyUser, userInDb.Password, verifyUser.LPassword);
                if(result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    ModelState.AddModelError("LEmail", "Invalid Email/Password");
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetInt32("CurrentUser", userInDb.UserId);
                    HttpContext.Session.SetString("CurrentUserName", userInDb.FirstName);
                    int id = userInDb.UserId;
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Index");
            }
        }
        [Route("dashboard")]
        [HttpGet]
        public IActionResult Dashboard()
        {
            int? SessionId = HttpContext.Session.GetInt32("CurrentUser");
            if(SessionId == null)
            {
                return RedirectToAction("Index");
            }
            List<Wedding> allWeddings = dbContext.Weddings.Include(w => w.Guests).ThenInclude(g => g.Owner).ToList();
            System.Console.WriteLine(SessionId);
            User Attendee = dbContext.Users.Include(u => u.GoingTo).ThenInclude(g => g.Wedding).FirstOrDefault(u => u.UserId == SessionId);
            ViewBag.SessionId = SessionId;
            ViewBag.Attending = Attendee;
            return View(allWeddings);
        }
        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [Route("add")]
        [HttpGet]
        public IActionResult Add()
        {
            int? SessionId = HttpContext.Session.GetInt32("CurrentUser");
            ViewBag.SessionId = SessionId;
            System.Console.WriteLine(SessionId);
            return View();
        }
        [HttpPost("createwedding")]
        public IActionResult CreateWedding(Wedding NewWedding)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(NewWedding);
                dbContext.SaveChanges();
                System.Console.WriteLine("Wedding ID: "+ NewWedding.WeddingId);
                return Redirect("/details/"+NewWedding.WeddingId);
            }
            else
            {
                int? SessionId = HttpContext.Session.GetInt32("CurrentUser");
                ViewBag.SessionId = SessionId;
                return View("Add");
            }
        }
        [Route("details/{weddingid}")]
        [HttpGet]
        public IActionResult Details(int WeddingId)
        {
            var OneWedding = dbContext.Weddings.Include(w => w.Guests).ThenInclude(g => g.Owner).FirstOrDefault(w => w.WeddingId == WeddingId);
            return View(OneWedding);
        }

        [Route("delete/{wedId}")]
        [HttpGet]
        public IActionResult DeleteWedding(int wedId)
        {
            System.Console.WriteLine("wedding ID is"+ wedId);
            Wedding WeddingToDelete = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == wedId);
            dbContext.Weddings.Remove(WeddingToDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [Route("deleteguest/{wedId}")]
        [HttpGet]
        public IActionResult DeleteGuest(int wedId)
        {
            int? SessionId = HttpContext.Session.GetInt32("CurrentUser");
            Guest ToRemove = dbContext.Guests.FirstOrDefault(g => g.UserId == SessionId && g.WeddingId == wedId);
            dbContext.Guests.Remove(ToRemove);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [Route("addguest/{wedId}")]
        [HttpGet]
        public IActionResult AddGuest(int wedId)
        {
            int? SessionId = HttpContext.Session.GetInt32("CurrentUser");
            Guest NewGuest = new Guest((int)SessionId, wedId);
            dbContext.Add(NewGuest);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}

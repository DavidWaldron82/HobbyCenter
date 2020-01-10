using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HobbyCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace HobbyCenter.Controllers
{
    public class HomeController : Controller
    {
         private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register-user")]
        public IActionResult Register (User user) {
            if(ModelState.IsValid) {
                if(dbContext.Users.Any(a => a.Email == user.Email)) {
                    ModelState.AddModelError("Email", "Email already exists...");
                    return View("Index");}
                if(dbContext.Users.Any(u => u.UserName == user.UserName)) {
                    ModelState.AddModelError("UserName", "UserName already exists...");
                    return View("Index");
                
                } else {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    user.Password = Hasher.HashPassword(user, user.Password);
                    dbContext.SaveChanges();
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("logged_user", user.UserId);
                    return RedirectToAction("Dashboard");
                }
                
            } else {
                return View("Index");
            }
        }
        [HttpGet("clear")]
        public IActionResult Clear() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost("login-user")]
        public IActionResult LoginUser(LoginUser logged) {
            var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == logged.Email);
            if(ModelState.IsValid) {
                if(userInDb == null) {
                    ModelState.AddModelError("Email", "Invalid email/password!");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(logged, userInDb.Password, logged.Password);
                if(result == 0) {
                    ModelState.AddModelError("Password", "Invalid Email/Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("logged_user", userInDb.UserId);
                return RedirectToAction("Dashboard");
            }
            else {
                return View("Index");
            }

        }
        [HttpGet("Dashboard")]
        public IActionResult Dashboard() {
            ViewModel Dashview = new ViewModel
            {
                AllHobbies = dbContext.Hobbies.Include(e => e.Ethusiasts).ThenInclude(u => u.User).ToList(),
                OneUser =dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user"))
            };
            if(HttpContext.Session.GetInt32("logged_user") != null) {
                return View(Dashview);
            } else {
                ModelState.AddModelError("Email", "You are not logged in!");
                return View("Index");
            }
            }
        [HttpGet("New")]
        public IActionResult New(){
            ViewModel Newview = new ViewModel
            {
                OneHobby = new Hobby()
            };
            return View();
        }
        [HttpPost("createhobby")]
        public IActionResult CreateHobby(ViewModel data){
            if(ModelState.IsValid){
            Hobby h = new Hobby
            {
                Name = data.OneHobby.Name,
                Description = data.OneHobby.Description,
            };
            if(dbContext.Hobbies.Any(u => u.Name == h.Name)) {
                    ModelState.AddModelError("OneHobby.Name", "Name already exists...");
                    return View("New");}
            dbContext.Hobbies.Add(h);
                dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
            } else {
                ModelState.AddModelError("Name","What's the name of your hobby?");
                ModelState.AddModelError("Description","Describe your hobby");
                return View("New", data);
            }
            }
            [HttpGet("Hobby/{Id}")]
        public IActionResult Hobby(int Id) {
            ViewModel Hobbyview = new ViewModel
            {
                AllHobbies = dbContext.Hobbies.Include(j => j.Ethusiasts).ThenInclude(u => u.User).ToList(),
                OneUser =dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user")),
                OneHobby = dbContext.Hobbies.SingleOrDefault(k => k.HobbyId == Id)
            };
            if(HttpContext.Session.GetInt32("logged_user") != null) {
                return View("Hobby",Hobbyview);
            } else {
                ModelState.AddModelError("Email", "You are not logged in!");
                return View("Index");
            }
            }
            [HttpGetAttribute("join/Id")]
        public IActionResult Join(int Id)
        {
            UserHob jo = new UserHob
            {
                UserId = dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user")).UserId,
                HobbyId = Id
            };
            ViewModel Dashview = new ViewModel
            {
                AllHobbies = dbContext.Hobbies.Include(j => j.Ethusiasts).ThenInclude(u => u.User).ToList(),
                OneUser =dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user"))
            };
            if(dbContext.UserHobs.Any(p => p.HobbyId==Id && p.UserId ==dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user")).UserId)){
                    ModelState.AddModelError("Enthusiasts", "User is already enthusiastic about his hobby");
                    return View("Dashboard", Dashview);
            } else {
                dbContext.Add(jo);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
        }
        [HttpGet("Hobby/Edit/{Id}")]
        public IActionResult Edit(int Id) {
            ViewModel Hobbyview = new ViewModel
            {
                AllHobbies = dbContext.Hobbies.Include(j => j.Ethusiasts).ThenInclude(u => u.User).ToList(),
                OneUser =dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user")),
                OneHobby = dbContext.Hobbies.SingleOrDefault(k => k.HobbyId == Id)
            };
            if(HttpContext.Session.GetInt32("logged_user") != null) {
                return View("Edit",Hobbyview);
            } else {
                ModelState.AddModelError("Email", "You are not logged in!");
                return View("Index");
            }
            }
        [HttpPost("updatehobby/{Id}")]
        public IActionResult UpdateHobby(ViewModel data, int Id){
            ViewModel Hobbyview = new ViewModel
            {
                AllHobbies = dbContext.Hobbies.Include(j => j.Ethusiasts).ThenInclude(u => u.User).ToList(),
                OneUser =dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user")),
                OneHobby = dbContext.Hobbies.SingleOrDefault(k => k.HobbyId == Id)
            };
            
            if(ModelState.IsValid){
                
            Hobby h = dbContext.Hobbies.FirstOrDefault(x => x.HobbyId == Id);
            {
                h.Name = data.OneHobby.Name;
                h.Description = data.OneHobby.Description;
                h.UpdatedAt = DateTime.Now;
            };
            if(dbContext.Hobbies.Any(u => u.Name == h.Name)){
                ModelState.AddModelError("OneHobby.Name","This hobby aleady exists");
                return View("Edit",Hobbyview);
            }
            else{
                dbContext.SaveChanges();
                }
            return RedirectToAction("Dashboard");
            } else {
                ModelState.AddModelError("Name","What's the name of your hobby?");
                ModelState.AddModelError("Description","Describe your hobby");
                return View("Edit",Hobbyview);
            }
            }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

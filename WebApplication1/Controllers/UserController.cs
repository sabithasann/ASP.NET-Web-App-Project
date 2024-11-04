using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        ProjectDBContext _dbContext;
        public UserController()
        {
            this._dbContext = new ProjectDBContext();
        }
        // GET: User
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User u)
        {
            ModelState.Remove("Email");
            ModelState.Remove("Phone");
            ModelState.Remove("RePassword");
            if(ModelState.IsValid)
            {
                var data = _dbContext.User
                    .Where(x => x.UserName == u.UserName && x.Password == u.Password)
                    .FirstOrDefault();
                if(data != null)
                {
                    TempData["Login"] = "Login is successfull.";
                    Session["UserId"] = data.UserId;
                    Session["UserName"] = data.UserName;
                    var role = _dbContext.Role
                        .Where(x => x.RoleId == data.RoleId)
                        .FirstOrDefault();
                    Session["Role"] = role.RoleName;

                    if (role.RoleName == "doctor")
                        return RedirectToAction("Index", "Doctor");
                    else if (role.RoleName == "assistant")
                        return RedirectToAction("Index", "Assistant");
                    else if (role.RoleName == "patient")
                        return RedirectToAction("Index", "Patient");
                    else
                    {
                        ViewBag.Invalid = "Something went wrong, try again.";
                        ModelState.Clear();
                        return View();
                    }       
                }
                ViewBag.Invalid = "Invalid User!";
                ModelState.Clear();
                return View();
            }
            ModelState.Clear();
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            TempData["Logout"] = "Logged out from the system.";

            return View("Login");
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User u)
        {
            if(_dbContext.User.Any(x => x.UserName == u.UserName))
            {
                ViewBag.Invalid = "Username already existed! Please write a unique Username.";
                ModelState.Clear();
                return View();
            }
            u.RoleId = 3;
            _dbContext.User.Add(u);
            _dbContext.SaveChanges();

            int userId = u.UserId;

            PatientDetails patientDetails = new PatientDetails { UserId = userId };

            _dbContext.PatientDetail.Add(patientDetails);
            _dbContext.SaveChanges();

            TempData["Signup"] = "Signup is successfull.";
            return RedirectToAction("Login", "User");
        }
    }
}
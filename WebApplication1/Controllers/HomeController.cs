using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        ProjectDBContext _dbContext;
        public HomeController()
        {
            this._dbContext = new ProjectDBContext();
        }
        public ActionResult Index()
        {
            var chamberList = _dbContext.Chamber.ToList();
            if (chamberList == null)
                return HttpNotFound();
            return View(chamberList);
        }

        public ActionResult AboutDoctor()
        {
            return View();
        }
        public ActionResult TakeAppointment()
        {
            @TempData["needLogin"] = "You need to Login first to take an appoinment.";
            return RedirectToAction("Login", "User");
        }
    }
}
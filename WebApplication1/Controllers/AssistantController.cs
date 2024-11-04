using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AssistantController : Controller
    {
        ProjectDBContext _dbContext;
        public AssistantController()
        {
            this._dbContext = new ProjectDBContext();
        }
        // GET: Assistant
        public ActionResult Index()
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var assistant = _dbContext.User.Where(x => x.UserId == userId).FirstOrDefault();
            var appointmentList = _dbContext.Appointment
                .Include(c => c.Chamber)
                .Include(u => u.User)
                .Where(x => x.ChamberId == assistant.ChamberId && (x.CheckIn == null || x.CheckIn == false) && (x.Prescribed == null || x.Prescribed == false) && x.AppointmentDate == DateTime.Today)
                .ToList();
            return View(appointmentList);
        }

        [HttpPost]
        public ActionResult CheckIn(Wallet w)
        {
            if (w != null && w.BillAmount >= 0)
            {
                int userId = Convert.ToInt32(Session["UserId"].ToString());
                w.UserId = userId;
                w.BillingDate = DateTime.Today;
                _dbContext.Wallet.Add(w);

                var appointment = _dbContext.Appointment.Where(a => a.AppointmentId == w.AppointmentId).FirstOrDefault();
                if (appointment != null)
                    appointment.CheckIn = true;

                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Assistant");
            }
            return RedirectToAction("Index", "Assistant");
        }

        public ActionResult Wallet()
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            var wallet = _dbContext.Wallet
                .Include(w => w.Appointment)
                .Include(w => w.Chamber)
                .Include(w => w.User)
                .Where(w => w.BillingDate == DateTime.Today).ToList();
            var totalBillAmountToday = wallet.Sum(w => w.BillAmount);
            
            ViewBag.TotalBillAmountToday = totalBillAmountToday;

            return View(wallet);
        }

        public ActionResult AssistantProfile()
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var assistantDetails = _dbContext.AssistantDetail
                .Include(u => u.User)
                .Where(x => x.UserId == userId)
                .FirstOrDefault();
            return View(assistantDetails ?? null);
        }

        public ActionResult UpdateProfile(AssistantDetails a)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            if (a.AssistantDetailsId > 0)
                return View(a);
            else
                return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditProfile(AssistantDetails a)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var assistantDetails = _dbContext.AssistantDetail
                .Include(u => u.User)
                .Where(x => x.UserId == userId)
                .FirstOrDefault();

            assistantDetails.FullName = a.FullName;
            assistantDetails.DateOfBirth = a.DateOfBirth;
            assistantDetails.Gender = a.Gender;
            assistantDetails.Address = a.Address;
            assistantDetails.BloodGroup = a.BloodGroup;
            assistantDetails.EmergencyContact = a.EmergencyContact;

            _dbContext.Entry(assistantDetails).State = EntityState.Modified;
            _dbContext.SaveChanges();
            TempData["Edit"] = "Assistant information updated successfully.";

            return RedirectToAction("AssistantProfile", "Assistant");
        }
    }
}
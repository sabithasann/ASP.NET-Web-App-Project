using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;
using System.Data.Entity;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PatientController : Controller
    {
        ProjectDBContext _dbContext;
        public PatientController()
        {
            this._dbContext = new ProjectDBContext();
        }
        // GET: Patient
        public ActionResult Index()
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var appointment = _dbContext.Appointment
                .Include(c => c.Chamber)
                .Where(x => x.UserId == userId && (x.Prescribed == null || x.Prescribed == false) && x.AppointmentDate >= DateTime.Today)
                .FirstOrDefault();
            return View(appointment ?? null);
        }

        public ActionResult TakeAppointment()
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            var chamberList = _dbContext.Chamber.ToList();
            if(chamberList == null)
                return HttpNotFound();
            return View(chamberList);
        }

        [HttpPost]
        public ActionResult TakeAppointment(Appointment appointment)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            DateTime tomorrow = DateTime.Now.AddDays(1).Date;
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            if (_dbContext.Appointment.Any(a => a.Time == appointment.Time && a.AppointmentDate == tomorrow))
            {
                var chamberList = _dbContext.Chamber.ToList();
                if (chamberList == null)
                    return HttpNotFound();
                ViewBag.Invalid = "Your selected time slot is already taken! Please select another one.";
                ModelState.Clear();
                return View(chamberList);
            }
            if(_dbContext.Appointment.Any(x => x.UserId == userId && (x.Prescribed == null || x.Prescribed == false)))
            {
                var chamberList = _dbContext.Chamber.ToList();
                if (chamberList == null)
                    return HttpNotFound();
                ViewBag.Invalid = "You have already taken an appointment slot. Only one appointment is allowed in a day.";
                ModelState.Clear();
                return View(chamberList);
            }
            appointment.AppointmentDate = tomorrow;
            appointment.UserId = Convert.ToInt32(Session["UserId"].ToString());

            _dbContext.Appointment.Add(appointment);
            _dbContext.SaveChanges();
            TempData["Appointment"] = "Your Appointment has been added successfully.";
            return RedirectToAction("Index", "Patient");
        }
        
        public ActionResult CancelAppointment(int id)
        {
            var data = _dbContext.Appointment
                .Where(x => x.AppointmentId == id)
                .FirstOrDefault();
            if (data == null)
                return HttpNotFound();
            if(data.CheckIn == true)
            {
                TempData["CheckIn"] = "You have already Checked In. You can not cancel your appointment now.";
                return RedirectToAction("Index", "Patient");
            }
            _dbContext.Appointment.Remove(data);
            _dbContext.SaveChanges();

            TempData["CancelAppointment"] = "Appointment has been cancelled successfully.";

            return RedirectToAction("Index", "Patient");
        }
        public ActionResult PreviousAppointments()
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var appointmentList = _dbContext.Appointment
                .Include(c => c.Chamber)
                .Where(x => x.UserId == userId).ToList();
            return View(appointmentList);
        }

        public ActionResult PatientProfile()
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var patientDetails = _dbContext.PatientDetail
                .Include(u => u.User)
                .Where(x => x.UserId == userId)
                .FirstOrDefault();
            return View(patientDetails ?? null);
        }

        public ActionResult UpdateProfile(PatientDetails p)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            if (p.PatientDetailsId > 0)
                return View(p);
            else
                return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditProfile(PatientDetails p) 
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var patientDetails = _dbContext.PatientDetail
                .Include(u => u.User)
                .Where(x => x.UserId == userId)
                .FirstOrDefault();

            patientDetails.FullName = p.FullName;
            patientDetails.DateOfBirth = p.DateOfBirth;
            patientDetails.Gender = p.Gender;
            patientDetails.Address = p.Address;
            patientDetails.BloodGroup = p.BloodGroup;
            patientDetails.EmergencyContact = p.EmergencyContact;

            _dbContext.Entry(patientDetails).State = EntityState.Modified;
            _dbContext.SaveChanges();
            TempData["Edit"] = "Patient information updated successfully.";

            return RedirectToAction("PatientProfile", "Patient");
        }

        public ActionResult History()
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var prescriptionList = _dbContext.Prescription
                .Include(c => c.Chamber)
                .Include(c => c.User)
                .Where(x => x.UserId == userId).ToList();
            return View(prescriptionList);
        }

        public ActionResult DetailsPrescription(int? prescriptionId)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            if(prescriptionId == null)
                return HttpNotFound();

            var medicineList = _dbContext.Medication
                .Include(p => p.Prescription)
                .Where(p => p.PrescriptionId == prescriptionId).ToList();
            return View(medicineList);
        }
    }
}
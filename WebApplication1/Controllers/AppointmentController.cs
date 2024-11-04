using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WebApplication1.Context;

namespace WebApplication1.Controllers
{
    public class AppointmentController : Controller
    {
        ProjectDBContext _dbContext;
        public AppointmentController()
        {
            this._dbContext = new ProjectDBContext();
        }
        public JsonResult GetAvailableTimes(int chamberId)
        {
            var availableTimes = _dbContext.TimeSlot.Where(t => t.ChamberId == chamberId).Select(t => new { Time = t.Time}).ToList();

            return Json(new { success = true, data = availableTimes }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreatePrescription(int? appointmentId)
        {
            if (appointmentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var appointment = _dbContext.Appointment
                .Include(a => a.User)
                .FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            var existingPrescription = _dbContext.Prescription.FirstOrDefault(p => p.AppointmentId == appointmentId);
            if (existingPrescription != null)
            {
                TempData["Error"] = "A prescription already exists for this appointment.";
                return RedirectToAction("Index", "Doctor");
            }

            return RedirectToAction("Prescribe", "Prescription", new { appointmentId = appointmentId });
        }
    }
}
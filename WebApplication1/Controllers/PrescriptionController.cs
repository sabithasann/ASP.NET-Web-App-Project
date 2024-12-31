using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WebApplication1.Context;
using WebApplication1.Models;
using System.Data.Entity.Validation;

namespace WebApplication1.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly ProjectDBContext _dbContext;

        public PrescriptionController()
        {
            this._dbContext = new ProjectDBContext();
        }
        public JsonResult GetMedicationNames()
        {
            var medicationNames = _dbContext.Medicine.Select(m => m.Name).ToList();
            return Json(medicationNames, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Prescribe(int? appointmentId)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");
            if (appointmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var appointment = _dbContext.Appointment
                .Include(a => a.User)
                .Include(a => a.Chamber)
                .FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                return HttpNotFound();

            var model = new Prescription
            {
                AppointmentId = appointment.AppointmentId,
                UserId = appointment.UserId,
                ChamberId = appointment.ChamberId,
                PrescriptionDate = DateTime.Now
            };
            return View(model);
        }
        public ActionResult CreatePrescription(Prescription model, List<string> MedicationName, List<string> Dosage, List<string> Instructions)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            if (ModelState.IsValid)
            {
                try
                {
                    var prescription = new Prescription
                    {
                        AppointmentId = model.AppointmentId,
                        UserId = model.UserId,
                        ChamberId = model.ChamberId,
                        PrescriptionDate = model.PrescriptionDate,
                        Diagnosis = model.Diagnosis,
                        AdditionalNotes = model.AdditionalNotes
                    };

                    _dbContext.Prescription.Add(prescription);
                    _dbContext.SaveChanges(); 

                    if (MedicationName != null && MedicationName.Count > 0)
                    {
                        for (int i = 0; i < MedicationName.Count; i++)
                        {
                            var medication = new Medication
                            {
                                PrescriptionId = prescription.PrescriptionId,
                                Name = MedicationName[i],
                                Dosage = Dosage[i],
                                Instructions = Instructions[i]
                            };
                            _dbContext.Medication.Add(medication);
                        }
                        _dbContext.SaveChanges();
                    }

                    // Update appointment as prescribed
                    var appointment = _dbContext.Appointment.FirstOrDefault(a => a.AppointmentId == model.AppointmentId);
                    if (appointment != null)
                    {
                        appointment.Prescribed = true;
                        _dbContext.SaveChanges();
                    }

                    TempData["Success"] = "Prescription added successfully.";
                    return RedirectToAction("Index", "Doctor");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                    ModelState.AddModelError("", "Unable to save changes. Please check the input fields for errors.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            TempData["Error"] = "Something went wrong. Please, try again.";
            return RedirectToAction("Index", "Doctor");
        }
    }
}
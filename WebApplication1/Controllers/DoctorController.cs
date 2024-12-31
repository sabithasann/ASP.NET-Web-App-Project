using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WebApplication1.Context;
using WebApplication1.Models;
using System.Net;
using System.Net.Http;
//using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class DoctorController : Controller
    {
        ProjectDBContext _dbContext;
        public DoctorController()
        {
            this._dbContext = new ProjectDBContext();
        }
        // GET: Doctor
        public ActionResult Index()
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            var appointmentList = _dbContext.Appointment
                .Include(c => c.Chamber)
                .Include(u => u.User)
                .Where(x => x.CheckIn == true && (x.Prescribed == null || x.Prescribed == false) && x.AppointmentDate == DateTime.Today)
                .ToList();

            return View(appointmentList);
        }

        public ActionResult AllAppoinments()
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");
            var appointmentList = _dbContext.Appointment
                .Include(c => c.Chamber)
                .Include(u => u.User)
                .ToList();

            return View(appointmentList);
        }

        public ActionResult DetailsAppointment(int? appointmentId)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            if (appointmentId == null)
                return HttpNotFound();

            var prescription = _dbContext.Prescription
                .Include(a => a.Appointment)
                .Include(a => a.User)
                .Include(a => a.Chamber)
                .Where(a => a.AppointmentId == appointmentId)
                .FirstOrDefault();

            return View(prescription);
        }

        public ActionResult History(int? patientId)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            if (patientId == null)
                return HttpNotFound();

            var prescriptionList = _dbContext.Prescription
                .Include(c => c.Chamber)
                .Include(c => c.User)
                .Where(x => x.UserId == patientId).ToList();
            return View(prescriptionList);
        }

        public ActionResult DetailsPrescription(int? prescriptionId)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            if (prescriptionId == null)
                return HttpNotFound();

            var medicineList = _dbContext.Medication
                .Include(p => p.Prescription)
                .Where(p => p.PrescriptionId == prescriptionId).ToList();
            return View(medicineList);
        }

        public ActionResult ViewAssistant()
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://pioneerhealthcare.somee.com/api/assistant");
                var response = client.GetAsync("assistant");
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var data = response.Result.Content.ReadAsAsync<IEnumerable<User>>().Result;
                    return View(data);
                }
                else
                    return HttpNotFound();
            }
        }

        public ActionResult AssistantProfile(int? Id)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            var assistantDetails = _dbContext.AssistantDetail
                .Include(u => u.User)
                .Where(x => x.UserId == Id)
                .FirstOrDefault();
            return View(assistantDetails ?? null);
        }

        public ActionResult AddNewAssistant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignupAssistant(User u)
        {
            if (_dbContext.User.Any(x => x.UserName == u.UserName))
            {
                ViewBag.Invalid = "Username already existed! Please write a unique Username.";
                ModelState.Clear();
                return View("AddNewAssistant");
            }
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://pioneerhealthcare.somee.com/api/signupassistant");
                var response = client.PostAsJsonAsync("signupassistant", u);
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    TempData["Signup"] = "Assistant signup is successfull.";
                    return RedirectToAction("ViewAssistant", "Doctor");
                }
                else
                    return HttpNotFound();
            }
        }

        public ActionResult Update(User u)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            if (u.UserId > 0)
                return View(u);
            else
                return HttpNotFound();
        }

        [HttpPost]
        public ActionResult UpdateNow(User u)
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            ModelState.Remove("UserName");
            ModelState.Remove("Password");
            ModelState.Remove("RePassword");
            ModelState.Remove("RoleId");
            if (ModelState.IsValid)
            {
                var user = _dbContext.User
                    .Where(x => x.UserId == u.UserId)
                    .FirstOrDefault();

                if (user == null)
                    return HttpNotFound();

                user.Email = u.Email;
                user.Phone = u.Phone;
                user.ChamberId = u.ChamberId;

                _dbContext.SaveChanges();
                TempData["Edit"] = "Assistant information updated successfully.";

                return RedirectToAction("ViewAssistant", "Doctor");
            }
            else
            {
                return RedirectToAction("ViewAssistant", "Doctor");
            }

        }
        //Put api not working, return 404 not found.
        /*[HttpPost]
        public ActionResult UpdateNow(User u) 
        {
            if (Session["UserName"] == null && Session["UserId"] == null)
                return RedirectToAction("Login", "User");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://pioneerhealthcare.somee.com/api/editassistant");
                var response = client.PutAsJsonAsync("editassistant/" + u.UserId.ToString(), u);
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    TempData["Edit"] = "Assistant information updated successfully.";

                    return RedirectToAction("ViewAssistant", "Doctor");
                }
                else
                    return HttpNotFound();
            }
        }*/

        public ActionResult Delete(int? id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://pioneerhealthcare.somee.com/api/deleteassistant");
                var response = client.DeleteAsync("deleteassistant/" + id.ToString());
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    TempData["Delete"] = "Assistant information deleted successfully.";

                    return RedirectToAction("ViewAssistant", "Doctor");
                }
                else
                    return HttpNotFound();
            }
        }
    }
}
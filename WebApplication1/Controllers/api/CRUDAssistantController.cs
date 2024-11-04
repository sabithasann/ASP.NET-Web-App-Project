using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using WebApplication1.Context;
using WebApplication1.Models;
using System.Web.ModelBinding;

namespace WebApplication1.Controllers.api
{
    public class CRUDAssistantController : ApiController
    {
        ProjectDBContext _dbContext;
        public CRUDAssistantController()
        {
            this._dbContext = new ProjectDBContext();
        }

        [Route("api/assistant")]
        [HttpGet]
        public IEnumerable<User> GetAssistant()
        {
            var assistantList = _dbContext.User
                .Include(c => c.Chamber)
                .Include(r => r.Role)
                .Where(x => x.RoleId == 2)
                .ToList();
            return assistantList;
        }

        [Route("api/signupassistant")]
        [HttpPost]
        public IHttpActionResult SignupAssistant(User u)
        {
            u.RoleId = 2;
            _dbContext.User.Add(u);
            _dbContext.SaveChanges();

            int userId = u.UserId;

            AssistantDetails assistantDetails = new AssistantDetails { UserId = userId };
            _dbContext.AssistantDetail.Add(assistantDetails);
            _dbContext.SaveChanges();

            return Ok(u);
        }

        [Route("api/editassistant/{id}")]
        [HttpPut]
        public IHttpActionResult EditAssistant(int id, User u)
        {
            ModelState.Remove("UserName");
            ModelState.Remove("Password");
            ModelState.Remove("RePassword");
            ModelState.Remove("RoleId");
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var user = _dbContext.User
                    .Where(x => x.UserId == id)
                    .FirstOrDefault();

            if (user == null)
                return NotFound();

            user.Email = u.Email;
            user.Phone = u.Phone;
            user.ChamberId = u.ChamberId;

            _dbContext.SaveChanges();

            return Ok();
        }


        [Route("api/deleteassistant/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteGame(int id)
        {
            var data = _dbContext.User.Where(x => x.UserId == id).FirstOrDefault();

            if (data == null)
                return NotFound();

            _dbContext.User.Remove(data);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}

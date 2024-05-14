using ClimateMonitoring;
using ClimateMonitoringService.DTOs;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.ModelBinding;

namespace ClimateMonitoringService.Controllers
{
    public class UsersController : ApiController
    {
        private readonly ClimateMonitoringDBEntities _dbContext;

        public UsersController()
        {
            _dbContext = new ClimateMonitoringDBEntities();
        }

        // POST api/users/register
        [HttpPost]
        [Route("api/users/register")]
        public IHttpActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST api/users/login
        [HttpPost]
        [Route("api/users/login")]
        public IHttpActionResult Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
            if (user == null)
            {
                return NotFound();
            }

            return Ok("Login successful.");
        }

        // PUT api/users/update
        [HttpPut]
        [Route("api/users/update")]
        public IHttpActionResult UpdateUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = _dbContext.Users.Find(user.UserID);
            if (existingUser == null)
            {
                return NotFound();
            }

            try
            {
                _dbContext.Entry(existingUser).CurrentValues.SetValues(user);
                _dbContext.SaveChanges();
                return Ok("User information updated successfully.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/users/{id}
        [HttpGet]
        [Route("api/users/{id}")]
        public IHttpActionResult GetUser(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

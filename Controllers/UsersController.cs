using JobTrackingProject.Application.Interfaces;
using JobTrackingProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackingProject.Controllers
{
    [Route("api/[controller]")]
   // [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _UserSerivce;
        public UsersController(IUserService userSerivce)
        {
            _UserSerivce = userSerivce;
        }
       
        [HttpGet]
        public ActionResult <IEnumerable<User>> GetUsers()
        {
            return Ok(_UserSerivce.GetUsers());
        }
        
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var job = _UserSerivce.GetUserById(id);
            if (job == null) return NotFound();
            return Ok(job);
        }

        [HttpPost]
        public ActionResult AddUsers([FromBody] User user)
        {
            _UserSerivce.AddUsers(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id) return BadRequest();
            _UserSerivce.UpdateUser(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            _UserSerivce.DeleteUser(id);
            return NoContent();
        }
    }
}

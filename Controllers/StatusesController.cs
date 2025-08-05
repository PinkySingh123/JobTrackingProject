using JobTrackingProject.Application.Interfaces;
using JobTrackingProject.Application.Services;
using JobTrackingProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackingProject.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        private readonly IStatusService _statusService;
        public StatusesController(IStatusService statusService)
        {
            _statusService = statusService; 
        }
        [HttpGet]
        public ActionResult <IEnumerable<Status>> GetStatuses()
        {
            return Ok(_statusService.GetStatus());
        }
        [HttpGet("{id}")]
        public ActionResult<Status> GetStatusById(int id)
        {
            var status = _statusService.GetStatusById(id);
            if(status == null) return NotFound();
            return Ok(status);
        }

        [HttpPost]
        public ActionResult AddStatuses([FromBody] Status status)
        {
            _statusService.AddStatus(status);
            return CreatedAtAction(nameof(GetStatusById), new { id = status.Id }, status);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateStatus(int id, [FromBody] Status status)
        {
            if (id != status.Id) return BadRequest();
            _statusService.UpdateStatus(status);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            _statusService.DeleteStatus(id);
            return NoContent();
        }
    }
}

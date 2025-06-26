using JobTrackingProject.Application.Interfaces;
using JobTrackingProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackingProject.Controllers
{
    [Route("api/[controller]")]
   // [Authorize]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Job>> GetJobs()
        {
            return Ok(_jobService.GetJobs());
        }

        [HttpGet("{id}")]
        public ActionResult<Job> GetJobById(int id)
        {
            var job = _jobService.GetJobById(id);
            if (job == null) return NotFound();
            return Ok(job);
        }

        [HttpPost]
        public ActionResult AddJob([FromBody] Job job)
        {
            _jobService.AddJob(job);
            return CreatedAtAction(nameof(GetJobById), new { id = job.Id }, job);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateJob(int id, [FromBody] Job job)
        {
            if (id != job.Id) return BadRequest();
            _jobService.UpdateJob(job);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteJob(int id)
        {
            _jobService.DeleteJob(id);
            return NoContent();
        }
    }
}

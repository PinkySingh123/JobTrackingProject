using JobTrackingProject.Application.Interfaces;
using JobTrackingProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackingProject.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class JobTypesController : ControllerBase
    {
        private readonly IJobTypeService _jobTypeService;
        public JobTypesController(IJobTypeService jobTypebService)
        {
            _jobTypeService = jobTypebService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<JobTypes>> GetJobTypes()
        {
            return Ok(_jobTypeService.GetJobTypes());
        }

        [HttpGet("{id}")]
        public ActionResult<JobTypes> GetJobTypeById(int id)
        {
            var job = _jobTypeService.GetJobTypesById(id);
            if (job == null) return NotFound();
            return Ok(job);
        }

        [HttpPost]
        public ActionResult AddJobTypes([FromBody] JobTypes jobType)
        {
            _jobTypeService.AddJobTypes(jobType);
            return CreatedAtAction(nameof(GetJobTypeById), new { id = jobType.Id }, jobType);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateJobType(int id, [FromBody] JobTypes jobType)
        {
            if (id != jobType.Id) return BadRequest();
            _jobTypeService.UpdateJobTypes(jobType);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            _jobTypeService.DeleteJobTypes(id);
            return NoContent();
        }
    }
}

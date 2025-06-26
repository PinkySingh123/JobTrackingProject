using JobTrackingProject.Application.Interfaces;
using JobTrackingProject.Domain.Entities;
using JobTrackingProject.Infrastructure.Data;

namespace JobTrackingProject.Application.Services
{
    public class JobTypesService : IJobTypeService
    {
        private readonly JobDbContext _context;
        public JobTypesService(JobDbContext context)
        {
            _context = context;
        }
        public IEnumerable<JobTypes> GetJobTypes() => _context.JobTypes.ToList();
        public JobTypes GetJobTypesById(int id) => _context.JobTypes.FirstOrDefault(jt => jt.Id == id);
        public void AddJobTypes(JobTypes jobTypes)
        {
            // jobTypes.Id =
            _context.JobTypes.Add(jobTypes);
            _context.SaveChanges();
        }
        public void UpdateJobTypes(JobTypes jobtype)
        {
            _context.JobTypes.Update(jobtype);
            _context.SaveChanges();
        }

        public void DeleteJobTypes(int id)
        {
            var jobType = _context.JobTypes.Find(id);
            if (jobType != null) 
            {
                _context.JobTypes.Remove(jobType);
                _context.SaveChanges();
            }
        }

    }
}

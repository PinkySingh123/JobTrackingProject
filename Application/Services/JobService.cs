using JobTrackingProject.Application.Interfaces;
using JobTrackingProject.Domain.Entities;
using JobTrackingProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTrackingProject.Application.Services
{
    public class JobService : IJobService
    {
        private readonly JobDbContext _context;
        public JobService(JobDbContext Context)
        {
            _context = Context;
        }

        public IEnumerable<Job> GetJobs() => _context.Jobs.ToList();

        public Job GetJobById(int id) => _context.Jobs.FirstOrDefault(j => j.Id == id);

        public void AddJob(Job job)
        {
            _context.Jobs.Add(job);
            _context.SaveChanges();
        }

        public void UpdateJob(Job job)
        {
            _context.Jobs.Update(job);
            _context.SaveChanges();
        }

        public void DeleteJob(int id)
        {
            var job = _context.Jobs.Find(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
                _context.SaveChanges();
            }
        }
    }
}

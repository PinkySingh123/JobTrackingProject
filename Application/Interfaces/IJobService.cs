using JobTrackingProject.Domain.Entities;

namespace JobTrackingProject.Application.Interfaces
{
    public interface IJobService
    {
        IEnumerable<Job> GetJobs();
        Job GetJobById(int id);
        void AddJob(Job job);
        void UpdateJob(Job job);
        void DeleteJob(int id);
    }
}

using JobTrackingProject.Domain.Entities;

namespace JobTrackingProject.Application.Interfaces
{
    public interface IJobTypeService
    {
        IEnumerable<JobTypes> GetJobTypes();
        JobTypes GetJobTypesById(int id);
        void AddJobTypes(JobTypes job);
        void UpdateJobTypes(JobTypes job);
        void DeleteJobTypes(int id);
    }
}

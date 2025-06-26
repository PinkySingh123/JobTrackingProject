using JobTrackingProject.Domain.Entities;

namespace JobTrackingProject.Application.Interfaces
{
    public interface IStatusService
    {
        IEnumerable<Status> GetStatus();
        Status GetStatusById(int id);
        void AddStatus(Status status);
        void UpdateStatus(Status status);
        void DeleteStatus(int id);
    }
}

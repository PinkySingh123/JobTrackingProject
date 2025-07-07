using JobTrackingProject.Application.Interfaces;
using JobTrackingProject.Domain.Entities;
using JobTrackingProject.Infrastructure.Data;

namespace JobTrackingProject.Application.Services
{
    public class StatusServices : IStatusService
    {
        private readonly JobDbContext _context;
        public StatusServices(JobDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Status> GetStatus() => _context.Statuses;
        public Status GetStatusById(int id) => _context.Statuses.FirstOrDefault(jt => jt.Id == id);
        public void AddStatus(Status Status)
        {
            // Statuss.Id =
            _context.Statuses.Add(Status);
            _context.SaveChanges();
        }
        public void UpdateStatus(Status status)
        {
            var existingStatus = _context.Statuses.Find(status.Id);
            if (existingStatus == null)
                throw new Exception("Status not found.");
                existingStatus.Name= status.Name;
            _context.SaveChanges();
        }

        public void DeleteStatus(int id)
        {
            var status = _context.Statuses.Find(id);
            if (status != null)
            {
                _context.Statuses.Remove(status);
                _context.SaveChanges();
            }
           
        }
    }
}

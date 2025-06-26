using JobTrackingProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobTrackingProject.Infrastructure.Data
{
    public class JobDbContext : DbContext
    {
        public JobDbContext(DbContextOptions<JobDbContext> options) : base(options) { }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JobTypes> JobTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }
    }
}

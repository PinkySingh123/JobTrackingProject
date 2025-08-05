using JobTrackingProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobTrackingProject.Infrastructure.Data
{
    public class JobDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public JobDbContext(DbContextOptions<JobDbContext> options) : base(options) { }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JobTypes> JobTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }
    }
}

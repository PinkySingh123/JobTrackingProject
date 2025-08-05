using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobTrackingProject.Domain.Entities
{
    [Table("AspNetUsers")]
    public class User : IdentityUser
    {
        //public int Id { get; set; }
        public string? Name { get; set; }
        //public string? Email { get; set; }
        //public string? PasswordHash { get; set; }

        //public string UserName { get; set; }
        //public string ConcurrencyStamp { get; set; }
        public DateTime CreatedAt { get; set; }

        //public DateTimeOffset LockoutEnd { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace JobTrackingProject.Domain.Entities
{
    public class JobTypes
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
    }
}

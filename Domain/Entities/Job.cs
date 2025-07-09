namespace JobTrackingProject.Domain.Entities
{
    public class Job
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public int StatusId { get; set; } // e.g. Applied, Interview, Offered, Rejected
        public DateTime CreatedAt { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Location { get; set; }
        public string JobUrl { get; set; }
        public string Notes { get; set; }

    }
}

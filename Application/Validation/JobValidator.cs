using FluentValidation;
using JobTrackingProject.Domain.Entities;
namespace JobTrackingProject.Application.Validation
{
    public class JobValidator:AbstractValidator<Job>
    {
        public JobValidator()
        {
            RuleFor(job => job.JobTitle)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title must be less than 100 characters");

            RuleFor(job => job.CompanyName)
                .NotEmpty().WithMessage("Company is required")
                .MaximumLength(100).WithMessage("Company must be less than 100 characters");

            RuleFor(job => job.Location)
                .NotEmpty().WithMessage("Location is required")
                .MaximumLength(100).WithMessage("Location must be less than 100 characters");

            RuleFor(job => job.StatusId)
                .NotEmpty().WithMessage("Status is required")
                .Must(status => new[] { "Applied", "Interview", "Offered", "Rejected" }.Contains(status))
                .WithMessage("Status must be one of: Applied, Interview, Offered, Rejected");

            RuleFor(job => job.ApplicationDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Applied date cannot be in the future");
        }
    }
}

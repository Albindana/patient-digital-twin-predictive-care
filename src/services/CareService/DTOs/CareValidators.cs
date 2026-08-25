using FluentValidation;

namespace CareService.DTOs;

public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
{
    public CreatePatientDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DateOfBirth).LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.");
        RuleFor(x => x.BloodType).Matches(@"^(A|B|AB|O)[+-]$").WithMessage("Invalid blood type format (e.g., A+, O-).");
        RuleFor(x => x.EmergencyContact).NotEmpty().MinimumLength(10);
        RuleFor(x => x.InsuranceNumber).NotEmpty();
    }
}

public class CreateCarePlanDtoValidator : AbstractValidator<CreateCarePlanDto>
{
    public CreateCarePlanDtoValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.StartDate).GreaterThanOrEqualTo(DateTime.UtcNow.Date);
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("End date must be after start date.");
    }
}
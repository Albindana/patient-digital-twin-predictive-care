namespace CareService.DTOs;

public record CreatePatientDto(
    Guid UserId,
    DateTime DateOfBirth,
    string BloodType,
    string EmergencyContact,
    string InsuranceNumber
);

public record CreateCarePlanDto(
    Guid PatientId,
    string Title,
    DateTime StartDate,
    DateTime? EndDate
);
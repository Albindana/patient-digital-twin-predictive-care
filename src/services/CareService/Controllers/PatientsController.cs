using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using CareService.Data;
using CareService.Entities;
using CareService.DTOs;

namespace CareService.Controllers;

[ApiController]
[Route("api/care/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IValidator<CreatePatientDto> _validator;

    public PatientsController(ApplicationDbContext context, IValidator<CreatePatientDto> validator)
    {
        _context = context;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePatient([FromBody] CreatePatientDto request)
    {
        // 1. Run FluentValidation rules
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        // 2. Map DTO to Entity
        var patient = new Patient
        {
            UserId = request.UserId,
            DateOfBirth = request.DateOfBirth,
            BloodType = request.BloodType,
            EmergencyContact = request.EmergencyContact,
            InsuranceNumber = request.InsuranceNumber,
            CreatedBy = "System"
        };

        // 3. Save to CareDb
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(CreatePatient), new { id = patient.Id }, patient);
    }
}
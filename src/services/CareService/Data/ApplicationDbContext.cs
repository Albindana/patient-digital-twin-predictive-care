using Microsoft.EntityFrameworkCore;
using CareService.Entities;

namespace CareService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<CarePlan> CarePlans => Set<CarePlan>();
    public DbSet<Medication> Medications => Set<Medication>();
    public DbSet<PatientDoctorAssignment> PatientDoctorAssignments => Set<PatientDoctorAssignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property("Id")
                    .HasDefaultValueSql("NEWSEQUENTIALID()");
            }
        }
    }
}
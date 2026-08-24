using Microsoft.EntityFrameworkCore;
using IdentityService.Entities;

namespace IdentityService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users => Set<User>();
}
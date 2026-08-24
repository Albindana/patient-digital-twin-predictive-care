using Microsoft.EntityFrameworkCore;
using IdentityService.Data;
using IdentityService.Repositories.Interfaces;
using IdentityService.Repositories.Implementations;
using IdentityService.Services;

var builder = WebApplication.CreateBuilder(args);

// DbContext Registration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository DI Registration
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();
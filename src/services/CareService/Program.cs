using Microsoft.EntityFrameworkCore;
using FluentValidation;
using CareService.Data;

var builder = WebApplication.CreateBuilder(args);

// DbContext setup for isolated CareDb
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Automatically register all FluentValidation validators in the assembly
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.Run();
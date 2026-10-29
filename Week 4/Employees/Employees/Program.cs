using Employees.Data;
using Microsoft.EntityFrameworkCore;
using Employees.Models;
using Employees.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//api configuration
builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


using Microsoft.EntityFrameworkCore;
using ProductInventoryApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Register database connection
builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Register controllers
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

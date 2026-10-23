
using CoursesWebApp.Data;
using CoursesWebApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register controllers
builder.Services.AddControllers();

// Register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Register application services
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<LearnerService>();
builder.Services.AddScoped<EnrollmentService>();
builder.Services.AddScoped<PaymentService>();

// Register payment processors
builder.Services.AddScoped<IPaymentProcessor, OnlinePaymentProcessor>();
builder.Services.AddScoped<IPaymentProcessor, ClassroomPaymentProcessor>();

var app = builder.Build();
 
// Enable Swagger in Development and Production
app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();

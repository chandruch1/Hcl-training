
using Courses.Services;
using Courses.Payments;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Keep in-memory data alive across HTTP requests
builder.Services.AddSingleton<CourseService>();
builder.Services.AddSingleton<EnrollmentService>();
builder.Services.AddSingleton<LearnerService>();

builder.Services.AddSingleton<IPaymentProcessor>(
    new OnlinePaymentProcessor(false));

builder.Services.AddSingleton<IPaymentProcessor>(
    new ClassroomPaymentProcessor(true));

builder.Services.AddSingleton<PaymentService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();

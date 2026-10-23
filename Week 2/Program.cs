using CoursesWebApp.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton<CourseService>();
builder.Services.AddSingleton<LearnerService>();
builder.Services.AddSingleton<EnrollmentService>();
builder.Services.AddSingleton<IPaymentProcessor, OnlinePaymentProcessor>();
builder.Services.AddSingleton<IPaymentProcessor, ClassroomPaymentProcessor>();
builder.Services.AddSingleton<PaymentService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();


using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StudentApi.Models;
using StudentApi.Services;
using Microsoft.EntityFrameworkCore;
using Learning1.Data;
using Learning1.Models;
using Learning1.Sevices;
using Learning1.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Strongly typed configuration
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

// Application services
builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<StudentService>();
builder.Services.AddScoped<EmployeeService>();

//EF core configuration
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Read JWT settings from appsettings.json
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT key is missing.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("JWT issuer is missing.");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("JWT audience is missing.");

// JWT Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
    });

// Authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StudentAccess", policy =>
        policy.RequireAuthenticatedUser()
              .RequireRole("Student"));
});

//Cors configuration

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Swagger with JWT Bearer support
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Learning1 API",
        Version = "v1",
        Description = "API with JWT authentication"
    });

    // Define Bearer authentication
    options.AddSecurityDefinition("Bearer",
        new Microsoft.OpenApi.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.ParameterLocation.Header,
            Description = "Paste your JWT token here."
        });

    // Apply Bearer authentication to Swagger operations
    options.AddSecurityRequirement(document =>
        new Microsoft.OpenApi.OpenApiSecurityRequirement
        {
            [new Microsoft.OpenApi.OpenApiSecuritySchemeReference(
                "Bearer", document)] = []
        });
});

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middlewware Configutaation

app.UseMiddleware<ExceptionHandlingMiddleware>();

// IMPORTANT: Authentication must run before authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

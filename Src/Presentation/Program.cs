using DotNetEnv;
using Data.Database;
using Microsoft.EntityFrameworkCore;
using Application.Services.Interfaces;
using Application.Services;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Interfaces;
using Data.Database.UnitOfWorks;
using Domain.Tokens;
using Domain.PasswordManagers;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
Env.Load();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<ITokenManager, JwtTokenManager>(
    provider =>
    {
        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ??
            throw new InvalidOperationException("JWT secret key not found in environment variables.");
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "TaskManagementSystem";
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "TaskManagementSystem";
        var expirationTime = DateTime.UtcNow.AddHours(int.Parse(
            Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS") ?? "1"
        ));
        return new JwtTokenManager(jwtSecret, jwtIssuer, jwtAudience, expirationTime);
    }
);
builder.Services.AddSingleton<IPasswordManager, BCryptPasswordManager>();

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JWT Authentication
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ??
    throw new InvalidOperationException("JWT secret key not found in environment variables.");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "TaskManagementSystem";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "TaskManagementSystem";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.Zero
    };
});

// Configure Database
builder.Services.AddDbContext<TaskManagementSystemDBContext>(options =>
    options.UseNpgsql(
        Environment.GetEnvironmentVariable("DB_CONNECTION") ??
        throw new InvalidOperationException("Database connection string not found.")
    )
);
// Configure logging
builder.Logging.ClearProviders(); // Optional: Remove default providers
builder.Logging.AddConsole();     // Add console logging
builder.Logging.AddDebug();       // Add debug output


// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(
);

// Register Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITaskService, TaskService>();


// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global error handling
app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Add authentication & authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
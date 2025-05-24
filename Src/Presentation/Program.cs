using DotNetEnv;
using Data.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
Env.Load();

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure Database
builder.Services.AddDbContext<TaskManagementSystemDBContext>(options =>
    options.UseNpgsql(
        Environment.GetEnvironmentVariable("DB_CONNECTION") ??
        throw new InvalidOperationException("Database connection string not found.")
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
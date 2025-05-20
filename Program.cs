using Microsoft.EntityFrameworkCore;
using SurveyApi.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan koneksi ke SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Tambahkan CORS dengan policy khusus
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhosts",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:3001",
                "http://localhost:3002"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

// Add services to the container, with JSON options for reference handling
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.MaxDepth = 64;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Pakai CORS sebelum routing dan authorization
app.UseCors("AllowLocalhosts");

app.UseAuthorization();

app.MapControllers();

app.Run();

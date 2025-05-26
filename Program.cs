using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SurveyApi.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// DB Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhosts", policy =>
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

// Controller + JSON options
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

// Pastikan folder uploads dalam wwwroot tersedia
var uploadsFolder = Path.Combine(app.Environment.WebRootPath, "uploads");
if (!Directory.Exists(uploadsFolder))
{
    Directory.CreateDirectory(uploadsFolder);
}

// Serve static files (wwwroot termasuk /uploads)
app.UseStaticFiles();

// Serve /uploads path secara eksplisit jika perlu
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsFolder),
    RequestPath = "/uploads"
});

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowLocalhosts");

app.UseAuthorization();

app.MapControllers();

app.Run();

using FinanceTracker.API.Endpoints;
using FinanceTracker.API.Middleware;
using FinanceTracker.Application.Mapping;
using FinanceTracker.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure (EF Core + repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// MediatR — scan Application assembly
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

// OpenAPI
builder.Services.AddOpenApi();

// CORS (allow Angular dev server)
builder.Services.AddCors(options =>
    options.AddPolicy("DevCors", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()));

// Auth (JWT added in Day 6 — placeholder)
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("DevCors");
app.UseAuthorization();

// Global exception handler
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Health check
app.MapGet("/healthz", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
   .WithTags("Health");

// Endpoint groups
app.MapTransactionEndpoints();
app.MapAccountEndpoints();

app.Run();

// Needed for WebApplicationFactory in integration tests
public partial class Program;

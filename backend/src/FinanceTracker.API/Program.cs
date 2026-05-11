using System.Text;
using FinanceTracker.API.Endpoints;
using FinanceTracker.API.Middleware;
using FinanceTracker.Application.Common.Behaviors;
using FinanceTracker.Application.Mapping;
using FinanceTracker.Infrastructure.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
    options.AddPolicy("DevCors", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("DevCors");
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapGet("/healthz", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
   .WithTags("Health");

app.MapAuthEndpoints();
app.MapTransactionEndpoints();
app.MapAccountEndpoints();

app.Run();

public partial class Program;

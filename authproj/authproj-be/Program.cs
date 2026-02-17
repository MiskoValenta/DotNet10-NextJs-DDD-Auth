
using Application.Interfaces;
using Application.Services;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace authproj_be;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    // 1. Database connection: PostgreSQL
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

    // 2. CORS - for communication with Next.js and posting cookies
    builder.Services.AddCors(options =>
    {
      options.AddPolicy("FrontendPolicy", policy =>
      {
        // IP Address from Frontend
        policy.WithOrigins("http://localhost:3000", "http://192.168.56.1:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
      });
    });

    // 3. DI Registration
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
    builder.Services.AddScoped<IJwtProvider, JwtProvider>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    builder.Services.AddControllers();

    // 4. Configure JWT Authetization
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero
          };

          // Recieve tokens from cookies
          options.Events = new JwtBearerEvents
          {
            OnMessageReceived = context =>
            {
              context.Token = context.Request.Cookies["accessToken"];
              return Task.CompletedTask;
            }
          };
        });

    builder.Services.AddAuthorization();

    var app = builder.Build();

    app.UseCors("FrontendPolicy"); // Must be infront of Auth
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
  }
}

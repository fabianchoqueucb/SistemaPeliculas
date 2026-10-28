using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaPeliculas.Api.Filters;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Core.Services;
using SistemaPeliculas.Infrastructure.Data;
using SistemaPeliculas.Infrastructure.Mappings;
using SistemaPeliculas.Infrastructure.Repositories;
using SistemaPeliculas.Infrastructure.Services;

namespace SistemaPeliculas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Base de datos MySQL
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<PeliculasContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));
            #endregion

            #region Repositorios y servicios
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<ISecurityService, SecurityService>();
            builder.Services.AddSingleton<IPasswordService, PasswordService>();
            #endregion

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            #region Autenticación JWT (CU-01)
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]!))
                    };

                    // Respuestas 401 y 403 con el mismo formato JSON que el resto de errores
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                status = 401,
                                title = "No autorizado",
                                detail = "Debes iniciar sesión y enviar el token en el header Authorization: Bearer <token>"
                            }));
                        },
                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                status = 403,
                                title = "Prohibido",
                                detail = "Tu rol no tiene permiso para esta acción"
                            }));
                        }
                    };
                });
            builder.Services.AddAuthorization();
            #endregion

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // ¿quién eres? (lee y valida el token)
            app.UseAuthorization();  // ¿puedes hacer esto? (revisa [Authorize] y el rol)

            app.MapControllers();

            app.Run();
        }
    }
}

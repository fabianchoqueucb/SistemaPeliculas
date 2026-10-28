using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Api.Filters;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Core.Services;
using SistemaPeliculas.Infrastructure.Data;
using SistemaPeliculas.Infrastructure.Mappings;
using SistemaPeliculas.Infrastructure.Repositories;

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
            #endregion

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

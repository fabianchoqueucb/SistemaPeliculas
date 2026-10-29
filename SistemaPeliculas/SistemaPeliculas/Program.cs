using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Infrastructure.Data;
using SistemaPeliculas.Infrastructure.Mappings;
using SistemaPeliculas.Infrastructure.Repositories;
using SistemaPeliculas.Services.Interfaces;
using SistemaPeliculas.Services.Services;
using SistemaPeliculas.Services.Validators;

namespace SistemaPeliculas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configurar la BD MySql
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<PeliculasContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            #endregion

            // Add services to the container.
            builder.Services.AddTransient<IMovieRepository, MovieRepository>();
            builder.Services.AddTransient<IReviewRepository, ReviewRepository>();
            builder.Services.AddTransient<ICommentRepository, CommentRepository>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IMovieService, MovieService>();
            builder.Services.AddTransient<IReviewService, ReviewService>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Registrar Validadores de FluentValidator
            builder.Services.AddScoped<MovieDtoValidator>();
            builder.Services.AddScoped<CrearMovieDtoValidator>();
            builder.Services.AddScoped<ActualizarMovieDtoValidator>();
            builder.Services.AddScoped<CrearReviewDtoValidator>();

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

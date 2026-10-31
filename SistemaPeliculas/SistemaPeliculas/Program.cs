using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Api.Filters;
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

            // Inyección de dependencias - Repositorio Genérico y Específicos
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddTransient<IMovieRepository, MovieRepository>();
            builder.Services.AddTransient<IReviewRepository, ReviewRepository>();
            builder.Services.AddTransient<ICommentRepository, CommentRepository>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IActorRepository, ActorRepository>();
            builder.Services.AddTransient<IWatchlistRepository, WatchlistRepository>();

            // Inyección de dependencias - Capa de Servicios
            builder.Services.AddTransient<IMovieService, MovieService>();
            builder.Services.AddTransient<IReviewService, ReviewService>();
            builder.Services.AddTransient<IActorService, ActorService>();
            builder.Services.AddTransient<IWatchlistService, WatchlistService>();

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Registrar Validadores de FluentValidator
            builder.Services.AddScoped<MovieDtoValidator>();
            builder.Services.AddScoped<CrearMovieDtoValidator>();
            builder.Services.AddScoped<ActualizarMovieDtoValidator>();
            builder.Services.AddScoped<CrearReviewDtoValidator>();
            builder.Services.AddScoped<CrearActorDtoValidator>();
            builder.Services.AddScoped<ActualizarActorDtoValidator>();
            builder.Services.AddScoped<CrearWatchlistDtoValidator>();

            var app = builder.Build();

            // Middleware global de manejo de excepciones
            app.UseMiddleware<ExceptionHandlingMiddleware>();

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

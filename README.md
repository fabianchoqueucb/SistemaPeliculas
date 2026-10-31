# SistemaPeliculas

API REST en ASP.NET Core (.NET 10) + MySQL para una plataforma de reseñas de películas, desarrollada bajo arquitectura limpia en 4 capas según los patrones y nivel avanzado del docente (Evaluación 2).

## Arquitectura por Capas

- **SistemaPeliculas.Api** – Controladores REST (`MovieController`, `ReviewController`, `ActorsController`, `WatchlistsController`, `CommentController`), respuestas estándar `ApiResponse<T>`, Middleware Global `ExceptionHandlingMiddleware`, OpenAPI y configuración en `Program.cs`.
- **SistemaPeliculas.Core** – Entidades del dominio (`BaseEntity`, `Movie`, `Review`, `Comment`, `User`, `Actor`, `Watchlist`), entidades de error (`ErrorResponse`), excepciones personalizadas (`BusinessException`), DTOs e interfaces de repositorios (`IBaseRepository<T>`, `IMovieRepository`, `IReviewRepository`, `ICommentRepository`, `IUserRepository`, `IActorRepository`, `IWatchlistRepository`).
- **SistemaPeliculas.Infrastructure** – Persistencia con Entity Framework Core 9 + Pomelo MySQL (`PeliculasContext`), configuraciones Fluent API (`IEntityTypeConfiguration<T>`), mapeos de AutoMapper y repositorios (`BaseRepository<T>`, etc.).
- **SistemaPeliculas.Services** – Capa de lógica de negocio (`MovieService`, `ReviewService`, `ActorService`, `WatchlistService`) y validación de DTOs con **FluentValidation** (`CrearMovieDtoValidator`, `ActualizarMovieDtoValidator`, `CrearReviewDtoValidator`, `CrearActorDtoValidator`, `ActualizarActorDtoValidator`, `CrearWatchlistDtoValidator`).

Flujo: Cliente (HTTP) → ExceptionHandlingMiddleware → Controlador → Servicio / Validadores → Repositorio → DbContext → MySQL.

## Requisitos

- .NET SDK 10
- Servidor MySQL 8

## Instalación y Ejecución

1. Ejecutar el script `Database/DbPeliculas.sql` en MySQL Workbench para crear la base `DbPeliculas`, tablas (`user`, `movie`, `review`, `comment`, `actor`, `watchlist`), datos semilla y el usuario de conexión `peliculas`.
2. Compilar la solución:
   ```bash
   dotnet build SistemaPeliculas/SistemaPeliculas.slnx
   ```
3. Ejecutar la API:
   ```bash
   dotnet run --project SistemaPeliculas/SistemaPeliculas/SistemaPeliculas.Api.csproj
   ```
4. Probar las peticiones directamente con `SistemaPeliculas/SistemaPeliculas/SistemaPeliculas.http` o importando la colección en Postman (`Postman/SistemaPeliculas.postman_collection.json`).

## Endpoints Principales (Evaluación 2)

| Recurso | Método | Endpoint | Descripción |
|---|---|---|---|
| **Películas** | `GET` | `/api/movie` | Listar catálogo de películas |
| **Películas** | `GET` | `/api/movie/{id}` | Consultar película por ID |
| **Películas** | `POST` | `/api/movie` | Registrar película (con validaciones de negocio) |
| **Películas** | `PUT` | `/api/movie/{id}` | Actualizar película |
| **Películas** | `DELETE` | `/api/movie/{id}` | Eliminar película |
| **Reseñas** | `GET` | `/api/review` | Listar todas las reseñas |
| **Reseñas** | `GET` | `/api/review/{id}` | Consultar reseña por ID |
| **Reseñas** | `GET` | `/api/review/movie/{movieId}` | Consultar reseñas de una película |
| **Reseñas** | `POST` | `/api/review` | Registrar reseña (valida usuario, película y palabras prohibidas) |
| **Reseñas** | `DELETE` | `/api/review/{id}` | Eliminar reseña |
| **Elenco (Actores)** | `GET` | `/api/actors` | Listar actores (admite filtros `?name=` y `?movieId=`) |
| **Elenco (Actores)** | `GET` | `/api/actors/{id}` | Consultar biografía y datos de un actor específico |
| **Elenco (Actores)** | `POST` | `/api/actors` | Registrar nuevo actor asociado a película |
| **Elenco (Actores)** | `PUT` | `/api/actors/{id}` | Actualizar datos del actor (sin modificar ID) |
| **Seguimiento (Watchlists)**| `GET` | `/api/watchlists/user/{userId}` | Recuperar catálogo personalizado de un usuario |
| **Seguimiento (Watchlists)**| `POST` | `/api/watchlists` | Vincular MovieId con UserId (bloquea duplicados) |
| **Seguimiento (Watchlists)**| `DELETE`| `/api/watchlists/{id}` | Quitar película de la lista de seguimiento personal |
| **Comentarios** | `GET` | `/api/comment` | Listar comentarios |
| **Comentarios** | `POST` | `/api/comment` | Registrar comentario |

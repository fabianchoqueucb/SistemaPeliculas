# SistemaPeliculas

API REST (ASP.NET Core .NET 10 + MySQL) para una plataforma de reseñas de películas: un moderador sube películas al catálogo y los usuarios publican reseñas con calificación de 1 a 10.

## Arquitectura

- **SistemaPeliculas.Api** – controladores, filtro global de errores, `Program.cs`.
- **SistemaPeliculas.Core** – entidades, DTOs, interfaces y servicios con las reglas de negocio.
- **SistemaPeliculas.Infrastructure** – `PeliculasContext` (EF Core + Pomelo MySQL), configuraciones, repositorios y perfil de AutoMapper.

Flujo: Cliente (Postman) → Controlador → Servicio → Repositorio → DbContext → MySQL.

## Requisitos

- .NET SDK 10
- MySQL 8

## Instalación

1. Ejecutar `Database/DbPeliculas.sql` en MySQL Workbench. Crea la base `DbPeliculas`, las tablas, datos de prueba y el usuario de conexión `peliculas`.
2. Ejecutar la API:
   ```
   cd SistemaPeliculas
   dotnet run --project SistemaPeliculas --launch-profile http
   ```
3. Probar con `SistemaPeliculas/SistemaPeliculas.http` o importando `Postman/SistemaPeliculas.postman_collection.json` (base_url = http://localhost:5093).

## Casos de uso implementados (Evaluación 1)

| CU | Endpoint |
|---|---|
| CU-02 Subir Película | `POST /api/movies` |
| CU-03 Subir Reseña | `POST /api/reviews` |

También: `GET /api/movies`, `GET /api/movies/{id}`, `GET /api/reviews`, `GET /api/reviews/{id}`.

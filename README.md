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

## Usuarios de prueba

| Rol | Correo | Contraseña |
|---|---|---|
| Moderador | ana.moderadora@peliculas.com | Moderador.2026 |
| Usuario | lucia@mail.com / carlos@mail.com | Usuario.2026 |
| Usuario inactivo | pedro@mail.com | Usuario.2026 |

## Casos de uso implementados

| CU | Endpoint | Acceso | Revisión |
|---|---|---|---|
| CU-01 Iniciar Sesión | `POST /api/auth/login` | Público | 2 |
| CU-02 Subir Película | `POST /api/movies` | Moderador (token) | 1 |
| CU-03 Subir Reseña | `POST /api/reviews` | Usuario con sesión (token) | 1 |
| CU-04 Consultar Catálogo | `GET /api/movies?title=&genre=&year=` y `GET /api/movies/{id}` | Público | 2 |
| CU-05 Consultar Reseñas de una Película | `GET /api/movies/{id}/reviews` | Público | 2 |

También: `GET /api/reviews`, `GET /api/reviews/{id}`.

## Probar con Postman

1. Importar `Postman/SistemaPeliculas.postman_collection.json` y `Postman/SistemaPeliculas.Desarrollo.postman_environment.json`.
2. Elegir el entorno **SistemaPeliculas - Desarrollo**.
3. Ejecutar primero la carpeta **Auth**: los logins guardan los tokens en `{{token_moderador}}` y `{{token_usuario}}`.

# SistemaPeliculas

API REST (ASP.NET Core .NET 10 + MySQL) para una plataforma de películas: permite gestionar el catálogo de películas, las reseñas de los usuarios (calificación de 1 a 10), el elenco de actores y la lista de seguimiento (favoritos) de cada usuario.

## Arquitectura

Solución en capas con cuatro proyectos:

- **SistemaPeliculas.Api** – controladores, middleware global de errores (`ExceptionHandlingMiddleware`) y `Program.cs` (configuración e inyección de dependencias).
- **SistemaPeliculas.Core** – entidades, DTOs, interfaces de repositorios y excepciones de negocio.
- **SistemaPeliculas.Services** – servicios con las reglas de negocio y validadores (FluentValidation).
- **SistemaPeliculas.Infrastructure** – `PeliculasContext` (EF Core + Pomelo MySQL), configuraciones de las tablas, repositorios y perfil de AutoMapper.

Flujo: Cliente → Controlador → Servicio → Repositorio → DbContext → MySQL.

## Requisitos

- .NET SDK 10
- MySQL 8

## Instalación

1. Ejecutar `Database/DbPeliculas.sql` en MySQL Workbench. Crea la base `DbPeliculas`, las tablas (`user`, `movie`, `review`, `comment`, `actor`, `watchlist`), datos de prueba y el usuario de conexión `peliculas`.
2. Verificar la cadena de conexión en `SistemaPeliculas/SistemaPeliculas/appsettings.json`. Debe coincidir con el usuario creado por el script:
   ```json
   "ConnectionStrings": {
     "ConnectionMySql": "server=localhost;port=3306;database=DbPeliculas;uid=peliculas;pwd=Peliculas.2026;"
   }
   ```
   Si tu MySQL usa otro puerto, otro usuario u otra contraseña, ajústalos aquí.
3. Ejecutar la API:
   ```
   cd SistemaPeliculas
   dotnet run --project SistemaPeliculas --launch-profile http
   ```
   La API queda en `http://localhost:5093`.

### Si no conecta a la base de datos

La API abre una conexión al arrancar, así que cualquier error de conexión la detiene de inmediato. Revisar:

- Que el servicio de MySQL esté corriendo y que el puerto sea el de la cadena de conexión.
- Que usuario y contraseña de `appsettings.json` sean los mismos que creó el script (`peliculas` / `Peliculas.2026`).
- Que el script SQL se haya ejecutado completo (la base `DbPeliculas` y todas las tablas deben existir).

## Datos de prueba

| Id | Usuario | Estado |
|---|---|---|
| 1 | Ana Rojas (ana.moderadora@peliculas.com) | Activo |
| 2 | Carlos Mendoza (carlos@mail.com) | Activo |
| 3 | Lucía Vargas (lucia@mail.com) | Activo |
| 4 | Pedro Flores (pedro@mail.com) | Inactivo |

La API todavía no implementa inicio de sesión ni roles: los endpoints no requieren token y el usuario se indica con `userId` en las peticiones.

## Casos de uso

### Gestionar película

| Operación | Endpoint |
|---|---|
| Crear | `POST /api/movie` |
| Consultar catálogo | `GET /api/movie?title=&genre=` |
| Consultar por Id | `GET /api/movie/{id}` |
| Actualizar | `PUT /api/movie/{id}` |
| Eliminar | `DELETE /api/movie/{id}` |

Reglas: el título no puede repetirse, la fecha de estreno no puede ser anterior al 28/12/1895 y la descripción no puede contener palabras prohibidas (estas dos últimas se validan al crear).

### Gestionar reseña

| Operación | Endpoint |
|---|---|
| Crear | `POST /api/review` |
| Consultar todas | `GET /api/review` |
| Consultar por Id | `GET /api/review/{id}` |
| Consultar reseñas de una película | `GET /api/review/movie/{movieId}` |
| Actualizar | `PUT /api/review/{id}` |
| Eliminar | `DELETE /api/review/{id}` |

Reglas: el usuario y la película deben existir, la película debe estar ya estrenada, la calificación va de 1 a 10, el texto no puede tener palabras inapropiadas y un usuario solo puede reseñar una vez cada película. Al actualizar, solo el autor puede editar la reseña y únicamente cambian la calificación y el texto.

### Gestionar elenco de actores

| Operación | Endpoint |
|---|---|
| Registrar actor | `POST /api/actors` |
| Consultar actores | `GET /api/actors?name=&movieId=` |
| Consultar por Id | `GET /api/actors/{id}` |
| Actualizar | `PUT /api/actors/{id}` |
| Eliminar | `DELETE /api/actors/{id}` |

Reglas: si se asocia el actor a una película (`movieId`), esa película debe existir. Si se elimina una película, sus actores se conservan con `movieId` vacío.

### Gestionar lista de seguimiento (favoritos)

| Operación | Endpoint |
|---|---|
| Agregar película | `POST /api/watchlists` |
| Consultar la lista de un usuario | `GET /api/watchlists/user/{userId}` |
| Quitar película | `DELETE /api/watchlists/{id}` |

Reglas: el usuario y la película deben existir y una película no puede estar repetida en la lista del mismo usuario.

### Comentarios

`/api/comment` expone listar, consultar, crear, actualizar y eliminar comentarios de reseñas, sin capa de servicio ni validaciones.

## Respuestas y errores

Las respuestas correctas se devuelven dentro de un objeto `ApiResponse`. Los errores (validación, reglas de negocio, recurso inexistente) los captura el middleware global y se devuelven en JSON con `status`, `title`, `message`, `errors` y `traceId`.

## Probar la API

Usar el archivo `SistemaPeliculas/SistemaPeliculas/SistemaPeliculas.http` (Visual Studio o la extensión REST Client de VS Code), que tiene ejemplos de todos los endpoints.

> La colección de Postman de la carpeta `Postman/` corresponde a un diseño anterior (con `/api/auth/login` y rutas en plural como `/api/movies`) y no coincide con la API actual.

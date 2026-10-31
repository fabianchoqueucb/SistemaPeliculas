-- =====================================================================
-- Sistema de Reseñas de Películas - Script de base de datos (MySQL 8)
-- Crea la base, las tablas, el usuario de conexión y datos de prueba.
-- =====================================================================

SET NAMES utf8mb4;

DROP DATABASE IF EXISTS DbPeliculas;
CREATE DATABASE DbPeliculas CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE DbPeliculas;

-- ---------------------------------------------------------------------
-- Tablas
-- ---------------------------------------------------------------------
CREATE TABLE `user` (
  Id          INT          NOT NULL AUTO_INCREMENT,
  FirstName   VARCHAR(50)  NOT NULL,
  LastName    VARCHAR(50)  NOT NULL,
  Email       VARCHAR(100) NOT NULL,
  Password    VARCHAR(200) NOT NULL,           -- hash PBKDF2: iteraciones.sal.hash
  DateOfBirth DATE         NOT NULL,
  Telephone   VARCHAR(15)  NULL,
  Role        VARCHAR(20)  NOT NULL,           -- Moderador | Usuario
  IsActive    TINYINT(1)   NOT NULL DEFAULT 1,
  PRIMARY KEY (Id),
  UNIQUE KEY UK_User_Email (Email)
);

CREATE TABLE movie (
  Id          INT           NOT NULL AUTO_INCREMENT,
  Title       VARCHAR(100)  NOT NULL,
  Description VARCHAR(1000) NOT NULL,
  ReleaseDate DATE          NOT NULL,
  Length      VARCHAR(10)   NOT NULL,          -- formato "#h #m"
  Genre       VARCHAR(30)   NOT NULL,
  PRIMARY KEY (Id),
  UNIQUE KEY UK_Movie_Title (Title)
);

CREATE TABLE review (
  Id          INT           NOT NULL AUTO_INCREMENT,
  MovieId     INT           NULL,
  UserId      INT           NULL,
  Description VARCHAR(1000) NULL,
  Date        DATETIME      NOT NULL,
  Grade       FLOAT         NOT NULL,          -- de 1 a 10
  PRIMARY KEY (Id),
  CONSTRAINT FK_Review_Movie FOREIGN KEY (MovieId) REFERENCES movie(Id) ON DELETE SET NULL,
  CONSTRAINT FK_Review_User  FOREIGN KEY (UserId)  REFERENCES `user`(Id) ON DELETE SET NULL
);

CREATE TABLE comment (
  Id          INT          NOT NULL AUTO_INCREMENT,
  ReviewId    INT          NULL,
  UserId      INT          NULL,
  Description VARCHAR(500) NOT NULL,
  Date        DATETIME     NOT NULL,
  IsActive    TINYINT(1)   NOT NULL DEFAULT 1,
  PRIMARY KEY (Id),
  CONSTRAINT FK_Comment_Review FOREIGN KEY (ReviewId) REFERENCES review(Id) ON DELETE SET NULL,
  CONSTRAINT FK_Comment_User   FOREIGN KEY (UserId)   REFERENCES `user`(Id) ON DELETE SET NULL
);

CREATE TABLE actor (
  Id          INT          NOT NULL AUTO_INCREMENT,
  MovieId     INT          NULL,
  FirstName   VARCHAR(100) NOT NULL,
  LastName    VARCHAR(100) NOT NULL,
  Email       VARCHAR(100) NOT NULL,
  DateOfBirth DATE         NOT NULL,
  IsActive    TINYINT(1)   NOT NULL DEFAULT 1,
  PRIMARY KEY (Id),
  CONSTRAINT FK_Actor_Movie FOREIGN KEY (MovieId) REFERENCES movie(Id) ON DELETE SET NULL
);

CREATE TABLE watchlist (
  Id        INT      NOT NULL AUTO_INCREMENT,
  UserId    INT      NOT NULL,
  MovieId   INT      NOT NULL,
  DateAdded DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (Id),
  UNIQUE KEY UK_Watchlist_User_Movie (UserId, MovieId),
  CONSTRAINT FK_Watchlist_User  FOREIGN KEY (UserId)  REFERENCES `user`(Id) ON DELETE CASCADE,
  CONSTRAINT FK_Watchlist_Movie FOREIGN KEY (MovieId) REFERENCES movie(Id)   ON DELETE CASCADE
);

-- ---------------------------------------------------------------------
-- Datos semilla (al menos un usuario por rol)
-- Contraseñas de prueba: moderador = Moderador.2026 | usuarios = Usuario.2026
-- ---------------------------------------------------------------------
INSERT INTO `user` (FirstName, LastName, Email, Password, DateOfBirth, Telephone, Role, IsActive) VALUES
('Ana',    'Rojas',   'ana.moderadora@peliculas.com', '10000.8vj/giGfKYYa7k/yLyTcfw==.J0BOf6t71iQuSB+u3YYN6UfS8amyAVnTKAJWq5pklFU=', '1990-04-12', '70011122', 'Moderador', 1),
('Carlos', 'Mendoza', 'carlos@mail.com', '10000.CB3clV+2Oda76lR8ejA72A==.aPFOsz8jy8+ybItayFnv60sb//9gk9WFkaf3FH735qE=', '1998-07-22', '70033344', 'Usuario',   1),
('Lucía',  'Vargas',  'lucia@mail.com', '10000.CB3clV+2Oda76lR8ejA72A==.aPFOsz8jy8+ybItayFnv60sb//9gk9WFkaf3FH735qE=', '2001-11-08', NULL,       'Usuario',   1),
('Pedro',  'Flores',  'pedro@mail.com', '10000.CB3clV+2Oda76lR8ejA72A==.aPFOsz8jy8+ybItayFnv60sb//9gk9WFkaf3FH735qE=', '1995-02-17', '70055566', 'Usuario',   0);

INSERT INTO movie (Title, Description, ReleaseDate, Length, Genre) VALUES
('El Padrino',          'La historia de la familia Corleone y su imperio criminal en Nueva York.', '1972-03-24', '2h 55m', 'Drama'),
('Toy Story',           'Los juguetes de Andy cobran vida cuando nadie los está mirando.',          '1995-11-22', '1h 21m', 'Animación'),
('Parásitos',           'Una familia pobre se infiltra poco a poco en la vida de una familia rica.', '2019-05-30', '2h 12m', 'Suspenso'),
('Estreno Próximo 2027','Película anunciada que todavía no se estrenó.',                             '2027-06-15', '2h 0m',  'Aventura');

INSERT INTO review (MovieId, UserId, Description, Date, Grade) VALUES
(1, 2, 'Una obra maestra, las actuaciones son impecables.', '2026-10-01 18:30:00', 10),
(2, 3, 'Divertida para toda la familia.',                  '2026-10-05 20:15:00', 8),
(3, 2, 'Muy buena, aunque el final es inquietante.',       '2026-10-10 21:00:00', 9);

INSERT INTO comment (ReviewId, UserId, Description, Date, IsActive) VALUES
(1, 3, 'Totalmente de acuerdo, un clásico.', '2026-10-02 10:00:00', 1);

INSERT INTO actor (Id, MovieId, FirstName, LastName, Email, DateOfBirth, IsActive) VALUES
(1, 1, 'Marlon', 'Brando', 'marlon.brando@hollywood.com', '1924-04-03', 1),
(2, 1, 'Al',     'Pacino', 'al.pacino@hollywood.com',      '1940-04-25', 1),
(3, 2, 'Tom',    'Hanks',  'tom.hanks@hollywood.com',       '1956-07-09', 1),
(4, 3, 'Song',   'Kang-ho','song.kangho@cinema.kr',         '1967-01-17', 1);

INSERT INTO watchlist (Id, UserId, MovieId, DateAdded) VALUES
(1, 2, 1, '2026-10-20 14:00:00'),
(2, 2, 2, '2026-10-21 16:30:00'),
(3, 3, 3, '2026-10-22 19:15:00');

-- ---------------------------------------------------------------------
-- Usuario de conexión para la API (el mismo en la PC de todo el grupo)
-- ---------------------------------------------------------------------
CREATE USER IF NOT EXISTS 'peliculas'@'localhost' IDENTIFIED BY 'Peliculas.2026';
GRANT ALL PRIVILEGES ON DbPeliculas.* TO 'peliculas'@'localhost';
FLUSH PRIVILEGES;

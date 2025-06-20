-- Script para crear la base de datos y tabla de eventos
-- Ejecutar este script en MySQL para configurar la base de datos

-- Crear la base de datos si no existe
CREATE DATABASE IF NOT EXISTS TodoAppDb;

-- Usar la base de datos
USE TodoAppDb;

-- Crear la tabla Events
CREATE TABLE IF NOT EXISTS `Events` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(100) NOT NULL,
    `DateTime` datetime(6) NOT NULL,
    `Location` varchar(200) NOT NULL,
    `Description` varchar(1000) NULL,
    `Status` varchar(20) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

-- Insertar datos de ejemplo
INSERT INTO `Events` (`Id`, `Title`, `DateTime`, `Location`, `Description`, `Status`, `CreatedAt`) VALUES
(1, 'Conferencia de Tecnología 2024', DATE_ADD(NOW(), INTERVAL 7 DAY), 'Centro de Convenciones', 'Una conferencia sobre las últimas tendencias en tecnología', 'Upcoming', NOW()),
(2, 'Meetup de Desarrolladores', DATE_ADD(NOW(), INTERVAL 14 DAY), 'Café Central', 'Networking y charlas sobre desarrollo de software', 'Attending', NOW()),
(3, 'Workshop de Angular', DATE_ADD(NOW(), INTERVAL 21 DAY), 'Universidad Local', 'Taller práctico sobre Angular 17+', 'Maybe', NOW());

-- Verificar que la tabla se creó correctamente
SELECT * FROM `Events`; 
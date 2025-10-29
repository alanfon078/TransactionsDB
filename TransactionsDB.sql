create database if not exists TransactionsDB;
-- drop database TransactionsDB;
USE TransactionsDB;

create table if not exists product(
    id INT AUTO_INCREMENT PRIMARY KEY,
    codigoDeBarras VARCHAR(100) NOT NULL UNIQUE,
    nombre VARCHAR(255) NOT NULL,
    descripcion varchar(50),
    precio DECIMAL(10, 2) NOT NULL,
    stock INT NOT NULL DEFAULT 0,
    descontinuado TINYINT(1) NOT NULL DEFAULT 0 -- 0 = Falso (Activo), 1 = Verdadero (Descontinuado)
);

-- Insertar algunos datos de ejemplo (opcional)
INSERT INTO product (codigoDeBarras, nombre, precio, stock) 
VALUES
('0070847036043', 'Monster Zero Ultra', 38.00, 130),
('7798422620304', 'Monster Ultra Strawberry Dreams', 38.00, 120),
('070847898245', 'Monster Ultra Blue Hawaiian', 48.00, 110),
('0070847894216', 'Monster Ultra Golden Pineapple', 38.00, 100),
('070847034629', 'Monster Ultra Fiesta Mango', 48.00, 90),
('070847027324', 'Monster Ultra Violet', 48.00, 80),
('0070847897637', 'Monster Ultra Peachy Keen', 38.00, 70),
('0070847894230', 'Monster Ultra Watermelon', 38.00, 60),
('0070847036715', 'Monster Ultra Paradise', 38.00, 50),
('070847812609', 'Java Monster Mean Bean', 60.00, 40),
('070847812715', 'Java Monster Loca Moca', 60.00, 30),
('070847898405', 'Monster Killer Brew Triple Shot Mean Bean', 80.00, 20);

select * from product;
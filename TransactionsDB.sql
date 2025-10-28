create database if not exists TransactionsDB;
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
('7501001111111', 'Refresco de Cola 600ml', 18.50, 100),
('7501002222222', 'Papas Fritas Sal 50g', 15.00, 200),
('7501003333333', 'Jabón de Barra Neutro', 12.00, 50);

select * from product;
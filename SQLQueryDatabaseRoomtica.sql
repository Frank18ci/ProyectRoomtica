use master
go

drop database db_roomtica
go

create database db_roomtica
go

use db_roomtica
go

create table caracteristica_habitacion(
	id int identity(1, 1) primary key,
	caracteristica varchar(100),
	estado bit,
)
go

create table tipo_habitacion(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	descripccion varchar(200),
	estado bit,
)
go
create table caracteristica_habitacion_tipo_habitacion(
	id_caracteristica_habitacion int references caracteristica_habitacion,
	id_tipo_habitacion int references tipo_habitacion,
	primary key (id_caracteristica_habitacion, id_tipo_habitacion),
	estado bit,
)
go

create table estado_habitacion(
	id int identity(1, 1) primary key,
	estado_habitacion varchar(40) unique,
	estado bit,
)
go
create table habitacion(
	id int identity(1, 1) primary key,
	numero varchar(5),
	piso varchar(3),
	precio_diario decimal(10,2),
	id_tipo int references tipo_habitacion,
	id_estado int references estado_habitacion,
	estado bit,
)
go
create table rol_trabajador(
	id int identity(1, 1) primary key,
	rol varchar(40) unique,
	estado bit,
)
go
create table tipo_documento(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	estado bit,
)
go
create table tipo_nacionalidad(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	estado bit,
)
go
create table tipo_sexo(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	estado bit,
)
go
create table trabajador(
	id int identity(1, 1) primary key,
	primer_nombre varchar(50),
	segundo_nombre varchar(50),
	primer_apellido varchar(50),
	segundo_apellido varchar(50),
	username varchar(20),
	password varchar(355),
	sueldo decimal(10,2),
	id_tipo_documento int references tipo_documento,
	numero_documento varchar(10) unique,
	telefono varchar(10) unique,
	email varchar(25) unique,
	id_rol int references rol_trabajador,
	estado bit,
)
go
create table cliente(
	id int identity(1, 1) primary key,
	primer_nombre varchar(50),
	segundo_nombre varchar(50),
	primer_apellido varchar(50),
	segundo_apellido varchar(50),
	id_tipo_documento int references tipo_documento,
	numero_documento varchar(10) unique,
	telefono varchar(10) unique,
	email varchar(25) unique,
	fecha_nacimiento date,
	id_tipo_nacionalidad int references tipo_nacionalidad,
	id_tipo_sexo int references tipo_sexo,
	estado bit
)
go
create table tipo_estacionamiento(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	costo decimal(10,2),
	estado bit,
)
go
create table estacionamiento(
	id int identity(1, 1) primary key,
	lugar varchar(10) unique,
	largo varchar(10),
	alto varchar(10),
	ancho varchar(10),
	id_tipo_estacionamiento int references tipo_estacionamiento,
	estado bit
)
go
create table tipo_reserva(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	estado bit,
)
go
create table reserva(
	id int identity(1, 1) primary key,
	id_habitacion int references habitacion,
	id_cliente int references cliente,
	id_trabajador int references trabajador,
	id_tipo_reserva int references tipo_reserva,
	fecha_ingreso date,
	fecha_salida date,
	costo_alojamiento decimal(10,2),
	estado bit,
)
go
create table tipo_comprobante(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	estado bit,
)
go
create table pago(
	id int identity(1, 1) primary key,
	id_reserva int unique references reserva,
	id_tipo_comprobante int references tipo_comprobante,
	igv decimal(10,2),
	total_pago decimal(10,2),
	fecha_emision date,
	fecha_pago date,
	estado bit,
)
go
create table reserva_estacionamiento(
	id_reserva int references reserva,
	id_estacionamiento int references estacionamiento,
	cantidad int,
	precio_estacionamiento decimal(10,2)
	primary key(id_reserva, id_estacionamiento),
	estado bit,
)
go
create table unidad_medida_producto(
	id int identity(1, 1) primary key,
	unidad varchar(40) unique,
	estado bit,
)
go
create table categoria_producto(
	id int identity(1, 1) primary key,
	categoria varchar(40) unique,
	estado bit,
)
go
create table producto(
	id int identity(1, 1) primary key,
	nombre varchar(100) unique,
	id_unidad_medida_producto int references unidad_medida_producto,
	id_categoria_producto int references categoria_producto,
	precio_unico decimal(10,2),
	cantidad int,
	estado bit,
)
go
create table consumo(
	id int identity(1, 1) primary key,
	id_reserva int references reserva,
	id_producto int references producto,
	cantidad int,
	precio_venta decimal(10,2),
	estado bit,
)
go

--Insertando tipo
INSERT INTO caracteristica_habitacion (caracteristica, estado) VALUES
('Vista al mar', 1),
('Aire acondicionado', 1),
('Televisión por cable', 1),
('Caja fuerte', 1),
('Minibar', 1),
('Balcón', 1),
('Cama king size', 1),
('No fumadores', 1),
('Accesible para discapacitados', 1),
('Jacuzzi', 1);

INSERT INTO tipo_habitacion (tipo, descripccion, estado) VALUES
('Individual', 'Habitación para una persona con cama individual.', 1),
('Doble', 'Habitación con dos camas individuales o una doble.', 1),
('Suite', 'Habitación amplia con sala y comodidades extras.', 1),
('Familiar', 'Habitación para familias, con varias camas.', 1),
('Económica', 'Habitación básica con servicios mínimos.', 1),
('Premium', 'Habitación de lujo con servicios exclusivos.', 1);


INSERT INTO caracteristica_habitacion_tipo_habitacion (id_caracteristica_habitacion, id_tipo_habitacion, estado) VALUES
(1, 1, 1),
(2, 1, 1),
(3, 2, 1),
(4, 2, 1),
(5, 3, 1),
(6, 3, 1),
(7, 4, 1),
(8, 4, 1),
(9, 1, 1),
(10, 2, 1);

INSERT INTO estado_habitacion (estado_habitacion, estado) VALUES
('Disponible', 1),
('Ocupada', 1),
('Mantenimiento', 1),
('Reservada', 1);

INSERT INTO rol_trabajador (rol, estado) VALUES
('Recepcionista', 1),
('Administrador', 1),
('Housekeeping', 1),
('Seguridad', 1);

INSERT INTO tipo_documento (tipo, estado) VALUES
('DNI', 1),
('Pasaporte', 1),
('Carnet de Extranjería', 1);

INSERT INTO tipo_nacionalidad (tipo, estado) VALUES
('Peruana', 1),
('Argentina', 1),
('Chilena', 1),
('Colombiana', 1);

INSERT INTO tipo_sexo (tipo, estado) VALUES
('Masculino', 1),
('Femenino', 1),
('Otro', 1);

INSERT INTO tipo_estacionamiento (tipo, costo, estado) VALUES
('Cubierto', 20.00, 1),
('Descubierto', 10.00, 1);

INSERT INTO tipo_reserva (tipo, estado) VALUES
('Online', 1),
('Presencial', 1);

INSERT INTO tipo_comprobante (tipo, estado) VALUES
('Boleta', 1),
('Factura', 1),
('Recibo', 1);

INSERT INTO unidad_medida_producto (unidad, estado) VALUES
('Unidad', 1),
('Litro', 1),
('Gramo', 1),
('Caja', 1);

INSERT INTO categoria_producto (categoria, estado) VALUES
('Bebidas', 1),
('Snacks', 1),
('Higiene', 1),
('Souvenirs', 1);
go

------------------------------------
------------------------------------

create or alter proc usp_listar_tipo_habitacion
as
	select * from tipo_habitacion
go

------------------------------------
------------------------------------

create or alter proc usp_listar_caracteristica_habitacion
as
	select * from caracteristica_habitacion
go

------------------------------------
------------------------------------

create or alter proc usp_listar_caracteristica_habitacion_tipo_habitacion
as
    select * from caracteristica_habitacion_tipo_habitacion
go

------------------------------------
------------------------------------

create or alter proc usp_listar_estado_habitacion
as
    select * from estado_habitacion
go

------------------------------------
------------------------------------

create or alter proc usp_listar_rol_trabajador
as
    select * from rol_trabajador
go
CREATE or alter proc usp_insertar_rol_trabajador
    @rol NVARCHAR(100),
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO rol_trabajador(rol, estado)
    VALUES (@rol, @estado);
    SELECT SCOPE_IDENTITY() AS Id;
END
go
CREATE or alter proc usp_actualizar_rol_trabajador
    @id INT,
    @rol NVARCHAR(100),
    @estado BIT
AS
BEGIN
    UPDATE rol_trabajador
    SET
        rol = @rol,
        estado = @estado
    WHERE id = @id;
END
go
CREATE or alter proc usp_eliminar_rol_trabajador
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE rol_trabajador
    SET estado = 0 
    WHERE Id = @id;
END
go
------------------------------------
------------------------------------

create or alter proc usp_listar_tipo_documento
as
    select * from tipo_documento
go
CREATE or alter proc usp_insertar_tipo_documento
    @tipo NVARCHAR(100),
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tipo_documento(tipo, estado)
    VALUES (@tipo, @estado);
    SELECT SCOPE_IDENTITY() AS Id;
END
go
CREATE or alter proc usp_actualizar_tipo_documento
    @id INT,
    @tipo NVARCHAR(100),
    @estado BIT
AS
BEGIN
    UPDATE tipo_documento
    SET
        tipo = @tipo,
        estado = @estado
    WHERE id = @id;
END
go
CREATE or alter proc usp_eliminar_tipo_documento
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tipo_documento
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------

create or alter proc usp_listar_tipo_nacionalidad
as
    select * from tipo_nacionalidad
go
CREATE or alter proc usp_insertar_tipo_nacionalidad
    @tipo NVARCHAR(100),
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tipo_nacionalidad(tipo, estado)
    VALUES (@tipo, @estado);
    SELECT SCOPE_IDENTITY() AS Id;
END
go
CREATE or alter proc usp_actualizar_tipo_nacionalidad
    @id INT,
    @tipo NVARCHAR(100),
    @estado BIT
AS
BEGIN
    UPDATE tipo_nacionalidad
    SET
        tipo = @tipo,
        estado = @estado
    WHERE id = @id;
END
go
CREATE or alter proc usp_eliminar_tipo_nacionalidad
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tipo_nacionalidad
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------

create or alter proc usp_listar_tipo_sexo
as
    select * from tipo_sexo
go
go
CREATE or alter proc usp_insertar_tipo_sexo
    @tipo NVARCHAR(100),
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tipo_sexo(tipo, estado)
    VALUES (@tipo, @estado);
    SELECT SCOPE_IDENTITY() AS Id;
END
go
CREATE or alter proc usp_actualizar_tipo_sexo
    @id INT,
    @tipo NVARCHAR(100),
    @estado BIT
AS
BEGIN
    UPDATE tipo_sexo
    SET
        tipo = @tipo,
        estado = @estado
    WHERE id = @id;
END
go
CREATE or alter proc usp_eliminar_tipo_sexo
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tipo_sexo
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------

create or alter proc usp_listar_tipo_estacionamiento
as
    select * from tipo_estacionamiento
go

------------------------------------
------------------------------------

create or alter proc usp_listar_tipo_reserva
as
    select * from tipo_reserva
go

------------------------------------
------------------------------------

create or alter proc usp_listar_tipo_comprobante
as
    select * from tipo_comprobante
go

------------------------------------
------------------------------------

create or alter proc usp_listar_unidad_medida_producto
as
    select * from unidad_medida_producto
go
CREATE or alter proc usp_insertar_unidad_medida_producto
    @unidad NVARCHAR(100),
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO unidad_medida_producto(unidad, estado)
    VALUES (@unidad, @estado);
    SELECT SCOPE_IDENTITY() AS Id;
END
go
CREATE or alter proc usp_actualizar_unidad_medida_producto
    @id INT,
    @unidad NVARCHAR(100),
    @estado BIT
AS
BEGIN
    UPDATE unidad_medida_producto
    SET
        unidad = @unidad,
        estado = @estado
    WHERE Id = @id;
END
go
CREATE or alter proc usp_eliminar_unidad_medida_producto
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE unidad_medida_producto
    SET estado = 0 
    WHERE Id = @id;
END
go
------------------------------------
------------------------------------
create or alter proc usp_listar_categoria_producto
as
    select * from categoria_producto
go
CREATE or alter proc usp_insertar_categoria_producto
    @categoria NVARCHAR(100),
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO categoria_producto(categoria, estado)
    VALUES (@categoria, @estado);
    SELECT SCOPE_IDENTITY() AS Id;
END
go
CREATE or alter proc usp_actualizar_categoria_producto
    @id INT,
    @categoria NVARCHAR(100),
    @estado BIT
AS
BEGIN
    UPDATE categoria_producto
    SET
        categoria = @categoria,
        estado = @estado
    WHERE Id = @id;
END
go
CREATE or alter proc usp_eliminar_categoria_producto
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE categoria_producto
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------

create or alter proc usp_listar_productos
as
    select * from producto
go
CREATE or alter proc usp_crear_producto
    @nombre NVARCHAR(100),
    @id_unidad_medida_producto INT,
    @id_categoria_producto INT,
    @precio_unico DECIMAL(18, 2),
    @cantidad INT,
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO producto (Nombre, id_unidad_medida_producto, id_categoria_producto, precio_unico, cantidad, estado)
    VALUES (@nombre, @id_unidad_medida_producto, @id_categoria_producto, @precio_unico, @cantidad, @estado);

    
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE or alter proc usp_actualizar_producto
	@id INT,
    @nombre NVARCHAR(100),
    @id_unidad_medida_producto INT,
    @id_categoria_producto INT,
    @precio_unico DECIMAL(18, 2),
    @cantidad INT,
    @estado BIT
AS
BEGIN
    UPDATE producto
    SET
        Nombre = @nombre,
        id_unidad_medida_producto = @id_unidad_medida_producto,
		id_categoria_producto = @id_categoria_producto,
		precio_unico = @precio_unico,
		cantidad = @cantidad,
		estado =@estado
    WHERE id = @id;
END
go
CREATE or alter proc usp_eliminar_producto
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE producto
    SET estado = 0 
    WHERE Id = @id;
END
go



--Insertando datos de uso

INSERT INTO habitacion (numero, piso, precio_diario, id_tipo, id_estado, estado) VALUES
('101', '1', 250.00, 1, 1, 1),
('102', '1', 200.00, 2, 1, 1),
('103', '1', 220.00, 2, 1, 1),
('201', '2', 300.00, 1, 1, 1),
('202', '2', 180.00, 3, 1, 1),
('203', '2', 280.00, 4, 1, 1),
('301', '3', 350.00, 1, 1, 1),
('302', '3', 250.00, 3, 1, 1),
('303', '3', 200.00, 2, 1, 1),
('304', '3', 400.00, 1, 1, 1);

INSERT INTO trabajador (primer_nombre, segundo_nombre, primer_apellido, segundo_apellido, username, password, sueldo, id_tipo_documento, numero_documento, telefono, email, id_rol, estado) VALUES
('Carlos', 'Andrés', 'Pérez', 'Ramírez', 'cperez', '1234', 1500.00, 1, '12345678', '987654321', 'carlos@example.com', 1, 1),
('Ana', 'María', 'Lopez', 'Díaz', 'alopez', 'abcd', 2000.00, 1, '87654321', '912345678', 'ana@example.com', 2, 1),
('Luis', 'Fernando', 'Mendoza', 'Vega', 'lmendoza', 'pass', 1300.00, 1, '11223344', '987123456', 'luis@example.com', 3, 1),
('Elena', 'Paola', 'Torres', 'Gomez', 'etorres', '3210', 1250.00, 1, '22334455', '999123123', 'elena@example.com', 1, 1),
('Raúl', 'David', 'Cruz', 'Santos', 'rcruz', 'qwerty', 1100.00, 2, '33445566', '988654321', 'raul@example.com', 4, 1),
('Patricia', 'Sofia', 'Morales', 'Núñez', 'pmorales', 'asdf', 1700.00, 1, '44556677', '911223344', 'patricia@example.com', 2, 1),
('Javier', 'Enrique', 'Vargas', 'Alva', 'jvargas', 'password', 1800.00, 3, '55667788', '944332211', 'javier@example.com', 1, 1),
('Lucía', 'Isabel', 'Cáceres', 'Mora', 'lcaceres', '9876', 1400.00, 1, '66778899', '922334455', 'lucia@example.com', 3, 1),
('Ricardo', 'Manuel', 'Salas', 'Ibañez', 'rsalas', '7410', 1600.00, 2, '77889900', '933221100', 'ricardo@example.com', 1, 1),
('Verónica', 'Fernanda', 'Reyes', 'Palacios', 'vreyes', '3698', 2000.00, 1, '88990011', '911223311', 'vero@example.com', 2, 1);

INSERT INTO cliente (primer_nombre, segundo_nombre, primer_apellido, segundo_apellido, id_tipo_documento, numero_documento, telefono, email, fecha_nacimiento, id_tipo_nacionalidad, id_tipo_sexo, estado) VALUES
('Mario', 'Andrés', 'Gómez', 'Paz', 1, '11111111', '900000001', 'mario@gmail.com', '1990-01-01', 1, 1, 1),
('Lucía', 'María', 'Salinas', 'Lopez', 1, '22222222', '900000002', 'lucia@gmail.com', '1985-05-12', 1, 2, 1),
('Carmen', 'Patricia', 'Torres', 'Vega', 2, '33333333', '900000003', 'carmen@gmail.com', '1992-07-21', 2, 2, 1),
('Raúl', 'Ignacio', 'Vargas', 'Soto', 1, '44444444', '900000004', 'raul@gmail.com', '1988-03-14', 3, 1, 1),
('Elena', 'Gabriela', 'Martinez', 'Aguilar', 3, '55555555', '900000005', 'elena@gmail.com', '1995-11-30', 4, 2, 1),
('Pedro', 'Manuel', 'Reyes', 'Delgado', 1, '66666666', '900000006', 'pedro@gmail.com', '1991-09-05', 1, 1, 1),
('Claudia', 'Patricia', 'Nuñez', 'Ibarra', 2, '77777777', '900000007', 'claudia@gmail.com', '1987-06-25', 2, 2, 1),
('Jorge', 'Eduardo', 'Flores', 'Pineda', 1, '88888888', '900000008', 'jorge@gmail.com', '1993-08-19', 3, 1, 1),
('Mónica', 'Isabel', 'Ramirez', 'Quispe', 3, '99999999', '900000009', 'monica@gmail.com', '1997-12-22', 4, 2, 1),
('Felipe', 'Antonio', 'Ponce', 'Cornejo', 1, '10101010', '900000010', 'felipe@gmail.com', '1990-04-02', 1, 1, 1);

INSERT INTO estacionamiento (lugar, largo, alto, ancho, id_tipo_estacionamiento, estado) VALUES
('E01', '5', '2', '3', 1, 1),
('E02', '5', '2', '3', 2, 1),
('E03', '5', '2', '3', 1, 1),
('E04', '5', '2', '3', 2, 1),
('E05', '5', '2', '3', 1, 1),
('E06', '5', '2', '3', 2, 1),
('E07', '5', '2', '3', 1, 1),
('E08', '5', '2', '3', 2, 1),
('E09', '5', '2', '3', 1, 1),
('E10', '5', '2', '3', 2, 1);

INSERT INTO reserva (id_habitacion, id_cliente, id_trabajador, id_tipo_reserva, fecha_ingreso, fecha_salida, costo_alojamiento, estado) VALUES
(1, 1, 1, 1, '2025-04-01', '2025-04-05', 1000.00, 1),
(2, 2, 2, 2, '2025-04-03', '2025-04-04', 200.00, 1),
(3, 3, 3, 1, '2025-04-02', '2025-04-06', 800.00, 1),
(4, 4, 4, 2, '2025-04-07', '2025-04-10', 900.00, 1),
(5, 5, 5, 1, '2025-04-01', '2025-04-03', 360.00, 1),
(6, 6, 6, 1, '2025-04-05', '2025-04-08', 840.00, 1),
(7, 7, 7, 2, '2025-04-09', '2025-04-12', 1050.00, 1),
(8, 8, 8, 1, '2025-04-11', '2025-04-15', 1200.00, 1),
(9, 9, 9, 1, '2025-04-13', '2025-04-14', 400.00, 1),
(10, 10, 10, 2, '2025-04-16', '2025-04-18', 600.00, 1);


create or alter proc usp_listar_productos 
as
	select p.id, p.nombre, ump.unidad, cp.categoria, precio_unico, cantidad, p.estado from producto p 
	join unidad_medida_producto ump on p.id_unidad_medida_producto = ump.id 
	join categoria_producto cp on p.id_categoria_producto = cp.id

go
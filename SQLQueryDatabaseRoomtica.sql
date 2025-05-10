use master
go

drop database if exists db_roomtica;
go

create database db_roomtica
go

use db_roomtica
go

create table caracteristica_habitacion(
	id int identity(1, 1) primary key,
	caracteristica varchar(100),
	estado bit
);
go

create table tipo_habitacion(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	descripccion varchar(200),
	estado bit
);
go

create table caracteristica_habitacion_tipo_habitacion(
	id_caracteristica_habitacion int references caracteristica_habitacion,
	id_tipo_habitacion int references tipo_habitacion,
	estado bit,
	primary key (id_caracteristica_habitacion, id_tipo_habitacion)
);
go

create table estado_habitacion(
	id int identity(1, 1) primary key,
	estado_habitacion varchar(40) unique,
	estado bit
);
go

create table habitacion(
	id int identity(1, 1) primary key,
	numero varchar(5),
	piso varchar(3),
	precio_diario decimal(10,2),
	id_tipo int references tipo_habitacion,
	id_estado int references estado_habitacion,
	estado bit
);
go

create table rol_trabajador(
	id int identity(1, 1) primary key,
	rol varchar(40) unique,
	estado bit
);
go

create table tipo_documento(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	estado bit
);
go

create table tipo_nacionalidad(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	estado bit
);
go

create table tipo_sexo(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	estado bit
);
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
	estado bit
);
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
);
go



create table tipo_estacionamiento(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	costo decimal(10,2),
	estado bit
);
go

create table estacionamiento(
	id int identity(1, 1) primary key,
	lugar varchar(10) unique,
	largo varchar(10),
	alto varchar(10),
	ancho varchar(10),
	id_tipo_estacionamiento int references tipo_estacionamiento,
	estado bit
);
go

create table tipo_reserva(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	estado bit
);
go

create table reserva(
	id int identity(1, 1) primary key,
	id_habitacion int references habitacion,
	id_trabajador int references trabajador,
	id_tipo_reserva int references tipo_reserva,
	fecha_ingreso date,
	fecha_salida date,
	costo_alojamiento decimal(10,2),
	estado bit
);
go

create table cliente_reserva(
	id int identity(1, 1) primary key,
	id_cliente int references cliente,
	id_reserva int references reserva,
	estado bit
);
go

create table tipo_comprobante(
	id int identity(1, 1) primary key,
	tipo varchar(40) unique,
	estado bit
);
go

create table pago(
	id int identity(1, 1) primary key,
	id_reserva int unique references reserva,
	id_tipo_comprobante int references tipo_comprobante,
	igv decimal(10,2),
	total_pago decimal(10,2),
	fecha_emision date,
	fecha_pago date,
	estado bit
);
go

create table reserva_estacionamiento(
	id_reserva int references reserva,
	id_estacionamiento int references estacionamiento,
	cantidad int,
	precio_estacionamiento decimal(10,2),
	estado bit,
	primary key(id_reserva, id_estacionamiento)
);
go

create table unidad_medida_producto(
	id int identity(1, 1) primary key,
	unidad varchar(40) unique,
	estado bit
);
go

create table categoria_producto(
	id int identity(1, 1) primary key,
	categoria varchar(40) unique,
	estado bit
);
go

create table producto(
	id int identity(1, 1) primary key,
	nombre varchar(100) unique,
	id_unidad_medida_producto int references unidad_medida_producto,
	id_categoria_producto int references categoria_producto,
	precio_unico decimal(10,2),
	cantidad int,
	estado bit
);
go

create table consumo(
	id int identity(1, 1) primary key,
	id_reserva int references reserva,
	id_producto int references producto,
	cantidad int,
	precio_venta decimal(10,2),
	estado bit
);
go

use db_roomtica
go

-- caracteristica_habitacion
insert into caracteristica_habitacion (caracteristica, estado) values
('Vista al mar', 1),
('Aire acondicionado', 1),
('Jacuzzi', 1),
('Balcón privado', 1),
('WiFi gratuito', 1),
('TV 50"', 1),
('Mini Bar', 1),
('Caja fuerte', 1),
('Escritorio', 1),
('Secador de cabello', 1);

-- tipo_habitacion
insert into tipo_habitacion (tipo, descripccion, estado) values
('Simple', 'Habitación para una persona', 1),
('Doble', 'Habitación para dos personas', 1),
('Suite', 'Habitación de lujo', 1),
('Familiar', 'Habitación para cuatro personas', 1),
('Penthouse', 'Suite de lujo con terraza', 1),
('Superior', 'Habitación de categoría superior', 1),
('Económica', 'Habitación económica', 1),
('Deluxe', 'Habitación deluxe', 1),
('Junior Suite', 'Habitación ejecutiva', 1),
('Estándar', 'Habitación estándar', 1);

-- estado_habitacion
insert into estado_habitacion (estado_habitacion, estado) values
('Disponible', 1),
('Ocupada', 1),
('Limpieza', 1),
('Mantenimiento', 1),
('Reservada', 1);

-- rol_trabajador
insert into rol_trabajador (rol, estado) values
('Administrador', 1),
('Recepcionista', 1);

-- tipo_documento
insert into tipo_documento (tipo, estado) values
('DNI', 1),
('Pasaporte', 1),
('Carnet de Extranjería', 1),
('Licencia de Conducir', 1);

-- tipo_nacionalidad
insert into tipo_nacionalidad (tipo, estado) values
('Peruana', 1),
('Colombiana', 1),
('Argentina', 1),
('Chilena', 1),
('Ecuatoriana', 1),
('Boliviana', 1),
('Venezolana', 1),
('Brasileña', 1),
('Uruguaya', 1),
('Paraguaya', 1);

-- tipo_sexo
insert into tipo_sexo (tipo, estado) values
('Masculino', 1),
('Femenino', 1),
('No Binario', 1);

-- tipo_estacionamiento
insert into tipo_estacionamiento (tipo, costo, estado) values
('Cubierto', 20.00, 1),
('Libre', 0.00, 1),
('VIP', 50.00, 1);

-- tipo_reserva
insert into tipo_reserva (tipo, estado) values
('Normal', 1),
('Corporativa', 1),
('Paquete', 1);

-- tipo_comprobante
insert into tipo_comprobante (tipo, estado) values
('Boleta', 1),
('Factura', 1),
('Ticket', 1);

-- unidad_medida_producto
insert into unidad_medida_producto (unidad, estado) values
('Unidad', 1),
('Litro', 1),
('Botella', 1),
('Paquete', 1);

-- categoria_producto
insert into categoria_producto (categoria, estado) values
('Bebidas', 1),
('Snacks', 1),
('Licores', 1),
('Higiene', 1);

-- producto
insert into producto (nombre, id_unidad_medida_producto, id_categoria_producto, precio_unico, cantidad, estado) values
('Agua Mineral', 2, 1, 5.00, 100, 1),
('Cerveza', 3, 3, 12.00, 50, 1),
('Galletas', 4, 2, 8.50, 200, 1),
('Vino Tinto', 3, 3, 45.00, 20, 1),
('Pasta Dental', 1, 4, 10.00, 30, 1),
('Shampoo', 2, 4, 15.00, 25, 1),
('Jugo de Naranja', 2, 1, 7.50, 60, 1),
('Whisky', 3, 3, 80.00, 10, 1),
('Papas Fritas', 4, 2, 6.00, 150, 1),
('Refresco', 2, 1, 4.50, 80, 1);

-- trabajador
insert into trabajador (primer_nombre, segundo_nombre, primer_apellido, segundo_apellido, username, password, sueldo, id_tipo_documento, numero_documento, telefono, email, id_rol, estado) values
('Carlos', 'Andrés', 'Gómez', 'Pérez', 'carlosg', '123456', 2500.00, 1, '12345678', '987654321', 'carlos@gmail.com', 1, 1),
('Lucía', 'María', 'Vega', 'Rojas', 'luciar', 'abcdef', 1800.00, 2, 'A1234567', '912345678', 'lucia@gmail.com', 2, 1),
('Marcos', 'José', 'Reyes', 'Díaz', 'marcosr', 'pass123', 2000.00, 2, '87654321', '998877665', 'marcos@gmail.com', 2, 1),
('Elena', 'Rocío', 'Castro', 'López', 'elenac', 'qwerty', 2200.00, 2, 'E123456', '987654310', 'elena@gmail.com', 2, 1),
('David', 'Antonio', 'Soto', 'García', 'davids', 'clave', 3000.00, 2, '65432178', '976543210', 'david@gmail.com', 2, 1);

-- cliente
insert into cliente (primer_nombre, segundo_nombre, primer_apellido, segundo_apellido, id_tipo_documento, numero_documento, telefono, email, fecha_nacimiento, id_tipo_nacionalidad, id_tipo_sexo, estado) values
('Ana', 'Luisa', 'Torres', 'Fernández', 1, '45678912', '987123456', 'ana@gmail.com', '1990-05-20', 1, 2, 1),
('Pedro', 'Luis', 'Mendoza', 'Salas', 2, 'P567891', '912345679', 'pedro@gmail.com', '1985-08-15', 2, 1, 1),
('María', 'Carmen', 'Ríos', 'Sánchez', 1, '45678913', '987654311', 'maria@gmail.com', '1992-07-25', 3, 2, 1),
('José', 'Manuel', 'Palacios', 'Paredes', 1, '45678914', '976543212', 'jose@gmail.com', '1980-03-05', 1, 1, 1),
('Valeria', 'Isabel', 'Morales', 'Reyes', 2, 'V789123', '923456789', 'valeria@gmail.com', '1995-09-10', 4, 2, 1);

-- habitacion
insert into habitacion (numero, piso, precio_diario, id_tipo, id_estado, estado) values
('101', '1', 150.00, 1, 1, 1),
('102', '1', 200.00, 2, 1, 1),
('201', '2', 350.00, 3, 1, 1),
('202', '2', 120.00, 4, 1, 1),
('301', '3', 400.00, 5, 1, 1);

-- estacionamiento
insert into estacionamiento (lugar, largo, alto, ancho, id_tipo_estacionamiento, estado) values
('A01', '5m', '2m', '3m', 1, 1),
('A02', '5m', '2m', '3m', 2, 1),
('A03', '5m', '2m', '3m', 3, 1);

-- reserva
insert into reserva (id_habitacion, id_trabajador, id_tipo_reserva, fecha_ingreso, fecha_salida, costo_alojamiento, estado) values
(1, 1, 1, '2025-05-01', '2025-05-05', 600.00, 1),
(2, 1, 2, '2025-05-02', '2025-05-04', 400.00, 1),
(3, 2, 1, '2025-05-03', '2025-05-06', 1050.00, 1);

-- cliente_reserva
insert into cliente_reserva(id_cliente, id_reserva, estado) values
(1, 1, 1),
(2, 1, 1),
(2, 2, 1),
(3, 2, 1);

-- pago
insert into pago (id_reserva, id_tipo_comprobante, igv, total_pago, fecha_emision, fecha_pago, estado) values
(1, 1, 108.00, 708.00, '2025-05-01', '2025-05-01', 1),
(2, 2, 72.00, 472.00, '2025-05-02', '2025-05-02', 1),
(3, 1, 189.00, 1239.00, '2025-05-03', '2025-05-03', 1);

-- reserva_estacionamiento
insert into reserva_estacionamiento (id_reserva, id_estacionamiento, cantidad, precio_estacionamiento, estado) values
(1, 1, 1, 20.00, 1),
(2, 2, 1, 0.00, 1),
(3, 3, 1, 50.00, 1);

-- consumo
insert into consumo (id_reserva, id_producto, cantidad, precio_venta, estado) values
(1, 1, 2, 5.00, 1),
(1, 2, 4, 12.00, 1),
(2, 3, 3, 8.50, 1),
(3, 4, 1, 45.00, 1),
(3, 5, 1, 10.00, 1);
go

------------------------------------
------------------------------------
create or alter proc usp_listar_caracteristica_habitacion
as
	select id, caracteristica, estado
	from caracteristica_habitacion
	where estado = 1;
go
CREATE or alter proc usp_listar_caracteristica_habitacion_por_caracteristica
    @caracteristica NVARCHAR(100)
AS
select id, caracteristica, estado
	from caracteristica_habitacion 
	where @caracteristica = caracteristica 
go
CREATE or alter proc usp_obtener_caracteristica_habitacion_por_id
    @id int
AS
	select
	*
	from caracteristica_habitacion
	where id = @id and estado = 1
go

CREATE or alter proc usp_crear_caracteristica_habitacion
    @caracteristica VARCHAR(100),
    @estado bit
AS
BEGIN
    INSERT INTO caracteristica_habitacion (caracteristica, estado)
    VALUES (@caracteristica, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go

CREATE or alter proc usp_actualizar_caracteristica_habitacion
	@id int,
    @caracteristica VARCHAR(100),
    @estado bit
AS
BEGIN
    UPDATE caracteristica_habitacion 
	set caracteristica = @caracteristica, estado = 1
    where @id = id
END
go
CREATE or alter proc usp_eliminar_caracteristica_habitacion
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE caracteristica_habitacion
    SET estado = 0 
    WHERE Id = @id;
END
go


------------------------------------
------------------------------------

CREATE or alter proc usp_listar_caracteristica_habitacion_tipo_habitacion
AS
BEGIN
    SELECT 
		ch.caracteristica,
		th.tipo,
		chth.estado
    FROM caracteristica_habitacion_tipo_habitacion chth 
	inner join tipo_habitacion th on chth.id_tipo_habitacion = th.id
	inner join caracteristica_habitacion ch on chth.id_caracteristica_habitacion = ch.id
	where chth.estado = 1;
END
GO

CREATE or alter proc usp_obtener_por_id_caracteristica_habitacion
    @id_caracteristica_habitacion INT
AS
BEGIN
    SELECT id_caracteristica_habitacion, id_tipo_habitacion, estado
    FROM caracteristica_habitacion_tipo_habitacion
    WHERE id_caracteristica_habitacion = @id_caracteristica_habitacion;
END
GO

CREATE or alter proc usp_obtener_caracteristica_habitacion_tipo_habitacion_por_id
    @id_caracteristica_habitacion INT,
    @id_tipo_habitacion INT
AS
BEGIN
    SELECT id_caracteristica_habitacion, id_tipo_habitacion, estado
    FROM caracteristica_habitacion_tipo_habitacion
    WHERE id_caracteristica_habitacion = @id_caracteristica_habitacion
      AND id_tipo_habitacion = @id_tipo_habitacion
	  AND estado = 1;
END
GO

CREATE or alter proc usp_crear_caracteristica_habitacion_tipo_habitacion
    @id_caracteristica_habitacion INT,
    @id_tipo_habitacion INT,
    @estado BIT
AS
BEGIN
    INSERT INTO caracteristica_habitacion_tipo_habitacion (id_caracteristica_habitacion, id_tipo_habitacion, estado)
    VALUES (@id_caracteristica_habitacion, @id_tipo_habitacion, 1);
END
GO

CREATE or alter proc usp_actualizar_caracteristica_habitacion_tipo_habitacion
    @id_caracteristica_habitacion INT,
    @id_tipo_habitacion INT,
    @estado BIT
AS
BEGIN
    UPDATE caracteristica_habitacion_tipo_habitacion
    SET estado = 1
    WHERE id_caracteristica_habitacion = @id_caracteristica_habitacion
      AND id_tipo_habitacion = @id_tipo_habitacion;
END
GO

CREATE or alter proc usp_eliminar_caracteristica_habitacion_tipo_habitacion
    @id_caracteristica_habitacion INT,
	@id_tipo_habitacion int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE caracteristica_habitacion_tipo_habitacion
    SET estado = 0 
    WHERE id_caracteristica_habitacion = @id_caracteristica_habitacion and id_tipo_habitacion = @id_tipo_habitacion;
END
go
------------------------------------
------------------------------------
CREATE or alter proc usp_listar_categorias_producto
AS
BEGIN
    SELECT id, categoria, estado
    FROM categoria_producto
	where estado = 1;
END
GO

CREATE or alter proc usp_obtener_categoria_producto_por_categoria
    @categoria varchar(40)
AS
BEGIN
    SELECT id, categoria, estado
    FROM categoria_producto
    WHERE @categoria = categoria;
END
GO

CREATE or alter proc usp_obtener_categoria_producto_por_id
    @id INT
AS
BEGIN
    SELECT id, categoria, estado
    FROM categoria_producto
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_categoria_producto
	@categoria varchar(40),
    @estado BIT
AS
BEGIN
    INSERT INTO categoria_producto (categoria, estado)
    VALUES (@categoria, 1);
END
GO

CREATE or alter proc usp_actualizar_categoria_producto
    @id INT,
    @categoria varchar(40),
    @estado BIT
AS
BEGIN
    UPDATE categoria_producto
    SET categoria = @categoria,
		estado = 1
    WHERE id = @id
END
GO

CREATE or alter proc usp_eliminar_categoria_producto
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE categoria_producto
    SET estado = 0 
    WHERE id = @id;
END
go


------------------------------------
------------------------------------

CREATE or alter proc usp_listar_clientes
AS
BEGIN
    SELECT  
		p.id,
		p.primer_nombre,
		p.segundo_nombre,
		p.primer_apellido,
		p.segundo_apellido,
		d.tipo,
		p.numero_documento,
		p.telefono,
		p.email,
		p.fecha_nacimiento,
		n.tipo,
		s.tipo,
		p.estado
    FROM cliente p
    INNER JOIN tipo_documento d ON p.id_tipo_documento = d.id
    INNER JOIN tipo_nacionalidad n ON p.id_tipo_nacionalidad = n.id
	INNER JOIN tipo_sexo s ON p.id_tipo_sexo = s.id
	where p.estado = 1
END
go
CREATE or alter proc usp_obtener_clienteDTO_por_id
    @id INT
AS
BEGIN
    SELECT  
		p.id,
		p.primer_nombre,
		p.segundo_nombre,
		p.primer_apellido,
		p.segundo_apellido,
		d.tipo,
		p.numero_documento,
		p.telefono,
		p.email,
		p.fecha_nacimiento,
		n.tipo,
		s.tipo,
		p.estado
    FROM cliente p
    INNER JOIN tipo_documento d ON p.id_tipo_documento = d.id
    INNER JOIN tipo_nacionalidad n ON p.id_tipo_nacionalidad = n.id
	INNER JOIN tipo_sexo s ON p.id_tipo_sexo = s.id
    WHERE p.id = @id and p.estado = 1
END
go
CREATE or alter proc usp_obtener_cliente_por_id
    @id INT
AS
BEGIN
    SELECT 
       *
    FROM cliente
    WHERE id = @id and estado = 1
END
GO

CREATE or alter proc usp_obtener_cliente_por_dni
    @numero_documento varchar(10)
AS
BEGIN
    SELECT 
       *
    FROM cliente
    WHERE numero_documento = @numero_documento and estado = 1
END
GO

CREATE or alter proc usp_crear_cliente
    @primer_nombre VARCHAR(50),
    @segundo_nombre VARCHAR(50),
    @primer_apellido VARCHAR(50),
    @segundo_apellido VARCHAR(50),
    @id_tipo_documento INT,
	@numero_documento VARCHAR(10),
	@telefono VARCHAR(10),
	@email VARCHAR(25),
	@fecha_nacimiento DATE,
	@id_tipo_nacionalidad INT,
	@id_tipo_sexo INT,
	@estado BIT
AS
BEGIN
    INSERT INTO cliente (primer_nombre, segundo_nombre, primer_apellido, segundo_apellido, id_tipo_documento, numero_documento, telefono, email, fecha_nacimiento, id_tipo_nacionalidad, id_tipo_sexo, estado)
    VALUES (@primer_nombre, @segundo_nombre, @primer_apellido, @segundo_apellido, @id_tipo_documento, @numero_documento, @telefono, @email, @fecha_nacimiento, @id_tipo_nacionalidad, @id_tipo_sexo, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_cliente
	@id int,
    @primer_nombre VARCHAR(50),
    @segundo_nombre VARCHAR(50),
    @primer_apellido VARCHAR(50),
    @segundo_apellido VARCHAR(50),
    @id_tipo_documento INT,
	@numero_documento VARCHAR(10),
	@telefono VARCHAR(10),
	@email VARCHAR(25),
	@fecha_nacimiento DATE,
	@id_tipo_nacionalidad INT,
	@id_tipo_sexo INT,
	@estado BIT
AS
BEGIN
    UPDATE cliente
    SET primer_nombre = @primer_nombre,
        segundo_nombre = @segundo_nombre,
        primer_apellido = @primer_apellido,
        segundo_apellido = @segundo_apellido,
        id_tipo_documento = @id_tipo_documento,
		numero_documento = @numero_documento,
		telefono = @telefono,
		email = @email,
		fecha_nacimiento = @fecha_nacimiento,
		id_tipo_nacionalidad = @id_tipo_nacionalidad,
		id_tipo_sexo = @id_tipo_sexo,
        estado = 1
    WHERE id = @id
END
go
CREATE or alter proc usp_eliminar_cliente
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cliente
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------

CREATE or alter proc usp_listar_consumos
AS
BEGIN
    SELECT 
      c.id,
	  r.id,
	  p.nombre,
	  c.cantidad,
	  c.precio_venta,
	  c.estado
    FROM consumo c
	INNER JOIN reserva r on c.id_reserva = r.id
	INNER JOIN producto p on c.id_producto = p.id
	where c.estado = 1;
END
go

CREATE or alter proc usp_obtener_consumoDTO_por_id
    @id INT
AS
BEGIN
     SELECT 
      c.id,
	  r.id,
	  p.nombre,
	  c.cantidad,
	  c.precio_venta,
	  c.estado
    FROM consumo c
	INNER JOIN reserva r on c.id_reserva = r.id
	INNER JOIN producto p on c.id_producto = p.id
    WHERE c.id = @id and c.estado = 1;
END
go
CREATE or alter proc usp_obtener_consumo_por_id
    @id INT
AS
BEGIN
    SELECT 
       *
    FROM consumo
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_consumo
    @id_reserva INT,
    @id_producto INT,
    @cantidad INT,
    @precio_venta decimal(10,2),
    @estado BIT
AS
BEGIN
    INSERT INTO consumo (id_reserva, id_producto, cantidad, precio_venta, estado)
    VALUES (@id_reserva, @id_producto, @cantidad, @precio_venta, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_consumo
    @id INT,
    @id_reserva INT,
    @id_producto INT,
    @cantidad INT,
    @precio_venta decimal(10,2),
    @estado BIT
AS
BEGIN
    UPDATE consumo
    SET id_reserva = @id_reserva,
        id_producto = @id_producto,
        cantidad = @cantidad,
        precio_venta = @precio_venta,
        estado = 1
    WHERE id = @id
END
go
CREATE or alter proc usp_eliminar_consumo
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE consumo
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------

CREATE or alter proc usp_listar_estacionamientos
AS
BEGIN
    SELECT 
       e.id,
	   e.lugar,
	   e.largo,
	   e.alto,
	   e.ancho,
	   u.tipo,
	   e.estado
    FROM estacionamiento e
    INNER JOIN tipo_estacionamiento u ON e.id_tipo_estacionamiento = u.id
	where e.estado = 1;
END
go
CREATE or alter proc usp_obtener_estacionamientoDTO_por_id
    @id INT
AS
BEGIN
    SELECT 
       e.id,
	   e.lugar,
	   e.largo,
	   e.alto,
	   e.ancho,
	   u.tipo,
	   e.estado
    FROM estacionamiento e
    INNER JOIN tipo_estacionamiento u ON e.id_tipo_estacionamiento = u.id
    WHERE e.id = @id and e.estado = 1;
END
go
CREATE or alter proc usp_obtener_estacionamiento_por_id
    @id INT
AS
BEGIN
    SELECT 
       *
    FROM estacionamiento
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_estacionamiento
    @lugar VARCHAR(10),
    @largo VARCHAR(10),
    @alto VARCHAR(10),
    @ancho VARCHAR(10),
    @id_tipo_estacionamiento INT,
    @estado BIT
AS
BEGIN
    INSERT INTO estacionamiento (lugar, largo, alto, ancho, id_tipo_estacionamiento, estado)
    VALUES (@lugar, @largo, @alto, @ancho, @id_tipo_estacionamiento, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_estacionamiento
    @id INT,
    @lugar VARCHAR(10),
    @largo VARCHAR(10),
    @alto VARCHAR(10),
    @ancho VARCHAR(10),
    @id_tipo_estacionamiento INT,
    @estado BIT
AS
BEGIN
    UPDATE estacionamiento
    SET lugar = @lugar,
        largo = @largo,
        alto = @alto,
        ancho = @ancho,
        id_tipo_estacionamiento = @id_tipo_estacionamiento,
        estado = 1
    WHERE id = @id
END
go
CREATE or alter proc usp_eliminar_estacionamiento
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE estacionamiento
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------

CREATE or alter proc usp_listar_estado_habitacion
AS
BEGIN
    SELECT 
       *
    FROM estado_habitacion
	where estado = 1;
END
go
CREATE or alter proc usp_listar_estado_habitacion_por_estado
    @estado bit
AS
BEGIN
     SELECT 
       *
    FROM estado_habitacion
    WHERE estado = 1
END
go
CREATE or alter proc usp_obtener_estado_habitacion_por_id
    @id INT
AS
BEGIN
    SELECT 
        *
    FROM estado_habitacion
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_estado_habitacion
    @estado_habitacion VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO estado_habitacion (estado_habitacion, estado)
    VALUES (@estado_habitacion, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_estado_habitacion
    @id INT,
	@estado_habitacion VARCHAR(40),
    @estado BIT
AS
BEGIN
    UPDATE estado_habitacion
    SET estado_habitacion = @estado_habitacion,
        estado = 1
    WHERE id = @id
END
go
CREATE or alter proc usp_eliminar_estado_habitacion
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE estado_habitacion
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------

CREATE or alter proc usp_listar_habitaciones
AS
BEGIN
    SELECT 
       h.id,
	   h.numero,
	   h.piso,
	   h.precio_diario,
	   th.descripccion,
	   eh.estado_habitacion,
	   h.estado
    FROM habitacion h
    INNER JOIN tipo_habitacion th ON h.id_tipo = th.id
    INNER JOIN estado_habitacion eh ON h.id_estado = eh.id
	where h.estado = 1;
END
go
CREATE or alter proc usp_obtener_habitacionDTO_por_id
    @id INT
AS
BEGIN
    SELECT 
       h.id,
	   h.numero,
	   h.piso,
	   h.precio_diario,
	   th.descripccion,
	   eh.estado_habitacion,
	   h.estado
    FROM habitacion h
    INNER JOIN tipo_habitacion th ON h.id_tipo = th.id
    INNER JOIN estado_habitacion eh ON h.id_estado = eh.id
    WHERE h.id = @id and h.estado = 1
END
go
CREATE or alter proc usp_obtener_habitacion_por_id
    @id INT
AS
BEGIN
    SELECT 
      *
    FROM habitacion
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_habitacion
    @numero VARCHAR(5),
    @piso VARCHAR(3),
    @precio_diario decimal(10,2),
    @id_tipo int,
    @id_estado INT,
    @estado BIT
AS
BEGIN
    INSERT INTO habitacion (numero, piso, precio_diario, id_tipo, id_estado, estado)
    VALUES (@numero, @piso, @precio_diario, @id_tipo, @id_estado, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_habitacion
    @id INT,
    @numero VARCHAR(5),
    @piso VARCHAR(3),
    @precio_diario decimal(10,2),
    @id_tipo int,
    @id_estado INT,
    @estado BIT
AS
BEGIN
    UPDATE habitacion
    SET numero = @numero,
        piso = @piso,
        precio_diario = @precio_diario,
        id_tipo = @id_tipo,
        id_estado = @id_estado,
        estado = 1
    WHERE id = @id
END
go
CREATE or alter proc usp_eliminar_habitacion
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE habitacion
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------

CREATE or alter proc usp_listar_pagos
AS
BEGIN
    SELECT 
       p.id,
	   h.numero,
	   c.tipo,
	   p.igv,
	   p.total_pago,
	   p.fecha_emision,
	   p.fecha_pago,
	   p.estado
    FROM pago p
    INNER JOIN reserva r ON p.id_reserva = r.id
    INNER JOIN tipo_comprobante c ON p.id_tipo_comprobante = c.id
	inner join habitacion h on r.id_habitacion = h.id
	where p.estado = 1;
END
go
CREATE or alter proc usp_obtener_pagoDTO_por_id
    @id INT
AS
BEGIN
    SELECT 
       p.id,
	   r.costo_alojamiento,
	   c.tipo,
	   p.igv,
	   p.total_pago,
	   p.fecha_emision,
	   p.fecha_pago,
	   p.estado
    FROM pago p
    INNER JOIN reserva r ON p.id_reserva = r.id
    INNER JOIN tipo_comprobante c ON p.id_tipo_comprobante = c.id
    WHERE p.id = @id and p.estado = 1;
END
go
CREATE or alter proc usp_obtener_pago_por_id
    @id INT
AS
BEGIN
    SELECT 
       *
    FROM pago
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_pago
    @id_reserva INT,
    @id_tipo_comprobante INT,
    @igv decimal(10,2),
    @total_pago decimal(10,2),
    @fecha_emision date,
	@fecha_pago date,
    @estado BIT
AS
BEGIN
    INSERT INTO pago (id_reserva, id_tipo_comprobante, igv, total_pago, fecha_emision, fecha_pago, estado)
    VALUES (@id_reserva, @id_tipo_comprobante, @igv, @total_pago, @fecha_emision,@fecha_pago, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_pago
    @id INT,
    @id_reserva INT,
    @id_tipo_comprobante INT,
    @igv decimal(10,2),
    @total_pago decimal(10,2),
    @fecha_emision date,
	@fecha_pago date,
    @estado BIT
AS
BEGIN
    UPDATE pago
    SET id_reserva = @id_reserva,
        id_tipo_comprobante = @id_tipo_comprobante,
        igv = @igv,
        total_pago = @total_pago,
        fecha_emision = @fecha_emision,
		fecha_pago = @fecha_pago, 
        estado = 1
    WHERE id = @id
END
go
CREATE or alter proc usp_eliminar_pago
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE pago
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------
CREATE or alter proc usp_listar_productos
AS
BEGIN
    SELECT 
        p.id,
        p.nombre,
        u.unidad,
        c.categoria,
        p.precio_unico,
        p.cantidad,
        p.estado
    FROM Producto p
    INNER JOIN unidad_medida_producto u ON p.id_unidad_medida_producto = u.id
    INNER JOIN categoria_producto c ON p.id_categoria_producto = c.id
	where p.estado = 1;
END
go
CREATE or alter proc usp_obtener_productoDTO_por_id
    @id INT
AS
BEGIN
    SELECT 
        p.id,
        p.nombre,
        u.unidad,
        c.categoria,
        p.precio_unico,
        p.cantidad,
        p.estado
    FROM Producto p
    INNER JOIN unidad_medida_producto u ON p.id_unidad_medida_producto = u.id
    INNER JOIN categoria_producto c ON p.id_categoria_producto = c.id
    WHERE p.id = @id and p.estado = 1;
END
go
CREATE or alter proc usp_obtener_producto_por_id
    @id INT
AS
BEGIN
    SELECT 
        id,
        nombre,
        id_unidad_medida_producto,
        id_categoria_producto,
        precio_unico,
        cantidad,
        estado
    FROM Producto
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_producto
    @nombre NVARCHAR(100),
    @id_unidad_medida_producto INT,
    @id_categoria_producto INT,
    @precio_unico decimal(10,2),
    @cantidad INT,
    @estado BIT
AS
BEGIN
    INSERT INTO Producto (nombre, id_unidad_medida_producto, id_categoria_producto, precio_unico, cantidad, estado)
    VALUES (@nombre, @id_unidad_medida_producto, @id_categoria_producto, @precio_unico, @cantidad, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_producto
    @id INT,
    @nombre NVARCHAR(100),
    @id_unidad_medida_producto INT,
    @id_categoria_producto INT,
    @precio_unico decimal(10,2),
    @cantidad INT,
    @estado BIT
AS
BEGIN
    UPDATE Producto
    SET nombre = @nombre,
        id_unidad_medida_producto = @id_unidad_medida_producto,
        id_categoria_producto = @id_categoria_producto,
        precio_unico = @precio_unico,
        cantidad = @cantidad,
        estado = 1
    WHERE id = @id
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

------------------------------------
------------------------------------

CREATE or alter proc usp_listar_reserva_estacionamiento
AS
BEGIN
    SELECT 
       r.estado,
	   e.lugar,
	   re.cantidad,
	   re.precio_estacionamiento,
	   re.estado
    FROM reserva_estacionamiento re
    INNER JOIN estacionamiento e ON re.id_estacionamiento = e.id
    INNER JOIN reserva r ON re.id_reserva = r.id
	where r.estado = 1;
END
go
CREATE or alter proc usp_obtener_reserva_estacionamientoDTO_por_id
    @id_reserva INT,
	@id_estacionamiento INT
AS
BEGIN
    SELECT 
       r.estado,
	   e.lugar,
	   re.cantidad,
	   re.precio_estacionamiento,
	   re.estado
    FROM reserva_estacionamiento re
    INNER JOIN estacionamiento e ON re.id_estacionamiento = e.id
    INNER JOIN reserva r ON re.id_reserva = r.id
    WHERE re.id_reserva = @id_reserva and re.id_estacionamiento = @id_estacionamiento
	and r.estado = 1;
END
go
CREATE or alter proc usp_obtener_reserva_estacionamiento_por_id
    @id_reserva INT,
	@id_estacionamiento INT
AS
BEGIN
    SELECT 
      *
    FROM reserva_estacionamiento 
    WHERE id_reserva = @id_reserva and id_estacionamiento = @id_estacionamiento
	and estado = 1;
END
GO

CREATE or alter proc usp_crear_reserva_estacionamiento
	@id_reserva int,
    @id_estacionamiento int,
    @cantidad INT,
    @precio_estacionamiento decimal(10,2),
    @estado BIT
AS
BEGIN
    INSERT INTO reserva_estacionamiento (id_reserva, id_estacionamiento, cantidad, precio_estacionamiento, estado)
    VALUES (@id_reserva, @id_estacionamiento, @cantidad, @precio_estacionamiento, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_reserva_estacionamiento
	@id_reserva int,
    @id_estacionamiento int,
    @cantidad INT,
    @precio_estacionamiento decimal(10,2),
    @estado BIT
AS
BEGIN
    UPDATE reserva_estacionamiento
    SET cantidad = @cantidad,
        precio_estacionamiento = @precio_estacionamiento,
        estado = 1
    WHERE id_reserva = @id_reserva and id_estacionamiento = @id_estacionamiento
END
go
CREATE or alter proc usp_eliminar_reserva_estacionamiento
	@id_reserva int,
    @id_estacionamiento int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE reserva_estacionamiento
    SET estado = 0 
     WHERE id_reserva = @id_reserva and id_estacionamiento = @id_estacionamiento
END
go

------------------------------------
------------------------------------

CREATE or alter proc usp_listar_reservas
AS
BEGIN
    SELECT 
       r.id,
	   h.numero,
	   t.primer_nombre,
	   tr.tipo,
	   r.fecha_ingreso,
	   r.fecha_salida,
	   r.costo_alojamiento,
	   r.estado
    FROM reserva r
    INNER JOIN habitacion h ON r.id_habitacion = h.id
	INNER JOIN trabajador t ON r.id_trabajador = t.id
	INNER JOIN tipo_reserva tr ON r.id_tipo_reserva = tr.id
	where r.estado = 1;
END
go
CREATE or alter proc usp_obtener_reservaDTO_por_id
    @id INT
AS
BEGIN
    SELECT 
       r.id,
	   h.numero,
	   t.primer_nombre,
	   tr.tipo,
	   r.fecha_ingreso,
	   r.fecha_salida,
	   r.costo_alojamiento,
	   r.estado
    FROM reserva r
    INNER JOIN habitacion h ON r.id_habitacion = h.id
	INNER JOIN trabajador t ON r.id_trabajador = t.id
	INNER JOIN tipo_reserva tr ON r.id_tipo_reserva = tr.id
    WHERE r.id = @id and r.estado = 1;
END
go
CREATE or alter proc usp_obtener_reserva_por_id
    @id INT
AS
BEGIN
   SELECT 
      *
    FROM reserva
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_reserva
    @id_habitacion INT,
    @id_trabajador INT,
    @id_tipo_reserva INT,
    @fecha_ingreso date,
	@fecha_salida date,
	@costo_alojamiento decimal(10,2),
    @estado BIT
AS
BEGIN
    INSERT INTO reserva (id_habitacion, id_trabajador, id_tipo_reserva, fecha_ingreso, fecha_salida, costo_alojamiento, estado)
    VALUES (@id_habitacion, @id_trabajador, @id_tipo_reserva, @fecha_ingreso, @fecha_salida, @costo_alojamiento, 1);

	UPDATE habitacion
		SET id_estado = 2
		WHERE id = @id_trabajador;	

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go


CREATE or alter proc usp_actualizar_reserva
    @id INT,
    @id_habitacion INT,
    @id_trabajador INT,
    @id_tipo_reserva INT,
    @fecha_ingreso date,
	@fecha_salida date,
	@costo_alojamiento decimal(10,2),
    @estado BIT
AS
BEGIN
    UPDATE reserva
    SET id_habitacion = @id_habitacion,
        id_trabajador = @id_trabajador,
        id_tipo_reserva = @id_tipo_reserva,
        fecha_ingreso = @fecha_ingreso,
		fecha_salida = @fecha_salida,
		costo_alojamiento = @costo_alojamiento,
        estado = 1
    WHERE id = @id
END
go
CREATE or alter proc usp_eliminar_reserva
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE reserva
    SET estado = 0 
    WHERE Id = @id;
END
go
------------------------------------
------------------------------------

CREATE or alter proc usp_listar_cliente_reserva
AS
    select 
	cr.id,
	c.primer_nombre,
	r.id
	from cliente_reserva cr
	join cliente c on cr.id_cliente = c.id
	join reserva r on r.id = cr.id_reserva
	where cr.estado = 1
GO

CREATE or alter proc usp_obtener_cliente_reserva_por_id
    @id INT
AS
	select 
	id, 
	id_cliente,
	id_reserva
	from cliente_reserva
	
	where estado = 1 and @id = id
go


CREATE or alter proc usp_obtener_cliente_reservaDTO_por_id
    @id INT
AS
	select 
	c.primer_nombre,
	r.id
	from cliente_reserva cr 
	join cliente c on cr.id_cliente = c.id
	join reserva r on r.id = cr.id_reserva
	where cr.estado = 1 and @id = cr.id
go


CREATE or alter proc usp_insertar_cliente_reserva
    @id_cliente INT,
    @id_reserva INT
AS
    INSERT INTO cliente_reserva (id_cliente, id_reserva, estado)
    VALUES (@id_cliente, @id_reserva, 1);
GO

CREATE OR ALTER PROCEDURE usp_actualizar_cliente_reserva
    @id INT,
    @id_cliente INT,
    @id_reserva INT
AS
    UPDATE cliente_reserva
    SET id_cliente = @id_cliente,
        id_reserva = @id_reserva
    WHERE id = @id and estado = 1;
GO
CREATE or alter proc usp_eliminar_cliente_reserva
    @id INT
AS
    UPDATE cliente_reserva 
	set estado = 0
    WHERE Id = @id;
go


------------------------------------
------------------------------------

CREATE or alter proc usp_listar_rol_trabajadores
AS
BEGIN
    SELECT 
       *
    FROM rol_trabajador
	where estado = 1;
END
go
CREATE or alter proc usp_listar_rol_trabajadores_por_rol
    @rol INT
AS
BEGIN
     SELECT 
       *
    FROM rol_trabajador
    WHERE rol = @rol
END
go
CREATE or alter proc usp_obtener_rol_trabajador_por_id
    @id INT
AS
BEGIN
    SELECT 
        *
    FROM rol_trabajador
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_rol_trabajador
    @rol VARCHAR(40),
    @estado bit
AS
BEGIN
    INSERT INTO rol_trabajador (rol, estado)
    VALUES (@rol, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_rol_trabajador
    @id INT,
    @rol VARCHAR(40),
    @estado bit
AS
BEGIN
    UPDATE rol_trabajador
    SET rol = @rol,
        estado = 1
    WHERE id = @id
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

CREATE or alter proc usp_listar_tipo_comprobante
AS
BEGIN
    SELECT 
      *
    FROM tipo_comprobante
	where estado = 1;
END
go
CREATE or alter proc usp_obtener_tipo_comprobante_por_tipo
    @tipo varchar(40)
AS
BEGIN
    SELECT 
      *
    FROM tipo_comprobante 
END
go
CREATE or alter proc usp_obtener_tipo_comprobante_por_id
    @id INT
AS
BEGIN
    SELECT 
      *
    FROM tipo_comprobante 
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_tipo_comprobante
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_comprobante (tipo, estado)
    VALUES (@tipo, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_tipo_comprobante
    @id INT,
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    UPDATE tipo_comprobante
    SET @tipo = @tipo,
        estado = 1
    WHERE id = @id
END
go
CREATE or alter proc usp_eliminar_tipo_comprobante
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tipo_comprobante
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------
CREATE or alter proc usp_listar_tipo_documentos
AS
BEGIN
    SELECT 
      *
    FROM tipo_documento
	where estado = 1;
END
go
CREATE or alter proc usp_listar_tipo_documentos_por_tipo
    @tipo varchar(40)
AS
BEGIN
    SELECT 
      *
    FROM tipo_documento 
END
go
CREATE or alter proc usp_obtener_tipo_documento_por_id
    @id INT
AS
BEGIN
    SELECT 
      *
    FROM tipo_documento 
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_tipo_documento
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_documento (tipo, estado)
    VALUES (@tipo, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_tipo_documento
    @id INT,
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    UPDATE tipo_documento
    SET @tipo = @tipo,
        estado = 1
    WHERE id = @id
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

CREATE or alter proc usp_listar_tipo_estacionamiento
AS
BEGIN
    SELECT 
      *
    FROM tipo_estacionamiento
	where estado = 1;
END
go
CREATE or alter proc usp_listar_tipo_estacionamiento_por_tipo
    @tipo varchar(40)
AS
BEGIN
    SELECT 
      *
    FROM tipo_estacionamiento
	where tipo = @tipo
END
go
CREATE or alter proc usp_obtener_tipo_estacionamiento_por_id
    @id INT
AS
BEGIN
    SELECT 
      *
    FROM tipo_estacionamiento 
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_tipo_estacionamiento
    @tipo VARCHAR(40),
	@costo decimal(10,2),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_estacionamiento (tipo, costo, estado)
    VALUES (@tipo, @costo, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_tipo_estacionamiento
    @id INT,
    @tipo VARCHAR(40),
	@costo decimal(10,2),
    @estado BIT
AS
BEGIN
    UPDATE tipo_estacionamiento
    SET tipo = @tipo,
		costo = @costo,
        estado = 1
    WHERE id = @id
END
go
CREATE or alter proc usp_eliminar_tipo_estacionamiento
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tipo_estacionamiento
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------

CREATE or alter proc usp_listar_tipo_habitacion
AS
BEGIN
    SELECT 
      *
    FROM tipo_habitacion
	where estado = 1;
END
go
CREATE or alter proc usp_listar_tipo_habitacion_por_tipo
    @tipo varchar(40)
AS
BEGIN
    SELECT 
      *
    FROM tipo_habitacion
	where tipo = @tipo
END
go
CREATE or alter proc usp_obtener_tipo_habitacion_por_id
    @id INT
AS
BEGIN
    SELECT 
      *
    FROM tipo_habitacion 
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_tipo_habitacion
    @tipo VARCHAR(40),
	@descripccion VARCHAR(200),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_habitacion (tipo, descripccion, estado)
    VALUES (@tipo, @descripccion, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_tipo_habitacion
    @id INT,
    @tipo VARCHAR(40),
	@descripccion VARCHAR(200),
    @estado BIT
AS
BEGIN
    UPDATE tipo_habitacion
    SET tipo = @tipo,
		descripccion = @descripccion,
        estado = 1
    WHERE id = @id
END
go
CREATE or alter proc usp_eliminar_tipo_habitacion
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tipo_habitacion
    SET estado = 0 
    WHERE Id = @id;
END
go

------------------------------------
------------------------------------

CREATE or alter proc usp_listar_tipo_nacionalidades
AS
BEGIN
    SELECT 
      *
    FROM tipo_nacionalidad
	where estado = 1;
END
go
CREATE or alter proc usp_obtener_tipo_nacionalidades_por_tipo
    @tipo varchar(40)
AS
BEGIN
    SELECT 
      *
    FROM tipo_nacionalidad
	where tipo = @tipo
END
go
CREATE or alter proc usp_obtener_tipo_nacionalidad_por_id
    @id INT
AS
BEGIN
    SELECT 
      *
    FROM tipo_nacionalidad 
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_tipo_nacionalidad
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_nacionalidad (tipo, estado)
    VALUES (@tipo, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_tipo_nacionalidad
    @id INT,
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    UPDATE tipo_nacionalidad
    SET tipo = @tipo,
        estado = 1
    WHERE id = @id
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

CREATE or alter proc usp_listar_tipo_reserva
AS
BEGIN
    SELECT 
      *
    FROM tipo_reserva
	where estado = 1;
END
go
CREATE or alter proc usp_obtener_tipo_reserva_por_tipo
    @tipo varchar(40)
AS
BEGIN
    SELECT 
      *
    FROM tipo_reserva
	where tipo = @tipo
END
go
CREATE or alter proc usp_obtener_tipo_reserva_por_id
    @id INT
AS
BEGIN
    SELECT 
      *
    FROM tipo_reserva 
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_tipo_reserva
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_reserva (tipo, estado)
    VALUES (@tipo, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_tipo_reserva
    @id INT,
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    UPDATE tipo_reserva
    SET tipo = @tipo,
        estado = 1
    WHERE id = @id
END
go
CREATE or alter proc usp_eliminar_tipo_reserva
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tipo_reserva
    SET estado = 0 
    WHERE Id = @id;
END
go


------------------------------------
------------------------------------

CREATE or alter proc usp_listar_tipo_sexo
AS
BEGIN
    SELECT 
      *
    FROM tipo_sexo
	where estado = 1;
END
go
CREATE or alter proc usp_obtener_tipo_sexo_por_tipo
    @tipo varchar(40)
AS
BEGIN
    SELECT 
      *
    FROM tipo_sexo
	where tipo = @tipo
END
go
CREATE or alter proc usp_obtener_tipo_sexo_por_id
    @id INT
AS
BEGIN
    SELECT 
      *
    FROM tipo_sexo 
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_tipo_sexo
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_sexo (tipo, estado)
    VALUES (@tipo, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_tipo_sexo
    @id INT,
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    UPDATE tipo_sexo
    SET tipo = @tipo,
        estado = 1
    WHERE id = @id
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

CREATE or alter proc usp_listar_trabajadores
AS
BEGIN
    SELECT 
      t.id,
	  t.primer_nombre,
	  t.segundo_nombre,
	  t.primer_apellido,
	  t.segundo_apellido,
	  t.username,
	  t.password,
	  t.sueldo,
	  td.tipo,
	  t.numero_documento,
	  t.telefono,
	  t.email,
	  rt.rol,
	  t.estado
    FROM trabajador t
    INNER JOIN tipo_documento td ON t.id_tipo_documento = td.id
    INNER JOIN rol_trabajador rt ON t.id_rol = rt.id
	where t.estado = 1;
END
go
CREATE or alter proc usp_obtener_trabajadorDTO_por_id
    @id INT
AS
BEGIN
   SELECT 
      t.id,
	  t.primer_nombre,
	  t.segundo_nombre,
	  t.primer_apellido,
	  t.segundo_apellido,
	  t.username,
	  t.password,
	  t.sueldo,
	  td.tipo,
	  t.numero_documento,
	  t.telefono,
	  t.email,
	  rt.rol,
	  t.estado
    FROM trabajador t
    INNER JOIN tipo_documento td ON t.id_tipo_documento = td.id
    INNER JOIN rol_trabajador rt ON t.id_rol = rt.id
    WHERE t.id = @id and t.estado = 1;
END
go
CREATE or alter proc usp_obtener_trabajador_por_id
    @id INT
AS
BEGIN
   SELECT 
      *
    FROM trabajador
    WHERE id = @id and estado = 1;
END
GO

CREATE or alter proc usp_crear_trabajador
    @primer_nombre varchar(50),
    @segundo_nombre varchar(50),
    @primer_apellido varchar(50),
    @segundo_apellido varchar(50),
    @username varchar(20),
	@password varchar(355),
	@sueldo decimal(10,2),
	@id_tipo_documento int,
	@numero_documento varchar(10),
	@telefono varchar(10),
	@email varchar(25),
	@id_rol int,
    @estado BIT
AS
BEGIN
    INSERT INTO trabajador (primer_nombre, segundo_nombre, primer_apellido, segundo_apellido, username, password, sueldo, id_tipo_documento, numero_documento, telefono, email, id_rol, estado)
    VALUES (@primer_nombre, @segundo_nombre, @primer_apellido, @segundo_apellido, @username, @password, @sueldo, @id_tipo_documento, @numero_documento, @telefono, @email, @id_rol, 1);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_trabajador
    @id INT,
    @primer_nombre varchar(50),
    @segundo_nombre varchar(50),
    @primer_apellido varchar(50),
    @segundo_apellido varchar(50),
    @username varchar(20),
	@password varchar(355),
	@sueldo decimal(10,2),
	@id_tipo_documento int,
	@numero_documento varchar(10),
	@telefono varchar(10),
	@email varchar(25),
	@id_rol int,
    @estado BIT
AS
BEGIN
    UPDATE trabajador
    SET primer_nombre = @primer_nombre,
		segundo_nombre = @segundo_nombre, 
		primer_apellido = @primer_apellido, 
		segundo_apellido = @segundo_apellido, 
		username = @username, 
		password = @password, 
		sueldo = @sueldo, 
		id_tipo_documento = @id_tipo_documento, 
		numero_documento = @numero_documento, 
		telefono = @telefono, 
		email = @email, 
		id_rol = @id_rol, 
		estado = 1
    WHERE id = @id
END
go
CREATE or alter proc usp_eliminar_trabajador
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE trabajador
    SET estado = 0 
    WHERE Id = @id;
END
go


------------------------------------
------------------------------------

create or alter proc usp_listar_unidad_medida_producto
as
    select *
	from unidad_medida_producto
	where estado = 1;
go
CREATE or alter proc usp_obtener_unidad_medida_por_unidad
    @unidad VARCHAR(40)
AS
BEGIN
    select *
	from unidad_medida_producto
	where unidad =@unidad
END
go
CREATE or alter proc usp_obtener_unidad_medida_por_id
    @id INT
AS
BEGIN
    select *
	from unidad_medida_producto
    WHERE Id = @id and estado = 1;
END
go
CREATE or alter proc usp_crear_unidad_medida_producto
    @unidad VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO unidad_medida_producto( unidad, estado)
	VALUES(@unidad, 1)
END
go

CREATE or alter proc usp_actualizar_unidad_medida_producto
    @id INT,
    @unidad VARCHAR(40),
    @estado BIT
AS
BEGIN
    UPDATE unidad_medida_producto
    SET
        unidad = @unidad,
        estado = 1
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

----------------------
----------------------

create or alter proc usp_listar_categoria_producto
as
    select * from categoria_producto
	where estado = 1;
go

CREATE or alter proc usp_obtener_categoria_producto_por_id
    @id INT
AS
BEGIN
    select *
	from categoria_producto
    WHERE id = @id and estado = 1;
END
go

CREATE or alter proc usp_insertar_categoria_producto
    @categoria VARCHAR(40),
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO categoria_producto(categoria, estado)
    VALUES (@categoria, 1);
    SELECT SCOPE_IDENTITY() AS Id;
END
go
CREATE or alter proc usp_actualizar_categoria_producto
    @id INT,
    @categoria VARCHAR(40),
    @estado BIT
AS
BEGIN
    UPDATE categoria_producto
    SET
        categoria = @categoria,
        estado = 1
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

create or alter proc usp_login_trabajador
@username varchar(20),
@password varchar(355)
as
	select * from 
	trabajador t 
	where t.username = @username and t.password = @password
go


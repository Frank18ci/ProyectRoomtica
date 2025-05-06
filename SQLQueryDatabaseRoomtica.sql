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
create or alter proc usp_listar_caracteristica_habitacion
as
	select id, caracteristica, estado
	from caracteristica_habitacion 
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
select id, caracteristica, estado
	from caracteristica_habitacion 
	where @id = id 
go
CREATE or alter proc usp_crear_caracteristica_habitacion
    @caracteristica VARCHAR(100),
    @estado bit
AS
BEGIN
    INSERT INTO caracteristica_habitacion (caracteristica, estado)
    VALUES (@caracteristica, @estado);

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
	set caracteristica = @caracteristica, estado = @estado
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
    SELECT id_caracteristica_habitacion, id_tipo_habitacion, estado
    FROM caracteristica_habitacion_tipo_habitacion;
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
      AND id_tipo_habitacion = @id_tipo_habitacion;
END
GO

CREATE or alter proc usp_crear_caracteristica_habitacion_tipo_habitacion
    @id_caracteristica_habitacion INT,
    @id_tipo_habitacion INT,
    @estado BIT
AS
BEGIN
    INSERT INTO caracteristica_habitacion_tipo_habitacion (id_caracteristica_habitacion, id_tipo_habitacion, estado)
    VALUES (@id_caracteristica_habitacion, @id_tipo_habitacion, @estado);
END
GO

CREATE or alter proc usp_actualizar_caracteristica_habitacion_tipo_habitacion
    @id_caracteristica_habitacion INT,
    @id_tipo_habitacion INT,
    @estado BIT
AS
BEGIN
    UPDATE caracteristica_habitacion_tipo_habitacion
    SET estado = @estado
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
    FROM categoria_producto;
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
    WHERE id = @id;
END
GO

CREATE or alter proc usp_crear_categoria_producto
	@categoria varchar(40),
    @estado BIT
AS
BEGIN
    INSERT INTO categoria_producto (categoria, estado)
    VALUES (@categoria, @estado);
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
		estado = @estado
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
    WHERE p.id = @id
END
go
CREATE or alter proc usp_obtener_cliente_por_id
    @id INT
AS
BEGIN
    SELECT 
       *
    FROM cliente
    WHERE id = @id
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
    VALUES (@primer_nombre, @segundo_nombre, @primer_apellido, @segundo_apellido, @id_tipo_documento, @numero_documento, @telefono, @email, @fecha_nacimiento, @id_tipo_nacionalidad, @id_tipo_sexo, @estado);

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
        estado = @estado
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
	  cli.primer_apellido + cli.segundo_nombre as Reserva,
	  p.nombre,
	  c.cantidad,
	  c.precio_venta,
	  c.estado
    FROM consumo c
	INNER JOIN reserva r on c.id_reserva = r.id
	INNER JOIN producto p on c.id_producto = p.id
	INNER JOIN cliente cli on r.id_cliente = cli.id
END
go
CREATE or alter proc usp_obtener_consumoDTO_por_id
    @id INT
AS
BEGIN
     SELECT 
      c.id,
	  cli.primer_apellido +' '+ cli.segundo_nombre as Reserva,
	  p.nombre,
	  c.cantidad,
	  c.precio_venta,
	  c.estado
    FROM consumo c
	INNER JOIN reserva r on c.id_reserva = r.id
	INNER JOIN producto p on c.id_producto = p.id
	INNER JOIN cliente cli on r.id_cliente = cli.id
    WHERE c.id = @id
END
go
CREATE or alter proc usp_obtener_consumo_por_id
    @id INT
AS
BEGIN
    SELECT 
       *
    FROM consumo
    WHERE id = @id
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
    VALUES (@id_reserva, @id_producto, @cantidad, @precio_venta, @estado);

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
        estado = @estado
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
    WHERE e.id = @id
END
go
CREATE or alter proc usp_obtener_estacionamiento_por_id
    @id INT
AS
BEGIN
    SELECT 
       *
    FROM estacionamiento
    WHERE id = @id
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
    VALUES (@lugar, @largo, @alto, @ancho, @id_tipo_estacionamiento, @estado);

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
        estado = @estado
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
END
go
CREATE or alter proc usp_listar_estado_habitacion_por_estado
    @estado bit
AS
BEGIN
     SELECT 
       *
    FROM estado_habitacion
    WHERE estado = @estado
END
go
CREATE or alter proc usp_obtener_estado_habitacion_por_id
    @id INT
AS
BEGIN
    SELECT 
        *
    FROM estado_habitacion
    WHERE id = @id
END
GO

CREATE or alter proc usp_crear_estado_habitacion
    @estado_habitacion VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO estado_habitacion (estado_habitacion, estado)
    VALUES (@estado_habitacion, @estado);

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
        estado = @estado
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
    WHERE h.id = @id
END
go
CREATE or alter proc usp_obtener_habitacion_por_id
    @id INT
AS
BEGIN
    SELECT 
      *
    FROM habitacion
    WHERE id = @id
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
    VALUES (@numero, @piso, @precio_diario, @id_tipo, @id_estado, @estado);

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
        estado = @estado
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
    WHERE p.id = @id
END
go
CREATE or alter proc usp_obtener_pago_por_id
    @id INT
AS
BEGIN
    SELECT 
       *
    FROM pago
    WHERE id = @id
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
    VALUES (@id_reserva, @id_tipo_comprobante, @igv, @total_pago, @fecha_emision,@fecha_pago, @estado);

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
        estado = @estado
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
    WHERE p.id = @id
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
    WHERE id = @id
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
    VALUES (@nombre, @id_unidad_medida_producto, @id_categoria_producto, @precio_unico, @cantidad, @estado);

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
        estado = @estado
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
    VALUES (@id_reserva, @id_estacionamiento, @cantidad, @precio_estacionamiento, @estado);

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
        estado = @estado
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
	   c.primer_apellido,
	   t.primer_nombre,
	   tr.tipo,
	   r.fecha_ingreso,
	   r.fecha_salida,
	   r.costo_alojamiento,
	   r.estado
    FROM reserva r
    INNER JOIN habitacion h ON r.id_habitacion = h.id
    INNER JOIN cliente c ON r.id_cliente = c.id
	INNER JOIN trabajador t ON r.id_trabajador = t.id
	INNER JOIN tipo_reserva tr ON r.id_tipo_reserva = tr.id
END
go
CREATE or alter proc usp_obtener_reservaDTO_por_id
    @id INT
AS
BEGIN
    SELECT 
       r.id,
	   h.numero,
	   c.primer_apellido,
	   t.primer_nombre,
	   tr.tipo,
	   r.fecha_ingreso,
	   r.fecha_salida,
	   r.costo_alojamiento,
	   r.estado
    FROM reserva r
    INNER JOIN habitacion h ON r.id_habitacion = h.id
    INNER JOIN cliente c ON r.id_cliente = c.id
	INNER JOIN trabajador t ON r.id_trabajador = t.id
	INNER JOIN tipo_reserva tr ON r.id_tipo_reserva = tr.id
    WHERE r.id = @id
END
go
CREATE or alter proc usp_obtener_reserva_por_id
    @id INT
AS
BEGIN
   SELECT 
      *
    FROM reserva
    WHERE id = @id
END
GO

CREATE or alter proc usp_crear_reserva
    @id_habitacion INT,
    @id_cliente INT,
    @id_trabajador INT,
    @id_tipo_reserva INT,
    @fecha_ingreso date,
	@fecha_salida date,
	@costo_alojamiento decimal(10,2),
    @estado BIT
AS
BEGIN
    INSERT INTO reserva (id_habitacion, id_cliente, id_trabajador, id_tipo_reserva, fecha_ingreso, fecha_salida, costo_alojamiento, estado)
    VALUES (@id_habitacion, @id_cliente, @id_trabajador, @id_tipo_reserva, @fecha_ingreso, @fecha_salida, @costo_alojamiento, @estado);

    SELECT SCOPE_IDENTITY() AS nuevo_id;
END
go
CREATE or alter proc usp_actualizar_reserva
    @id INT,
    @id_habitacion INT,
    @id_cliente INT,
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
        id_cliente = @id_cliente,
        id_trabajador = @id_trabajador,
        id_tipo_reserva = @id_tipo_reserva,
        fecha_ingreso = @fecha_ingreso,
		fecha_salida = @fecha_salida,
		costo_alojamiento = @costo_alojamiento,
        estado = @estado
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

CREATE or alter proc usp_listar_rol_trabajadores
AS
BEGIN
    SELECT 
       *
    FROM rol_trabajador
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
    WHERE id = @id
END
GO

CREATE or alter proc usp_crear_rol_trabajador
    @rol VARCHAR(40),
    @estado bit
AS
BEGIN
    INSERT INTO rol_trabajador (rol, estado)
    VALUES (@rol, @estado);

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
        estado = @estado
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
    WHERE id = @id
END
GO

CREATE or alter proc usp_crear_tipo_comprobante
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_comprobante (tipo, estado)
    VALUES (@tipo, @estado);

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
        estado = @estado
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
    WHERE id = @id
END
GO

CREATE or alter proc usp_crear_tipo_documento
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_documento (tipo, estado)
    VALUES (@tipo, @estado);

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
        estado = @estado
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
    WHERE id = @id
END
GO

CREATE or alter proc usp_crear_tipo_estacionamiento
    @tipo VARCHAR(40),
	@costo decimal(10,2),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_estacionamiento (tipo, costo, estado)
    VALUES (@tipo, @costo, @estado);

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
        estado = @estado
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
    WHERE id = @id
END
GO

CREATE or alter proc usp_crear_tipo_habitacion
    @tipo VARCHAR(40),
	@descripccion VARCHAR(200),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_habitacion (tipo, descripccion, estado)
    VALUES (@tipo, @descripccion, @estado);

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
        estado = @estado
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
    WHERE id = @id
END
GO

CREATE or alter proc usp_crear_tipo_nacionalidad
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_nacionalidad (tipo, estado)
    VALUES (@tipo, @estado);

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
        estado = @estado
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
    WHERE id = @id
END
GO

CREATE or alter proc usp_crear_tipo_reserva
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_reserva (tipo, estado)
    VALUES (@tipo, @estado);

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
        estado = @estado
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
    WHERE id = @id
END
GO

CREATE or alter proc usp_crear_tipo_sexo
    @tipo VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO tipo_sexo (tipo, estado)
    VALUES (@tipo, @estado);

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
        estado = @estado
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
    WHERE t.id = @id
END
go
CREATE or alter proc usp_obtener_trabajador_por_id
    @id INT
AS
BEGIN
   SELECT 
      *
    FROM trabajador
    WHERE id = @id
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
    VALUES (@primer_nombre, @segundo_nombre, @primer_apellido, @segundo_apellido, @username, @password, @sueldo, @id_tipo_documento, @numero_documento, @telefono, @email, @id_rol, @estado);

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
		estado = @estado
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
    WHERE Id = @id;
END
go
CREATE or alter proc usp_crear_unidad_medida_producto
    @unidad VARCHAR(40),
    @estado BIT
AS
BEGIN
    INSERT INTO unidad_medida_producto( unidad, estado)
	VALUES(@unidad, @estado)
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

create or alter proc usp_listar_categoria_producto
as
    select * from categoria_producto
go
CREATE or alter proc usp_insertar_categoria_producto
    @categoria VARCHAR(40),
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
    @categoria VARCHAR(40),
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





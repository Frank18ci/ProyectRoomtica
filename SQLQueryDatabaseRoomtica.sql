use master
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



create or alter proc usp_listar_caracteristica_habitacion
as
	select * from caracteristica_habitacion
go

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


exec usp_listar_caracteristica_habitacion
go


INSERT INTO tipo_habitacion (tipo, descripccion, estado) VALUES
('Individual', 'Habitación para una persona con cama individual.', 1),
('Doble', 'Habitación con dos camas individuales o una doble.', 1),
('Suite', 'Habitación amplia con sala y comodidades extras.', 1),
('Familiar', 'Habitación para familias, con varias camas.', 1),
('Económica', 'Habitación básica con servicios mínimos.', 1),
('Premium', 'Habitación de lujo con servicios exclusivos.', 1);
go


create or alter proc usp_listar_tipo_habitacion
as
	select * from tipo_habitacion
go

exec usp_listar_tipo_habitacion
go
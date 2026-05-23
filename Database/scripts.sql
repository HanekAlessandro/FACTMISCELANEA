use master
go

if db_id('MiscelaneaMaster') is not null
drop database MiscelaneaMaster
go

create database MiscelaneaMaster
go

use MiscelaneaMaster
go

/* Tabla empresa */

create table Empresa (
    id_empresa int primary key identity(1,1),
    nombre_comercial varchar(100) not null,
    nit_cedula_juridica varchar(50) not null,
    direccion varchar(255),
    telefono varchar(8),
    mensaje_pie_factura varchar(255)
)
go

/* Tabla categorías */

create table Categorias (
    id_categoria int primary key identity(1,1),
    nombre_categoria varchar(50) not null
)
go

/* Tabla métodos pago */

create table Metodos_Pago (
    id_metodo int primary key identity(1,1),
    nombre_metodo varchar(30) not null
)
go

/* Tabla usuarios */

create table Usuarios (
    id_usuario int primary key identity(1,1),
    nombre_completo varchar(100) not null,
    nombre_usuario varchar(50) not null,
    password varchar(100) not null,
    rol varchar(20)
)
go

/* Tabla proveedores */

create table Proveedores (
    id_proveedor int primary key identity(1,1),
    nombre_empresa varchar(100) not null,
    contacto_nombre varchar(100),
    telefono varchar(8),
    direccion varchar(255)
)
go

/* Tabla clientes */

create table Clientes (
    id_cliente int primary key identity(1,1),
    identificacion varchar(20),
    nombre varchar(100),
    direccion varchar(255),
    telefono varchar(8)
)
go

/* Tabla productos */

create table Productos (
    id_producto int primary key identity(1,1),
    codigo_barras varchar(50) not null,
    nombre varchar(100) not null,
    descripcion varchar(255),
    id_categoria int,
    iva_porcentaje decimal(5,2),
    stock_actual int,
    stock_minimo int,

    foreign key (id_categoria)
    references Categorias(id_categoria)
)
go

/* Tabla precios promociones */

create table Precios_Promociones (
    id_precio int primary key identity(1,1),
    id_producto int,
    costo_compra decimal(10,2),
    precio_final decimal(10,2),
    es_promocional bit,
    fecha_inicio_promo date,
    fecha_fin_promo date,

    foreign key (id_producto)
    references Productos(id_producto)
)
go

/* Tabla facturas */

create table Facturas (
    id_factura int primary key identity(1,1),
    folio varchar(20),
    fecha_hora datetime default getdate(),
    id_cliente int,
    id_metodo_pago int,
    id_usuario int,
    subtotal decimal(10,2),
    iva decimal(10,2),
    total decimal(10,2),

    foreign key (id_cliente)
    references Clientes(id_cliente),

    foreign key (id_metodo_pago)
    references Metodos_Pago(id_metodo),

    foreign key (id_usuario)
    references Usuarios(id_usuario)
)
go

/* Tabla detalle factura */

create table Factura_Detalle (
    id_detalle int primary key identity(1,1),
    id_factura int,
    id_producto int,
    cantidad int,
    precio_unitario decimal(10,2),
    subtotal decimal(10,2),

    foreign key (id_factura)
    references Facturas(id_factura),

    foreign key (id_producto)
    references Productos(id_producto)
)
go

/* Tabla devoluciones */

create table Devoluciones (
    id_devolucion int primary key identity(1,1),
    id_factura int,
    id_producto int,
    cantidad int,
    motivo varchar(255),
    fecha_devolucion datetime default getdate(),

    foreign key (id_factura)
    references Facturas(id_factura),

    foreign key (id_producto)
    references Productos(id_producto)
)
go

/* Tabla kardex */

create table Kardex (
    id_movimiento int primary key identity(1,1),
    id_producto int,
    tipo_movimiento varchar(20),
    cantidad int,
    fecha datetime default getdate(),
    observaciones varchar(255),

    foreign key (id_producto)
    references Productos(id_producto)
)
go

/* Trigger ventas */

create trigger trg_ProcesarVenta
on Factura_Detalle
after insert
as
begin

    update P
    set P.stock_actual = P.stock_actual - i.cantidad
    from Productos P
    inner join inserted i
        on P.id_producto = i.id_producto

    insert into Kardex (
        id_producto,
        tipo_movimiento,
        cantidad,
        observaciones
    )
    select
        id_producto,
        'SALIDA',
        cantidad,
        'Venta realizada'
    from inserted

end
go

/* Vista reporte mensual */

create view Vista_Reporte_Mensual
as

select
    format(fecha_hora,'yyyy-MM') as Mes,
    count(id_factura) as Total_Ventas,
    sum(total) as Total_Ingresos
from Facturas
group by format(fecha_hora,'yyyy-MM')
go

/* Vista alerta stock */

create view Vista_Alerta_Stock
as

select
    codigo_barras,
    nombre,
    stock_actual,
    stock_minimo
from Productos
where stock_actual <= stock_minimo
go
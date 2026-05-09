USE [master]
GO

IF DB_ID('MiscelaneaMaster') IS NOT NULL
DROP DATABASE [MiscelaneaMaster]
GO

CREATE DATABASE [MiscelaneaMaster]
GO

ALTER DATABASE [MiscelaneaMaster]
SET COMPATIBILITY_LEVEL = 150
GO

USE [MiscelaneaMaster]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =========================================================
-- TABLA EMPRESA
-- =========================================================

CREATE TABLE Empresa(

    id_empresa INT PRIMARY KEY DEFAULT 1,

    nombre_comercial VARCHAR(100) NOT NULL,

    nit_cedula_juridica VARCHAR(50) NOT NULL,

    direccion VARCHAR(MAX),

    telefono VARCHAR(8)
    CHECK(
        telefono NOT LIKE '%[^0-9]%'
        AND LEN(telefono)=8
    ),

    mensaje_pie_factura VARCHAR(255)

)
GO

-- =========================================================
-- TABLA CATEGORIAS
-- =========================================================

CREATE TABLE Categorias(

    id_categoria INT PRIMARY KEY IDENTITY(1,1),

    nombre_categoria VARCHAR(50)
    NOT NULL
    UNIQUE

)
GO

-- =========================================================
-- TABLA METODOS DE PAGO
-- =========================================================

CREATE TABLE Metodos_Pago(

    id_metodo INT PRIMARY KEY IDENTITY(1,1),

    nombre_metodo VARCHAR(30)
    NOT NULL
    UNIQUE

)
GO

-- =========================================================
-- TABLA USUARIOS
-- =========================================================

CREATE TABLE Usuarios(

    id_usuario INT PRIMARY KEY IDENTITY(1,1),

    nombre_completo VARCHAR(100)
    NOT NULL,

    nombre_usuario VARCHAR(50)
    UNIQUE
    NOT NULL,

    password_hash VARCHAR(255)
    NOT NULL,

    rol VARCHAR(20)
    CHECK(
        rol IN ('Administrador','Cajero')
    )

)
GO

-- =========================================================
-- TABLA PROVEEDORES
-- =========================================================

CREATE TABLE Proveedores(

    id_proveedor INT PRIMARY KEY IDENTITY(1,1),

    nombre_empresa VARCHAR(100)
    NOT NULL,

    contacto_nombre VARCHAR(100),

    telefono VARCHAR(8)
    CHECK(
        telefono NOT LIKE '%[^0-9]%'
        AND LEN(telefono)=8
    ),

    direccion VARCHAR(MAX)

)
GO

-- =========================================================
-- TABLA CLIENTES
-- =========================================================

CREATE TABLE Clientes(

    id_cliente INT PRIMARY KEY IDENTITY(1,1),

    identificacion VARCHAR(20)
    UNIQUE,

    nombre VARCHAR(100)
    DEFAULT 'Público General',

    direccion VARCHAR(255),

    telefono VARCHAR(8)
    CHECK(
        telefono NOT LIKE '%[^0-9]%'
        AND LEN(telefono)=8
    ),

    puntos_lealtad INT
    DEFAULT 0
    CHECK(puntos_lealtad >= 0)

)
GO

-- =========================================================
-- TABLA PRODUCTOS
-- =========================================================

CREATE TABLE Productos(

    id_producto INT PRIMARY KEY IDENTITY(1,1),

    codigo_barras VARCHAR(50)
    UNIQUE
    NOT NULL,

    nombre VARCHAR(100)
    NOT NULL,

    descripcion VARCHAR(255),

    id_categoria INT,

    iva_porcentaje DECIMAL(5,2)
    DEFAULT 15.00
    CHECK(iva_porcentaje >= 0),

    stock_actual INT
    DEFAULT 0
    CHECK(stock_actual >= 0),

    stock_minimo INT
    DEFAULT 5
    CHECK(stock_minimo >= 0),

    CONSTRAINT FK_Productos_Categorias
    FOREIGN KEY(id_categoria)
    REFERENCES Categorias(id_categoria)

)
GO

-- =========================================================
-- TABLA PRECIOS Y PROMOCIONES
-- =========================================================

CREATE TABLE Precios_Promociones(

    id_precio INT PRIMARY KEY IDENTITY(1,1),

    id_producto INT NOT NULL,

    costo_compra DECIMAL(10,2)
    NOT NULL
    CHECK(costo_compra >= 0),

    precio_sugerido DECIMAL(10,2)
    CHECK(precio_sugerido >= 0),

    precio_final DECIMAL(10,2)
    NOT NULL
    CHECK(precio_final >= 0),

    es_promocional BIT DEFAULT 0,

    fecha_inicio_promo DATE,

    fecha_fin_promo DATE,

    CONSTRAINT FK_Precios_Productos
    FOREIGN KEY(id_producto)
    REFERENCES Productos(id_producto)

)
GO

-- =========================================================
-- TABLA FACTURAS
-- =========================================================

CREATE TABLE Facturas(

    id_factura INT PRIMARY KEY IDENTITY(1,1),

    folio VARCHAR(20)
    UNIQUE
    NOT NULL,

    fecha_hora DATETIME
    DEFAULT GETDATE(),

    id_cliente INT,

    id_metodo_pago INT,

    id_usuario INT,

    subtotal_sin_iva DECIMAL(10,2)
    CHECK(subtotal_sin_iva >= 0),

    total_iva DECIMAL(10,2)
    CHECK(total_iva >= 0),

    total_final DECIMAL(10,2)
    CHECK(total_final >= 0),

    CONSTRAINT FK_Facturas_Clientes
    FOREIGN KEY(id_cliente)
    REFERENCES Clientes(id_cliente),

    CONSTRAINT FK_Facturas_MetodosPago
    FOREIGN KEY(id_metodo_pago)
    REFERENCES Metodos_Pago(id_metodo),

    CONSTRAINT FK_Facturas_Usuarios
    FOREIGN KEY(id_usuario)
    REFERENCES Usuarios(id_usuario)

)
GO

-- =========================================================
-- TABLA DETALLE FACTURA
-- =========================================================

CREATE TABLE Factura_Detalle(

    id_detalle INT PRIMARY KEY IDENTITY(1,1),

    id_factura INT NOT NULL,

    id_producto INT NOT NULL,

    cantidad INT
    NOT NULL
    CHECK(cantidad > 0),

    precio_unitario_aplicado DECIMAL(10,2)
    CHECK(precio_unitario_aplicado >= 0),

    iva_aplicado DECIMAL(10,2)
    CHECK(iva_aplicado >= 0),

    subtotal_linea DECIMAL(10,2)
    CHECK(subtotal_linea >= 0),

    CONSTRAINT FK_Detalle_Factura
    FOREIGN KEY(id_factura)
    REFERENCES Facturas(id_factura),

    CONSTRAINT FK_Detalle_Producto
    FOREIGN KEY(id_producto)
    REFERENCES Productos(id_producto)

)
GO

-- =========================================================
-- TABLA DEVOLUCIONES
-- =========================================================

CREATE TABLE Devoluciones(

    id_devolucion INT PRIMARY KEY IDENTITY(1,1),

    id_factura INT,

    id_producto INT,

    cantidad INT
    NOT NULL
    CHECK(cantidad > 0),

    motivo VARCHAR(255),

    fecha_devolucion DATETIME
    DEFAULT GETDATE(),

    id_usuario_autoriza INT,

    CONSTRAINT FK_Devoluciones_Factura
    FOREIGN KEY(id_factura)
    REFERENCES Facturas(id_factura),

    CONSTRAINT FK_Devoluciones_Producto
    FOREIGN KEY(id_producto)
    REFERENCES Productos(id_producto),

    CONSTRAINT FK_Devoluciones_Usuario
    FOREIGN KEY(id_usuario_autoriza)
    REFERENCES Usuarios(id_usuario)

)
GO

-- =========================================================
-- TABLA KARDEX
-- =========================================================

CREATE TABLE Kardex(

    id_movimiento INT PRIMARY KEY IDENTITY(1,1),

    id_producto INT NOT NULL,

    tipo_movimiento VARCHAR(20)
    CHECK(
        tipo_movimiento IN
        (
            'ENTRADA',
            'SALIDA',
            'DEVOLUCION',
            'AJUSTE'
        )
    ),

    cantidad INT NOT NULL,

    fecha DATETIME
    DEFAULT GETDATE(),

    referencia_id INT,

    observaciones VARCHAR(255),

    CONSTRAINT FK_Kardex_Productos
    FOREIGN KEY(id_producto)
    REFERENCES Productos(id_producto)

)
GO

-- =========================================================
-- TRIGGER VENTAS
-- =========================================================

CREATE TRIGGER trg_ProcesarVenta
ON Factura_Detalle
AFTER INSERT
AS
BEGIN

    SET NOCOUNT ON;

    IF EXISTS(
        SELECT 1
        FROM Productos P
        INNER JOIN inserted i
        ON P.id_producto = i.id_producto
        WHERE P.stock_actual < i.cantidad
    )
    BEGIN

        RAISERROR('Stock insuficiente.',16,1);

        ROLLBACK TRANSACTION;

        RETURN;

    END

    UPDATE P
    SET P.stock_actual =
    P.stock_actual - i.cantidad

    FROM Productos P

    INNER JOIN inserted i
    ON P.id_producto = i.id_producto;

    INSERT INTO Kardex
    (
        id_producto,
        tipo_movimiento,
        cantidad,
        referencia_id,
        observaciones
    )

    SELECT
        id_producto,
        'SALIDA',
        cantidad,
        id_factura,
        'Venta Facturada'

    FROM inserted;

END
GO

-- =========================================================
-- VISTA REPORTE MENSUAL
-- =========================================================

CREATE VIEW Vista_Reporte_Mensual
AS

SELECT

    FORMAT(fecha_hora,'yyyy-MM') AS Mes,

    COUNT(DISTINCT id_factura)
    AS Total_Ventas_Contadas,

    SUM(subtotal_sin_iva)
    AS Subtotal_Neto,

    SUM(total_iva)
    AS Total_Impuestos,

    SUM(total_final)
    AS Ingreso_Bruto

FROM Facturas

GROUP BY FORMAT(fecha_hora,'yyyy-MM')
GO

-- =========================================================
-- VISTA ALERTA STOCK
-- =========================================================

CREATE VIEW Vista_Alerta_Stock
AS

SELECT

    codigo_barras,
    nombre,
    stock_actual,
    stock_minimo

FROM Productos

WHERE stock_actual <= stock_minimo
GO
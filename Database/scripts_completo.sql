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
-- TABLA EMPRESA (MEJORADA)
-- =========================================================

CREATE TABLE Empresa(
    id_empresa INT PRIMARY KEY DEFAULT 1,
    nombre_comercial VARCHAR(100) NOT NULL,
    nombre_fiscal VARCHAR(100) NULL,
    nit_cedula_juridica VARCHAR(50) NOT NULL,
    direccion VARCHAR(MAX),
    telefono VARCHAR(8)
    CHECK(
        telefono NOT LIKE '%[^0-9]%'
        AND LEN(telefono)=8
    ),
    telefono2 VARCHAR(8) NULL
    CHECK(telefono2 NOT LIKE '%[^0-9]%' AND LEN(telefono2)=8),
    email VARCHAR(100) NULL,
    sitio_web VARCHAR(255) NULL,
    logo_url VARCHAR(255) NULL,
    mensaje_pie_factura VARCHAR(255),
    regimen_fiscal VARCHAR(50) DEFAULT 'General',
    obligado_contabilidad BIT DEFAULT 0,
    activo BIT DEFAULT 1,
    fecha_registro DATETIME DEFAULT GETDATE()
)
GO

-- =========================================================
-- TABLA CATEGORIAS
-- =========================================================

CREATE TABLE Categorias(
    id_categoria INT PRIMARY KEY IDENTITY(1,1),
    nombre_categoria VARCHAR(50) NOT NULL UNIQUE,
    descripcion VARCHAR(255) NULL,
    activo BIT DEFAULT 1
)
GO

-- =========================================================
-- TABLA METODOS DE PAGO
-- =========================================================

CREATE TABLE Metodos_Pago(
    id_metodo INT PRIMARY KEY IDENTITY(1,1),
    nombre_metodo VARCHAR(30) NOT NULL UNIQUE,
    activo BIT DEFAULT 1
)
GO

-- =========================================================
-- TABLA USUARIOS (MEJORADA CON LOGIN)
-- =========================================================

CREATE TABLE Usuarios(
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
    nombre_completo VARCHAR(100) NOT NULL,
    nombre_usuario VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARBINARY(256) NOT NULL,
    rol VARCHAR(20) DEFAULT 'Cajero'
    CHECK(rol IN ('Administrador','Cajero')),
    estado VARCHAR(20) DEFAULT 'Activo'
    CHECK(estado IN ('Activo','Inactivo','Bloqueado')),
    intentos_fallidos INT DEFAULT 0,
    ultimo_acceso DATETIME NULL,
    ultimo_cambio_password DATETIME NULL,
    token_recuperacion VARCHAR(255) NULL,
    token_expiracion DATETIME NULL,
    id_empresa INT NULL DEFAULT 1,
    registrado_por INT NULL,
    fecha_registro DATETIME DEFAULT GETDATE()
)
GO

-- =========================================================
-- TABLA LOGS_LOGIN (NUEVA)
-- =========================================================

CREATE TABLE Logs_Login(
    id_log INT PRIMARY KEY IDENTITY(1,1),
    id_usuario INT NULL,
    nombre_usuario VARCHAR(50),
    ip_address VARCHAR(45),
    user_agent VARCHAR(255),
    resultado VARCHAR(20) CHECK(resultado IN ('Exitoso','Fallido','Bloqueado')),
    fecha_intento DATETIME DEFAULT GETDATE()
)
GO

-- =========================================================
-- TABLA PROVEEDORES (MEJORADA)
-- =========================================================

CREATE TABLE Proveedores(
    id_proveedor INT PRIMARY KEY IDENTITY(1,1),
    nombre_empresa VARCHAR(100) NOT NULL,
    nit VARCHAR(50) NULL UNIQUE,
    contacto_nombre VARCHAR(100),
    contacto_email VARCHAR(100),
    telefono VARCHAR(8)
    CHECK(
        telefono NOT LIKE '%[^0-9]%'
        AND LEN(telefono)=8
    ),
    telefono_alterno VARCHAR(8) NULL
    CHECK(telefono_alterno NOT LIKE '%[^0-9]%' AND LEN(telefono_alterno)=8),
    direccion VARCHAR(MAX),
    direccion_alterna VARCHAR(MAX) NULL,
    sitio_web VARCHAR(255) NULL,
    condiciones_pago VARCHAR(50) DEFAULT 'Contado',
    credito_maximo DECIMAL(10,2) DEFAULT 0,
    saldo_pendiente DECIMAL(10,2) DEFAULT 0,
    calificacion INT DEFAULT 3 CHECK(calificacion BETWEEN 1 AND 5),
    activo BIT DEFAULT 1,
    id_usuario_registro INT NULL,
    fecha_registro DATETIME DEFAULT GETDATE()
)
GO

-- =========================================================
-- TABLA CLIENTES (MEJORADA)
-- =========================================================

CREATE TABLE Clientes(
    id_cliente INT PRIMARY KEY IDENTITY(1,1),
    identificacion VARCHAR(20) UNIQUE,
    nombre VARCHAR(100) DEFAULT 'Público General',
    email VARCHAR(100) NULL,
    direccion VARCHAR(255),
    telefono VARCHAR(8)
    CHECK(
        telefono NOT LIKE '%[^0-9]%'
        AND LEN(telefono)=8
    ),
    puntos_lealtad INT DEFAULT 0 CHECK(puntos_lealtad >= 0),
    activo BIT DEFAULT 1,
    fecha_registro DATETIME DEFAULT GETDATE(),
    id_usuario_registro INT NULL
)
GO

-- =========================================================
-- TABLA PRODUCTOS (MEJORADA)
-- =========================================================

CREATE TABLE Productos(
    id_producto INT PRIMARY KEY IDENTITY(1,1),
    codigo_barras VARCHAR(50) UNIQUE NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255),
    id_categoria INT,
    id_proveedor_preferente INT NULL,
    iva_porcentaje DECIMAL(5,2) DEFAULT 15.00 CHECK(iva_porcentaje >= 0),
    stock_actual INT DEFAULT 0 CHECK(stock_actual >= 0),
    stock_minimo INT DEFAULT 5 CHECK(stock_minimo >= 0),
    ultimo_costo_compra DECIMAL(10,2) NULL,
    costo_promedio DECIMAL(10,2) NULL,
    activo BIT DEFAULT 1,
    fecha_registro DATETIME DEFAULT GETDATE(),
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
    costo_compra DECIMAL(10,2) NOT NULL CHECK(costo_compra >= 0),
    precio_sugerido DECIMAL(10,2) CHECK(precio_sugerido >= 0),
    precio_final DECIMAL(10,2) NOT NULL CHECK(precio_final >= 0),
    es_promocional BIT DEFAULT 0,
    fecha_inicio_promo DATE,
    fecha_fin_promo DATE,
    fecha_actualizacion DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Precios_Productos
    FOREIGN KEY(id_producto)
    REFERENCES Productos(id_producto)
)
GO

-- =========================================================
-- TABLA COMPRAS (NUEVA - CONEXIÓN PROVEEDORES)
-- =========================================================

CREATE TABLE Compras(
    id_compra INT PRIMARY KEY IDENTITY(1,1),
    folio VARCHAR(20) UNIQUE NOT NULL,
    fecha_hora DATETIME DEFAULT GETDATE(),
    id_proveedor INT NOT NULL,
    id_usuario INT NOT NULL,
    id_empresa INT DEFAULT 1,
    subtotal DECIMAL(10,2) CHECK(subtotal >= 0),
    iva_total DECIMAL(10,2) CHECK(iva_total >= 0),
    total_compra DECIMAL(10,2) CHECK(total_compra >= 0),
    factura_proveedor VARCHAR(50),
    tipo_comprobante VARCHAR(20) DEFAULT 'Factura',
    estado VARCHAR(20) DEFAULT 'Completada',
    observaciones VARCHAR(255),
    CONSTRAINT FK_Compras_Proveedores
    FOREIGN KEY(id_proveedor)
    REFERENCES Proveedores(id_proveedor),
    CONSTRAINT FK_Compras_Usuarios
    FOREIGN KEY(id_usuario)
    REFERENCES Usuarios(id_usuario)
)
GO

-- =========================================================
-- TABLA DETALLE_COMPRAS (NUEVA)
-- =========================================================

CREATE TABLE Detalle_Compras(
    id_detalle_compra INT PRIMARY KEY IDENTITY(1,1),
    id_compra INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL CHECK(cantidad > 0),
    costo_unitario DECIMAL(10,2) CHECK(costo_unitario >= 0),
    iva_porcentaje DECIMAL(5,2) DEFAULT 15.00,
    descuento_unitario DECIMAL(10,2) DEFAULT 0,
    subtotal_linea DECIMAL(10,2) CHECK(subtotal_linea >= 0),
    CONSTRAINT FK_DetalleCompra_Compra
    FOREIGN KEY(id_compra)
    REFERENCES Compras(id_compra)
    ON DELETE CASCADE,
    CONSTRAINT FK_DetalleCompra_Producto
    FOREIGN KEY(id_producto)
    REFERENCES Productos(id_producto)
)
GO

-- =========================================================
-- TABLA FACTURAS (MEJORADA)
-- =========================================================

CREATE TABLE Facturas(
    id_factura INT PRIMARY KEY IDENTITY(1,1),
    folio VARCHAR(20) UNIQUE NOT NULL,
    fecha_hora DATETIME DEFAULT GETDATE(),
    id_cliente INT,
    id_metodo_pago INT,
    id_usuario INT,
    id_empresa INT DEFAULT 1,
    subtotal_sin_iva DECIMAL(10,2) CHECK(subtotal_sin_iva >= 0),
    total_iva DECIMAL(10,2) CHECK(total_iva >= 0),
    total_final DECIMAL(10,2) CHECK(total_final >= 0),
    anulada BIT DEFAULT 0,
    motivo_anulacion VARCHAR(255),
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
    cantidad INT NOT NULL CHECK(cantidad > 0),
    precio_unitario_aplicado DECIMAL(10,2) CHECK(precio_unitario_aplicado >= 0),
    iva_aplicado DECIMAL(10,2) CHECK(iva_aplicado >= 0),
    subtotal_linea DECIMAL(10,2) CHECK(subtotal_linea >= 0),
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
    cantidad INT NOT NULL CHECK(cantidad > 0),
    motivo VARCHAR(255),
    fecha_devolucion DATETIME DEFAULT GETDATE(),
    id_usuario_autoriza INT,
    procesada_en_kardex BIT DEFAULT 0,
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
-- TABLA KARDEX (MEJORADA)
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
            'AJUSTE',
            'COMPRA'
        )
    ),
    cantidad INT NOT NULL,
    fecha DATETIME DEFAULT GETDATE(),
    referencia_id INT,
    referencia_tipo VARCHAR(20) NULL,
    observaciones VARCHAR(255),
    CONSTRAINT FK_Kardex_Productos
    FOREIGN KEY(id_producto)
    REFERENCES Productos(id_producto)
)
GO

-- =========================================================
-- FOREIGN KEYS ADICIONALES
-- =========================================================

ALTER TABLE Usuarios ADD CONSTRAINT FK_Usuarios_Empresa
    FOREIGN KEY (id_empresa) REFERENCES Empresa(id_empresa);

ALTER TABLE Usuarios ADD CONSTRAINT FK_Usuarios_RegistradoPor
    FOREIGN KEY (registrado_por) REFERENCES Usuarios(id_usuario);

ALTER TABLE Proveedores ADD CONSTRAINT FK_Proveedores_UsuarioRegistro
    FOREIGN KEY (id_usuario_registro) REFERENCES Usuarios(id_usuario);

ALTER TABLE Clientes ADD CONSTRAINT FK_Clientes_UsuarioRegistro
    FOREIGN KEY (id_usuario_registro) REFERENCES Usuarios(id_usuario);

ALTER TABLE Productos ADD CONSTRAINT FK_Productos_ProveedorPreferente
    FOREIGN KEY (id_proveedor_preferente) REFERENCES Proveedores(id_proveedor);

ALTER TABLE Logs_Login ADD CONSTRAINT FK_LogsLogin_Usuario
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario);
GO

-- =========================================================
-- TRIGGER VENTAS (MEJORADO)
-- =========================================================

CREATE OR ALTER TRIGGER trg_ProcesarVenta
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
    SET P.stock_actual = P.stock_actual - i.cantidad
    FROM Productos P
    INNER JOIN inserted i
    ON P.id_producto = i.id_producto;

    INSERT INTO Kardex
    (
        id_producto,
        tipo_movimiento,
        cantidad,
        referencia_id,
        referencia_tipo,
        observaciones
    )
    SELECT
        id_producto,
        'SALIDA',
        cantidad,
        id_factura,
        'FACTURA',
        'Venta Facturada'
    FROM inserted;
END
GO

-- =========================================================
-- TRIGGER COMPRAS (NUEVO)
-- =========================================================

CREATE OR ALTER TRIGGER trg_ProcesarCompra
ON Detalle_Compras
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE P
    SET P.stock_actual = P.stock_actual + i.cantidad,
        P.ultimo_costo_compra = i.costo_unitario
    FROM Productos P
    INNER JOIN inserted i ON P.id_producto = i.id_producto;
    
    INSERT INTO Kardex
    (
        id_producto,
        tipo_movimiento,
        cantidad,
        referencia_id,
        referencia_tipo,
        observaciones
    )
    SELECT
        id_producto,
        'COMPRA',
        cantidad,
        id_compra,
        'COMPRA',
        'Compra a proveedor'
    FROM inserted;
END
GO

-- =========================================================
-- TRIGGER ACTUALIZAR SALDO PROVEEDOR
-- =========================================================

CREATE OR ALTER TRIGGER trg_ActualizarSaldoProveedor
ON Compras
AFTER INSERT
AS
BEGIN
    UPDATE P
    SET P.saldo_pendiente = P.saldo_pendiente + i.total_compra
    FROM Proveedores P
    INNER JOIN inserted i ON P.id_proveedor = i.id_proveedor;
END
GO

-- =========================================================
-- VISTA REPORTE MENSUAL
-- =========================================================

CREATE VIEW Vista_Reporte_Mensual
AS
SELECT
    FORMAT(fecha_hora,'yyyy-MM') AS Mes,
    COUNT(DISTINCT id_factura) AS Total_Ventas_Contadas,
    SUM(subtotal_sin_iva) AS Subtotal_Neto,
    SUM(total_iva) AS Total_Impuestos,
    SUM(total_final) AS Ingreso_Bruto
FROM Facturas
WHERE anulada = 0
GROUP BY FORMAT(fecha_hora,'yyyy-MM')
GO

-- =========================================================
-- VISTA ALERTA STOCK (MEJORADA)
-- =========================================================

CREATE VIEW Vista_Alerta_Stock
AS
SELECT
    P.codigo_barras,
    P.nombre,
    P.stock_actual,
    P.stock_minimo,
    PROV.nombre_empresa AS ProveedorPreferente,
    PROV.telefono AS TelefonoProveedor,
    PROV.contacto_email AS EmailProveedor
FROM Productos P
LEFT JOIN Proveedores PROV ON P.id_proveedor_preferente = PROV.id_proveedor
WHERE P.stock_actual <= P.stock_minimo AND P.activo = 1
GO

-- =========================================================
-- VISTA PROVEEDORES CON PRODUCTOS
-- =========================================================

CREATE VIEW Vista_Proveedores_Productos
AS
SELECT 
    P.id_proveedor,
    P.nombre_empresa AS Proveedor,
    P.nit,
    P.contacto_nombre,
    P.contacto_email,
    P.telefono,
    P.activo,
    P.calificacion,
    PR.id_producto,
    PR.codigo_barras,
    PR.nombre AS Producto,
    PR.stock_actual,
    PR.stock_minimo,
    PC.precio_final,
    PC.costo_compra
FROM Proveedores P
LEFT JOIN Productos PR ON P.id_proveedor = PR.id_proveedor_preferente
LEFT JOIN Precios_Promociones PC ON PR.id_producto = PC.id_producto
WHERE P.activo = 1
GO

-- =========================================================
-- VISTA LOGS ACCESO
-- =========================================================

CREATE VIEW Vista_Logs_Acceso
AS
SELECT
    L.id_log,
    L.nombre_usuario,
    U.nombre_completo,
    U.rol,
    L.ip_address,
    L.user_agent,
    L.resultado,
    L.fecha_intento
FROM Logs_Login L
LEFT JOIN Usuarios U ON L.id_usuario = U.id_usuario
GO

-- =========================================================
-- PROCEDIMIENTOS DE LOGIN Y REGISTRO
-- =========================================================

CREATE PROCEDURE sp_RegistrarUsuario
    @nombre_completo VARCHAR(100),
    @nombre_usuario VARCHAR(50),
    @email VARCHAR(100),
    @password VARCHAR(255),
    @rol VARCHAR(20),
    @registrado_por INT,
    @resultado VARCHAR(100) OUTPUT,
    @nuevo_id INT OUTPUT
AS
BEGIN
    BEGIN TRY
        IF EXISTS(SELECT 1 FROM Usuarios WHERE nombre_usuario = @nombre_usuario)
        BEGIN
            SET @resultado = 'El nombre de usuario ya existe';
            SET @nuevo_id = 0;
            RETURN;
        END
        
        IF EXISTS(SELECT 1 FROM Usuarios WHERE email = @email)
        BEGIN
            SET @resultado = 'El email ya está registrado';
            SET @nuevo_id = 0;
            RETURN;
        END
        
        DECLARE @rol_registrador VARCHAR(20);
        SELECT @rol_registrador = rol FROM Usuarios WHERE id_usuario = @registrado_por;
        
        IF @rol_registrador != 'Administrador'
        BEGIN
            SET @resultado = 'Solo administradores pueden registrar nuevos usuarios';
            SET @nuevo_id = 0;
            RETURN;
        END
        
        INSERT INTO Usuarios (
            nombre_completo, nombre_usuario, email, password_hash, 
            rol, registrado_por, ultimo_cambio_password, id_empresa
        )
        VALUES (
            @nombre_completo, @nombre_usuario, @email, HASHBYTES('SHA2_256', @password),
            @rol, @registrado_por, GETDATE(), 1
        );
        
        SET @nuevo_id = SCOPE_IDENTITY();
        SET @resultado = 'Usuario registrado exitosamente';
    END TRY
    BEGIN CATCH
        SET @resultado = ERROR_MESSAGE();
        SET @nuevo_id = 0;
    END CATCH
END
GO

CREATE PROCEDURE sp_LoginUsuario
    @nombre_usuario VARCHAR(50),
    @password VARCHAR(255),
    @ip_address VARCHAR(45),
    @user_agent VARCHAR(255),
    @resultado VARCHAR(100) OUTPUT,
    @usuario_id INT OUTPUT,
    @rol VARCHAR(20) OUTPUT,
    @nombre_completo VARCHAR(100) OUTPUT
AS
BEGIN
    DECLARE @password_hash VARBINARY(256);
    DECLARE @estado VARCHAR(20);
    DECLARE @intentos INT;
    
    SELECT 
        @usuario_id = id_usuario,
        @password_hash = password_hash,
        @estado = estado,
        @intentos = intentos_fallidos,
        @nombre_completo = nombre_completo,
        @rol = rol
    FROM Usuarios 
    WHERE nombre_usuario = @nombre_usuario;
    
    IF @usuario_id IS NULL
    BEGIN
        INSERT INTO Logs_Login (nombre_usuario, ip_address, user_agent, resultado)
        VALUES (@nombre_usuario, @ip_address, @user_agent, 'Fallido');
        
        SET @resultado = 'Usuario o contraseña incorrectos';
        SET @usuario_id = 0;
        SET @rol = NULL;
        SET @nombre_completo = NULL;
        RETURN;
    END
    
    IF @estado = 'Bloqueado'
    BEGIN
        INSERT INTO Logs_Login (id_usuario, nombre_usuario, ip_address, user_agent, resultado)
        VALUES (@usuario_id, @nombre_usuario, @ip_address, @user_agent, 'Bloqueado');
        
        SET @resultado = 'Usuario bloqueado. Contacte al administrador';
        SET @usuario_id = 0;
        SET @rol = NULL;
        SET @nombre_completo = NULL;
        RETURN;
    END
    
    IF @password_hash = HASHBYTES('SHA2_256', @password)
    BEGIN
        UPDATE Usuarios 
        SET intentos_fallidos = 0,
            ultimo_acceso = GETDATE()
        WHERE id_usuario = @usuario_id;
        
        INSERT INTO Logs_Login (id_usuario, nombre_usuario, ip_address, user_agent, resultado)
        VALUES (@usuario_id, @nombre_usuario, @ip_address, @user_agent, 'Exitoso');
        
        SET @resultado = 'Login exitoso';
    END
    ELSE
    BEGIN
        SET @intentos = @intentos + 1;
        
        UPDATE Usuarios 
        SET intentos_fallidos = @intentos,
            estado = CASE WHEN @intentos >= 5 THEN 'Bloqueado' ELSE estado END
        WHERE id_usuario = @usuario_id;
        
        INSERT INTO Logs_Login (id_usuario, nombre_usuario, ip_address, user_agent, resultado)
        VALUES (@usuario_id, @nombre_usuario, @ip_address, @user_agent, 'Fallido');
        
        SET @resultado = CASE 
            WHEN @intentos >= 5 THEN 'Usuario bloqueado por múltiples intentos fallidos'
            ELSE 'Usuario o contraseña incorrectos'
        END;
        SET @usuario_id = 0;
        SET @rol = NULL;
        SET @nombre_completo = NULL;
    END
END
GO

CREATE PROCEDURE sp_CambiarPassword
    @usuario_id INT,
    @password_actual VARCHAR(255),
    @password_nueva VARCHAR(255),
    @resultado VARCHAR(100) OUTPUT
AS
BEGIN
    DECLARE @password_hash VARBINARY(256);
    
    SELECT @password_hash = password_hash 
    FROM Usuarios 
    WHERE id_usuario = @usuario_id;
    
    IF @password_hash = HASHBYTES('SHA2_256', @password_actual)
    BEGIN
        UPDATE Usuarios 
        SET password_hash = HASHBYTES('SHA2_256', @password_nueva),
            ultimo_cambio_password = GETDATE()
        WHERE id_usuario = @usuario_id;
        
        SET @resultado = 'Contraseña cambiada exitosamente';
    END
    ELSE
    BEGIN
        SET @resultado = 'Contraseña actual incorrecta';
    END
END
GO

CREATE PROCEDURE sp_SolicitarRecuperacionPassword
    @email VARCHAR(100),
    @token VARCHAR(255),
    @expiracion_minutos INT = 60,
    @resultado VARCHAR(100) OUTPUT
AS
BEGIN
    IF EXISTS(SELECT 1 FROM Usuarios WHERE email = @email AND estado = 'Activo')
    BEGIN
        UPDATE Usuarios 
        SET token_recuperacion = @token,
            token_expiracion = DATEADD(MINUTE, @expiracion_minutos, GETDATE())
        WHERE email = @email;
        
        SET @resultado = 'Token de recuperación generado';
    END
    ELSE
    BEGIN
        SET @resultado = 'Email no encontrado o usuario inactivo';
    END
END
GO

CREATE PROCEDURE sp_RestablecerPassword
    @token VARCHAR(255),
    @password_nueva VARCHAR(255),
    @resultado VARCHAR(100) OUTPUT
AS
BEGIN
    DECLARE @usuario_id INT;
    DECLARE @expiracion DATETIME;
    
    SELECT @usuario_id = id_usuario, @expiracion = token_expiracion
    FROM Usuarios 
    WHERE token_recuperacion = @token AND estado = 'Activo';
    
    IF @usuario_id IS NOT NULL AND @expiracion > GETDATE()
    BEGIN
        UPDATE Usuarios 
        SET password_hash = HASHBYTES('SHA2_256', @password_nueva),
            token_recuperacion = NULL,
            token_expiracion = NULL,
            ultimo_cambio_password = GETDATE()
        WHERE id_usuario = @usuario_id;
        
        SET @resultado = 'Contraseña restablecida exitosamente';
    END
    ELSE IF @usuario_id IS NOT NULL AND @expiracion <= GETDATE()
    BEGIN
        SET @resultado = 'El token ha expirado';
    END
    ELSE
    BEGIN
        SET @resultado = 'Token inválido';
    END
END
GO

-- =========================================================
-- PROCEDIMIENTOS CRUD EXISTENTES (MANTENIDOS)
-- =========================================================

CREATE PROCEDURE sp_CreateProducto
    @codigo VARCHAR(50), @nombre VARCHAR(100), @id_cat INT, 
    @iva DECIMAL(5,2), @stock_ini INT, @costo DECIMAL(10,2), @p_venta DECIMAL(10,2)
AS
BEGIN
    BEGIN TRANSACTION
    INSERT INTO Productos (codigo_barras, nombre, id_categoria, iva_porcentaje, stock_actual)
    VALUES (@codigo, @nombre, @id_cat, @iva, @stock_ini);
    
    DECLARE @nuevo_id INT = SCOPE_IDENTITY();
    
    INSERT INTO Precios_Promociones (id_producto, costo_compra, precio_final)
    VALUES (@nuevo_id, @costo, @p_venta);
    COMMIT TRANSACTION
END;
GO

CREATE PROCEDURE sp_ReadProducto
    @filtro VARCHAR(100)
AS
BEGIN
    SELECT P.codigo_barras, P.nombre, P.stock_actual, PRE.precio_final, PRE.es_promocional
    FROM Productos P
    INNER JOIN Precios_Promociones PRE ON P.id_producto = PRE.id_producto
    WHERE P.codigo_barras = @filtro OR P.nombre LIKE '%' + @filtro + '%';
END;
GO

CREATE PROCEDURE sp_UpdateStock
    @codigo VARCHAR(50), @cantidad INT
AS
BEGIN
    UPDATE Productos SET stock_actual = stock_actual + @cantidad 
    WHERE codigo_barras = @codigo;
END;
GO

CREATE PROCEDURE sp_DeleteProducto
    @id_prod INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Factura_Detalle WHERE id_producto = @id_prod)
    BEGIN
        DELETE FROM Precios_Promociones WHERE id_producto = @id_prod;
        DELETE FROM Productos WHERE id_producto = @id_prod;
    END
    ELSE PRINT 'No se puede eliminar: El producto tiene historial de ventas.';
END;
GO

CREATE PROCEDURE sp_CreateCliente
    @identificacion VARCHAR(20), @nombre VARCHAR(100), @tel VARCHAR(15)
AS
BEGIN
    INSERT INTO Clientes (identificacion, nombre, telefono)
    VALUES (@identificacion, @nombre, @tel);
END;
GO

CREATE PROCEDURE sp_UpdateCliente
    @id_cliente INT, @nombre VARCHAR(100), @tel VARCHAR(15)
AS
BEGIN
    UPDATE Clientes SET nombre = @nombre, telefono = @tel WHERE id_cliente = @id_cliente;
END;
GO

CREATE PROCEDURE sp_GestionarPromocion
    @id_prod INT, @precio_promo DECIMAL(10,2), @es_promo BIT, @fin_promo DATE
AS
BEGIN
    UPDATE Precios_Promociones
    SET precio_final = @precio_promo, 
        es_promocional = @es_promo, 
        fecha_fin_promo = @fin_promo
    WHERE id_producto = @id_prod;
END;
GO

CREATE PROCEDURE sp_BuscarProductoVenta
    @TerminoBusqueda VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.id_producto,
        P.codigo_barras AS [Código],
        P.nombre AS [Producto],
        C.nombre_categoria AS [Categoría],
        P.stock_actual AS [Existencia],
        PRE.precio_final AS [Precio Público],
        CASE 
            WHEN PRE.es_promocional = 1 AND (PRE.fecha_fin_promo IS NULL OR PRE.fecha_fin_promo >= CAST(GETDATE() AS DATE))
            THEN 'SÍ (OFERTA)' 
            ELSE 'NO' 
        END AS [Promoción Activa],
        P.iva_porcentaje AS [% IVA]
    FROM Productos P
    INNER JOIN Categorias C ON P.id_categoria = C.id_categoria
    INNER JOIN Precios_Promociones PRE ON P.id_producto = PRE.id_producto
    WHERE 
        P.codigo_barras = @TerminoBusqueda
        OR P.nombre LIKE '%' + @TerminoBusqueda + '%'
        OR C.nombre_categoria LIKE '%' + @TerminoBusqueda + '%';
END;
GO

-- =========================================================
-- NUEVOS PROCEDIMIENTOS PARA PROVEEDORES Y EMPRESA
-- =========================================================

CREATE PROCEDURE sp_AsignarProveedorPreferente
    @codigo_producto VARCHAR(50),
    @id_proveedor INT,
    @resultado VARCHAR(100) OUTPUT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Productos WHERE codigo_barras = @codigo_producto)
    BEGIN
        UPDATE Productos 
        SET id_proveedor_preferente = @id_proveedor
        WHERE codigo_barras = @codigo_producto;
        
        SET @resultado = 'Proveedor asignado correctamente';
    END
    ELSE
    BEGIN
        SET @resultado = 'Producto no encontrado';
    END
END
GO

CREATE PROCEDURE sp_ProductosPorProveedor
    @id_proveedor INT
AS
BEGIN
    SELECT 
        P.id_producto,
        P.codigo_barras,
        P.nombre,
        P.stock_actual,
        P.stock_minimo,
        PR.precio_final,
        CASE 
            WHEN PR.es_promocional = 1 AND (PR.fecha_fin_promo IS NULL OR PR.fecha_fin_promo >= CAST(GETDATE() AS DATE))
            THEN 'Sí'
            ELSE 'No'
        END AS EnPromocion
    FROM Productos P
    INNER JOIN Precios_Promociones PR ON P.id_producto = PR.id_producto
    WHERE P.id_proveedor_preferente = @id_proveedor AND P.activo = 1
END
GO

CREATE PROCEDURE sp_ActualizarEmpresa
    @nombre_comercial VARCHAR(100),
    @nombre_fiscal VARCHAR(100),
    @nit VARCHAR(50),
    @direccion VARCHAR(MAX),
    @telefono VARCHAR(8),
    @email VARCHAR(100),
    @mensaje_pie_factura VARCHAR(255) = NULL,
    @resultado VARCHAR(100) OUTPUT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Empresa WHERE id_empresa = 1)
    BEGIN
        UPDATE Empresa 
        SET nombre_comercial = @nombre_comercial,
            nombre_fiscal = @nombre_fiscal,
            nit_cedula_juridica = @nit,
            direccion = @direccion,
            telefono = @telefono,
            email = @email,
            mensaje_pie_factura = ISNULL(@mensaje_pie_factura, mensaje_pie_factura),
            fecha_registro = GETDATE()
        WHERE id_empresa = 1;
        
        SET @resultado = 'Empresa actualizada correctamente';
    END
    ELSE
    BEGIN
        INSERT INTO Empresa (id_empresa, nombre_comercial, nombre_fiscal, nit_cedula_juridica, direccion, telefono, email, mensaje_pie_factura)
        VALUES (1, @nombre_comercial, @nombre_fiscal, @nit, @direccion, @telefono, @email, @mensaje_pie_factura);
        
        SET @resultado = 'Empresa creada correctamente';
    END
END
GO

CREATE PROCEDURE sp_CreateProveedor
    @nombre_empresa VARCHAR(100),
    @contacto_nombre VARCHAR(100) = NULL,
    @telefono VARCHAR(8),
    @direccion VARCHAR(MAX) = NULL,
    @nit VARCHAR(50) = NULL,
    @contacto_email VARCHAR(100) = NULL,
    @condiciones_pago VARCHAR(50) = 'Contado',
    @credito_maximo DECIMAL(10,2) = 0,
    @id_usuario_registro INT,
    @resultado VARCHAR(100) OUTPUT,
    @id_proveedor INT OUTPUT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Proveedores WHERE nombre_empresa = @nombre_empresa)
    BEGIN
        SET @resultado = 'El proveedor ya existe';
        SET @id_proveedor = 0;
        RETURN;
    END
    
    INSERT INTO Proveedores (
        nombre_empresa, 
        contacto_nombre, 
        telefono, 
        direccion, 
        nit, 
        contacto_email,
        condiciones_pago,
        credito_maximo,
        id_usuario_registro,
        fecha_registro,
        activo
    )
    VALUES (
        @nombre_empresa, 
        @contacto_nombre, 
        @telefono, 
        @direccion, 
        @nit, 
        @contacto_email,
        @condiciones_pago,
        @credito_maximo,
        @id_usuario_registro,
        GETDATE(),
        1
    );
    
    SET @id_proveedor = SCOPE_IDENTITY();
    SET @resultado = 'Proveedor registrado correctamente';
END
GO

CREATE PROCEDURE sp_RegistrarCompra
    @folio VARCHAR(20),
    @id_proveedor INT,
    @id_usuario INT,
    @detalle_compras NVARCHAR(MAX),
    @factura_proveedor VARCHAR(50) = NULL,
    @observaciones VARCHAR(255) = NULL,
    @resultado VARCHAR(100) OUTPUT,
    @id_compra INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @subtotal DECIMAL(10,2) = 0;
        DECLARE @iva_total DECIMAL(10,2) = 0;
        DECLARE @total DECIMAL(10,2) = 0;
        
        INSERT INTO Compras (folio, id_proveedor, id_usuario, subtotal, iva_total, total_compra, factura_proveedor, observaciones, estado)
        VALUES (@folio, @id_proveedor, @id_usuario, 0, 0, 0, @factura_proveedor, @observaciones, 'Completada');
        
        SET @id_compra = SCOPE_IDENTITY();
        
        SET @resultado = 'Compra registrada exitosamente';
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @resultado = ERROR_MESSAGE();
        SET @id_compra = 0;
    END CATCH
END
GO

-- =========================================================
-- DATOS DE PRUEBA
-- =========================================================

INSERT INTO Empresa (id_empresa, nombre_comercial, nombre_fiscal, nit_cedula_juridica, direccion, telefono, email, mensaje_pie_factura)
VALUES (1, 'Miscelánea Master', 'Miscelánea Master S.A.', '123456789', 'Calle Principal #123', '12345678', 'info@miscelaneamaster.com', 'Gracias por su compra');

INSERT INTO Metodos_Pago (nombre_metodo) VALUES ('Efectivo'), ('Tarjeta Crédito'), ('Tarjeta Débito'), ('Transferencia');

INSERT INTO Categorias (nombre_categoria) VALUES ('Electrónicos'), ('Alimentos'), ('Limpieza'), ('Papelería');

INSERT INTO Usuarios (nombre_completo, nombre_usuario, email, password_hash, rol, registrado_por, id_empresa)
VALUES ('Administrador', 'admin', 'admin@miscelaneamaster.com', HASHBYTES('SHA2_256', 'admin123'), 'Administrador', NULL, 1);

INSERT INTO Usuarios (nombre_completo, nombre_usuario, email, password_hash, rol, registrado_por, id_empresa)
VALUES ('Cajero General', 'cajero', 'cajero@miscelaneamaster.com', HASHBYTES('SHA2_256', 'cajero123'), 'Cajero', 1, 1);

PRINT '==================================================';
PRINT 'BASE DE DATOS COMPLETA CREADA EXITOSAMENTE';
PRINT '==================================================';
PRINT 'CREDENCIALES DE ACCESO:';
PRINT 'Administrador: usuario = admin | password = admin123';
PRINT 'Cajero: usuario = cajero | password = cajero123';
PRINT '==================================================';

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

-- Insertar SOLO las categorías que NO existen
INSERT INTO Categorias (nombre_categoria, descripcion, activo)
SELECT * FROM (VALUES
    ('Bebidas', 'Gaseosas, jugos, aguas, bebidas energéticas', 1),
    ('Ferretería', 'Herramientas, clavos, pegamentos', 1),
    ('Cuidado Personal', 'Shampoo, jabones, cremas, maquillaje', 1),
    ('Lácteos', 'Leche, yogures, quesos, mantequilla', 1),
    ('Panadería', 'Pan, galletas, pastelillos', 1),
    ('Carnes Frías', 'Jamón, salchichas, mortadela', 1),
    ('Abarrotes', 'Arroz, frijoles, azúcar, sal, aceite', 1),
    ('Mascotas', 'Alimento para perros, gatos, accesorios', 1),
    ('Juguetería', 'Juguetes para niños', 1),
    ('Ropa', 'Prendas de vestir', 1),
    ('Calzado', 'Zapatos, sandalias, zapatillas', 1)
) AS nuevas(nombre_categoria, descripcion, activo)
WHERE NOT EXISTS (
    SELECT 1 FROM Categorias C WHERE C.nombre_categoria = nuevas.nombre_categoria
);
GO

-- Verificar todas las categorías ahora
SELECT id_categoria, nombre_categoria FROM Categorias ORDER BY nombre_categoria;
GO

-- Insertar productos con stock bajo
INSERT INTO Productos (codigo_barras, nombre, descripcion, id_categoria, iva_porcentaje, stock_actual, stock_minimo, activo)
VALUES 
('STOCK001', 'Coca Cola 600ml', 'Gaseosa cola', 1, 15.00, 2, 10, 1),
('STOCK002', 'Pepsi 600ml', 'Gaseosa cola', 1, 15.00, 3, 10, 1),
('STOCK003', 'Doritos Nacho', 'Tostadas de maíz', 2, 15.00, 1, 8, 1);
GO

-- Verificar
SELECT nombre, stock_actual, stock_minimo FROM Productos WHERE stock_actual <= stock_minimo;
GO

-- Actualizar contraseña del admin (ejecutar en SQL Server)
UPDATE Usuarios 
SET password_hash = HASHBYTES('SHA2_256', 'admin123')
WHERE nombre_usuario = 'admin';

UPDATE Usuarios 
SET password_hash = HASHBYTES('SHA2_256', 'cajero123')
WHERE nombre_usuario = 'cajero';

-- Agregar columna activo si no existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Productos') AND name = 'activo')
BEGIN
    ALTER TABLE Productos ADD activo BIT NOT NULL DEFAULT 1;
    PRINT '✅ Columna activo agregada a Productos';
END
GO

-- Verificar
SELECT id_producto, nombre, activo FROM Productos;
GO

SELECT COLUMN_NAME, DATA_TYPE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Precios_Promociones';

SELECT COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Facturas'
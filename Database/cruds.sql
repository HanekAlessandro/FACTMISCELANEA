USE MiscelaneaMaster;
GO

-- =============================================
-- CRUD: MÓDULO DE PRODUCTOS E INVENTARIO
-- =============================================

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

-- =============================================
-- CRUD: MÓDULO DE CLIENTES
-- =============================================

-- CREATE
CREATE PROCEDURE sp_CreateCliente
    @identificacion VARCHAR(20), @nombre VARCHAR(100), @tel VARCHAR(15)
AS
BEGIN
    INSERT INTO Clientes (identificacion, nombre, telefono)
    VALUES (@identificacion, @nombre, @tel);
END;
GO

-- UPDATE
CREATE PROCEDURE sp_UpdateCliente
    @id_cliente INT, @nombre VARCHAR(100), @tel VARCHAR(15)
AS
BEGIN
    UPDATE Clientes SET nombre = @nombre, telefono = @tel WHERE id_cliente = @id_cliente;
END;
GO

-- =============================================
-- CRUD: MÓDULO DE PROMOCIONES
-- =============================================

-- UPDATE: Activar o cambiar precio promocional
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

-- BÚSQUEDA AVANZADA PARA PUNTO DE VENTA (READ)
-- =============================================

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
        -- Lógica de precio: Si hay promo vigente, se marca
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
        P.codigo_barras = @TerminoBusqueda -- Búsqueda exacta por escáner
        OR P.nombre LIKE '%' + @TerminoBusqueda + '%' -- Búsqueda parcial por nombre
        OR C.nombre_categoria LIKE '%' + @TerminoBusqueda + '%'; -- Búsqueda por categoría
END;
GO
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Categorias] (
    [id_categoria] int NOT NULL IDENTITY,
    [nombre_categoria] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([id_categoria])
);
GO

CREATE TABLE [Clientes] (
    [id_cliente] int NOT NULL IDENTITY,
    [identificacion] nvarchar(20) NULL,
    [nombre] nvarchar(100) NOT NULL DEFAULT N'Público General',
    [direccion] nvarchar(255) NULL,
    [telefono] nvarchar(8) NULL,
    [puntos_lealtad] int NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([id_cliente])
);
GO

CREATE TABLE [Empresa] (
    [id_empresa] int NOT NULL IDENTITY,
    [nombre_comercial] nvarchar(100) NOT NULL,
    [nit_cedula_juridica] nvarchar(50) NOT NULL,
    [direccion] nvarchar(max) NULL,
    [telefono] nvarchar(8) NULL,
    [mensaje_pie_factura] nvarchar(255) NULL,
    CONSTRAINT [PK_Empresa] PRIMARY KEY ([id_empresa])
);
GO

CREATE TABLE [Metodos_Pago] (
    [id_metodo] int NOT NULL IDENTITY,
    [nombre_metodo] nvarchar(30) NOT NULL,
    CONSTRAINT [PK_Metodos_Pago] PRIMARY KEY ([id_metodo])
);
GO

CREATE TABLE [Proveedores] (
    [id_proveedor] int NOT NULL IDENTITY,
    [nombre_empresa] nvarchar(100) NOT NULL,
    [contacto_nombre] nvarchar(100) NULL,
    [telefono] nvarchar(8) NULL,
    [direccion] nvarchar(max) NULL,
    CONSTRAINT [PK_Proveedores] PRIMARY KEY ([id_proveedor])
);
GO

CREATE TABLE [Usuarios] (
    [id_usuario] int NOT NULL IDENTITY,
    [nombre_completo] nvarchar(100) NOT NULL,
    [nombre_usuario] nvarchar(50) NOT NULL,
    [password_hash] nvarchar(255) NOT NULL,
    [rol] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([id_usuario])
);
GO

CREATE TABLE [Productos] (
    [id_producto] int NOT NULL IDENTITY,
    [codigo_barras] nvarchar(50) NOT NULL,
    [nombre] nvarchar(100) NOT NULL,
    [descripcion] nvarchar(255) NULL,
    [id_categoria] int NULL,
    [iva_porcentaje] decimal(5,2) NOT NULL DEFAULT 15.0,
    [stock_actual] int NOT NULL DEFAULT 0,
    [stock_minimo] int NOT NULL DEFAULT 5,
    CONSTRAINT [PK_Productos] PRIMARY KEY ([id_producto]),
    CONSTRAINT [FK_Productos_Categorias_id_categoria] FOREIGN KEY ([id_categoria]) REFERENCES [Categorias] ([id_categoria]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Facturas] (
    [id_factura] int NOT NULL IDENTITY,
    [folio] nvarchar(20) NOT NULL,
    [fecha_hora] datetime2 NOT NULL DEFAULT (GETDATE()),
    [id_cliente] int NULL,
    [id_metodo_pago] int NULL,
    [id_usuario] int NULL,
    [subtotal_sin_iva] decimal(18,2) NULL,
    [total_iva] decimal(18,2) NULL,
    [total_final] decimal(18,2) NULL,
    CONSTRAINT [PK_Facturas] PRIMARY KEY ([id_factura]),
    CONSTRAINT [FK_Facturas_Clientes_id_cliente] FOREIGN KEY ([id_cliente]) REFERENCES [Clientes] ([id_cliente]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Facturas_Metodos_Pago_id_metodo_pago] FOREIGN KEY ([id_metodo_pago]) REFERENCES [Metodos_Pago] ([id_metodo]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Facturas_Usuarios_id_usuario] FOREIGN KEY ([id_usuario]) REFERENCES [Usuarios] ([id_usuario]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Kardex] (
    [id_movimiento] int NOT NULL IDENTITY,
    [id_producto] int NOT NULL,
    [tipo_movimiento] nvarchar(20) NULL,
    [cantidad] int NOT NULL,
    [fecha] datetime2 NOT NULL DEFAULT (GETDATE()),
    [referencia_id] int NULL,
    [observaciones] nvarchar(255) NULL,
    CONSTRAINT [PK_Kardex] PRIMARY KEY ([id_movimiento]),
    CONSTRAINT [FK_Kardex_Productos_id_producto] FOREIGN KEY ([id_producto]) REFERENCES [Productos] ([id_producto]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Precios_Promociones] (
    [id_precio] int NOT NULL IDENTITY,
    [id_producto] int NOT NULL,
    [costo_compra] decimal(18,2) NOT NULL,
    [precio_sugerido] decimal(18,2) NULL,
    [precio_final] decimal(18,2) NOT NULL,
    [es_promocional] bit NOT NULL DEFAULT CAST(0 AS bit),
    [fecha_inicio_promo] datetime2 NULL,
    [fecha_fin_promo] datetime2 NULL,
    CONSTRAINT [PK_Precios_Promociones] PRIMARY KEY ([id_precio]),
    CONSTRAINT [FK_Precios_Promociones_Productos_id_producto] FOREIGN KEY ([id_producto]) REFERENCES [Productos] ([id_producto]) ON DELETE CASCADE
);
GO

CREATE TABLE [Devoluciones] (
    [id_devolucion] int NOT NULL IDENTITY,
    [id_factura] int NULL,
    [id_producto] int NULL,
    [cantidad] int NOT NULL,
    [motivo] nvarchar(255) NULL,
    [fecha_devolucion] datetime2 NOT NULL DEFAULT (GETDATE()),
    [id_usuario_autoriza] int NULL,
    CONSTRAINT [PK_Devoluciones] PRIMARY KEY ([id_devolucion]),
    CONSTRAINT [FK_Devoluciones_Facturas_id_factura] FOREIGN KEY ([id_factura]) REFERENCES [Facturas] ([id_factura]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Devoluciones_Productos_id_producto] FOREIGN KEY ([id_producto]) REFERENCES [Productos] ([id_producto]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Devoluciones_Usuarios_id_usuario_autoriza] FOREIGN KEY ([id_usuario_autoriza]) REFERENCES [Usuarios] ([id_usuario]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Factura_Detalle] (
    [id_detalle] int NOT NULL IDENTITY,
    [id_factura] int NOT NULL,
    [id_producto] int NOT NULL,
    [cantidad] int NOT NULL,
    [precio_unitario_aplicado] decimal(18,2) NULL,
    [iva_aplicado] decimal(18,2) NULL,
    [subtotal_linea] decimal(18,2) NULL,
    CONSTRAINT [PK_Factura_Detalle] PRIMARY KEY ([id_detalle]),
    CONSTRAINT [FK_Factura_Detalle_Facturas_id_factura] FOREIGN KEY ([id_factura]) REFERENCES [Facturas] ([id_factura]) ON DELETE CASCADE,
    CONSTRAINT [FK_Factura_Detalle_Productos_id_producto] FOREIGN KEY ([id_producto]) REFERENCES [Productos] ([id_producto]) ON DELETE NO ACTION
);
GO

CREATE UNIQUE INDEX [IX_Categorias_nombre_categoria] ON [Categorias] ([nombre_categoria]);
GO

CREATE UNIQUE INDEX [IX_Clientes_identificacion] ON [Clientes] ([identificacion]) WHERE [identificacion] IS NOT NULL;
GO

CREATE INDEX [IX_Devoluciones_id_factura] ON [Devoluciones] ([id_factura]);
GO

CREATE INDEX [IX_Devoluciones_id_producto] ON [Devoluciones] ([id_producto]);
GO

CREATE INDEX [IX_Devoluciones_id_usuario_autoriza] ON [Devoluciones] ([id_usuario_autoriza]);
GO

CREATE INDEX [IX_Factura_Detalle_id_factura] ON [Factura_Detalle] ([id_factura]);
GO

CREATE INDEX [IX_Factura_Detalle_id_producto] ON [Factura_Detalle] ([id_producto]);
GO

CREATE UNIQUE INDEX [IX_Facturas_folio] ON [Facturas] ([folio]);
GO

CREATE INDEX [IX_Facturas_id_cliente] ON [Facturas] ([id_cliente]);
GO

CREATE INDEX [IX_Facturas_id_metodo_pago] ON [Facturas] ([id_metodo_pago]);
GO

CREATE INDEX [IX_Facturas_id_usuario] ON [Facturas] ([id_usuario]);
GO

CREATE INDEX [IX_Kardex_id_producto] ON [Kardex] ([id_producto]);
GO

CREATE UNIQUE INDEX [IX_Metodos_Pago_nombre_metodo] ON [Metodos_Pago] ([nombre_metodo]);
GO

CREATE UNIQUE INDEX [IX_Precios_Promociones_id_producto] ON [Precios_Promociones] ([id_producto]);
GO

CREATE UNIQUE INDEX [IX_Productos_codigo_barras] ON [Productos] ([codigo_barras]);
GO

CREATE INDEX [IX_Productos_id_categoria] ON [Productos] ([id_categoria]);
GO

CREATE UNIQUE INDEX [IX_Usuarios_nombre_usuario] ON [Usuarios] ([nombre_usuario]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260510043346_InitialCreate', N'8.0.0');
GO

COMMIT;
GO


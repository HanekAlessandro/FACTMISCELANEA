use MiscelaneaMaster
go

/* Módulo productos */

create procedure sp_CreateProducto
    @codigo varchar(50),
    @nombre varchar(100),
    @id_cat int,
    @iva decimal(5,2),
    @stock_ini int,
    @costo decimal(10,2),
    @p_venta decimal(10,2)
as
begin

    insert into Productos (
        codigo_barras,
        nombre,
        id_categoria,
        iva_porcentaje,
        stock_actual
    )
    values (
        @codigo,
        @nombre,
        @id_cat,
        @iva,
        @stock_ini
    )

    declare @nuevo_id int

    set @nuevo_id = scope_identity()

    insert into Precios_Promociones (
        id_producto,
        costo_compra,
        precio_final
    )
    values (
        @nuevo_id,
        @costo,
        @p_venta
    )

end
go


create procedure sp_ReadProducto
    @filtro varchar(100)
as
begin

    select
        P.codigo_barras,
        P.nombre,
        P.stock_actual,
        PRE.precio_final,
        PRE.es_promocional
    from Productos P
    inner join Precios_Promociones PRE
        on P.id_producto = PRE.id_producto
    where
        P.codigo_barras = @filtro
        or P.nombre like '%' + @filtro + '%'

end
go


create procedure sp_UpdateStock
    @codigo varchar(50),
    @cantidad int
as
begin

    update Productos
    set stock_actual = stock_actual + @cantidad
    where codigo_barras = @codigo

end
go


create procedure sp_DeleteProducto
    @id_prod int
as
begin

    if not exists (
        select 1
        from Factura_Detalle
        where id_producto = @id_prod
    )
    begin

        delete from Precios_Promociones
        where id_producto = @id_prod

        delete from Productos
        where id_producto = @id_prod

    end

    else
    begin

        print 'El producto tiene historial de ventas'

    end

end
go


/* Módulo clientes */

create procedure sp_CreateCliente
    @identificacion varchar(20),
    @nombre varchar(100),
    @tel varchar(15)
as
begin

    insert into Clientes (
        identificacion,
        nombre,
        telefono
    )
    values (
        @identificacion,
        @nombre,
        @tel
    )

end
go


create procedure sp_UpdateCliente
    @id_cliente int,
    @nombre varchar(100),
    @tel varchar(15)
as
begin

    update Clientes
    set
        nombre = @nombre,
        telefono = @tel
    where id_cliente = @id_cliente

end
go


/* Módulo promociones */

create procedure sp_GestionarPromocion
    @id_prod int,
    @precio_promo decimal(10,2),
    @es_promo bit,
    @fin_promo date
as
begin

    update Precios_Promociones
    set
        precio_final = @precio_promo,
        es_promocional = @es_promo,
        fecha_fin_promo = @fin_promo
    where id_producto = @id_prod

end
go


/* Buscar producto venta */

create procedure sp_BuscarProductoVenta
    @TerminoBusqueda varchar(100)
as
begin

    select
        P.id_producto,
        P.codigo_barras,
        P.nombre,
        C.nombre_categoria,
        P.stock_actual,
        PRE.precio_final,
        PRE.es_promocional,
        P.iva_porcentaje
    from Productos P
    inner join Categorias C
        on P.id_categoria = C.id_categoria
    inner join Precios_Promociones PRE
        on P.id_producto = PRE.id_producto
    where
        P.codigo_barras = @TerminoBusqueda
        or P.nombre like '%' + @TerminoBusqueda + '%'
        or C.nombre_categoria like '%' + @TerminoBusqueda + '%'
end
go
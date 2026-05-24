// productosData está definida en el archivo .cshtml como variable global

console.log("ProductosDataJson:", productosData);

let productos = [];
let precioProductoActual = 0;
let stockProductoActual = 0;

$("#productoSelect").change(function() {
    var productoId = parseInt($(this).val());
    var producto = productosData.find(p => p.id == productoId);
    
    if (producto && producto.id) {
        precioProductoActual = typeof producto.precio === 'number' ? producto.precio : parseFloat(producto.precio) / 100;
        stockProductoActual = producto.stock;
        $("#precioInput").val("₡" + precioProductoActual.toFixed(2));
        $("#stockInfo").html("Stock disponible: " + stockProductoActual + " unidades");
        $("#cantidadInput").attr("max", stockProductoActual);
        $("#stockError").hide();
    } else {
        $("#precioInput").val("");
        $("#stockInfo").html("");
        precioProductoActual = 0;
        stockProductoActual = 0;
    }
});

$("#cantidadInput").on("input", function() {
    var cantidad = parseInt($(this).val());
    if (cantidad > stockProductoActual && stockProductoActual > 0) {
        $("#stockError").text("Máximo disponible: " + stockProductoActual + " unidades").show();
    } else {
        $("#stockError").hide();
    }
});

function agregarProducto() {
    var productoId = $("#productoSelect").val();
    var productoNombre = $("#productoSelect option:selected").text();
    var cantidad = parseInt($("#cantidadInput").val()) || 0;
    var precioUnitario = precioProductoActual;

    if (!productoId) { mostrarError("Seleccione un producto"); return; }
    if (cantidad < 1) { mostrarError("Ingrese una cantidad válida"); return; }
    if (precioUnitario <= 0) { mostrarError("El producto no tiene precio configurado"); return; }
    if (cantidad > stockProductoActual) { mostrarError("Stock insuficiente. Disponible: " + stockProductoActual + " unidades"); return; }

    var existente = productos.find(p => p.id == productoId);
    if (existente) {
        var nuevaCantidad = existente.cantidad + cantidad;
        if (nuevaCantidad > stockProductoActual) {
            mostrarError("Stock insuficiente. Ya tiene " + existente.cantidad + ". Máximo: " + stockProductoActual);
            return;
        }
        existente.cantidad = nuevaCantidad;
        existente.subtotal = parseFloat((existente.cantidad * existente.precioUnitario).toFixed(2));
        existente.iva = parseFloat((existente.subtotal * 0.15).toFixed(2));
    } else {
        productos.push({
            id: productoId,
            nombre: productoNombre.split(' -')[0],
            cantidad: cantidad,
            precioUnitario: precioUnitario,
            subtotal: parseFloat((cantidad * precioUnitario).toFixed(2)),
            iva: parseFloat(((cantidad * precioUnitario) * 0.15).toFixed(2))
        });
    }
    actualizarTabla();
    calcularTotales();
    $("#productoSelect").val("");
    $("#precioInput").val("");
    $("#cantidadInput").val(1);
    $("#stockInfo").html("");
    precioProductoActual = 0;
    stockProductoActual = 0;
}

function eliminarProducto(index) {
    productos.splice(index, 1);
    actualizarTabla();
    calcularTotales();
}

function actualizarTabla() {
    var tbody = $("#productosBody");
    tbody.empty();
    if (productos.length === 0) {
        tbody.append('<tr><td colspan="6" class="text-center text-muted py-4">No hay productos agregados</td></tr>');
        return;
    }
    for (var i = 0; i < productos.length; i++) {
        var p = productos[i];
        tbody.append(`
            <tr>
                <td>${p.nombre}</td>
                <td class="text-center">${p.cantidad}</td>
                <td class="text-end">₡${p.precioUnitario.toFixed(2)}</td>
                <td class="text-end">₡${p.iva.toFixed(2)}</td>
                <td class="text-end">₡${p.subtotal.toFixed(2)}</td>
                <td class="text-center"><button type="button" class="btn-eliminar-item" onclick="eliminarProducto(${i})">Eliminar</button></td>
            </tr>
        `);
    }
}

function calcularTotales() {
    var subtotal = 0, iva = 0;
    for (var i = 0; i < productos.length; i++) {
        subtotal += productos[i].subtotal;
        iva += productos[i].iva;
    }
    $("#subtotalTotal").text(subtotal.toFixed(2));
    $("#ivaTotal").text(iva.toFixed(2));
    $("#totalFinal").text((subtotal + iva).toFixed(2));
    $("#subtotalSinIva").val(subtotal.toFixed(2));
    $("#totalIva").val(iva.toFixed(2));
    $("#totalFactura").val((subtotal + iva).toFixed(2));
}

function mostrarError(mensaje) {
    $("#errorMessage").text(mensaje);
    $("#errorModal").modal("show");
}

$(document).ready(function() {
    setTimeout(function() { 
        $(".alert").fadeOut("slow", function() { 
            $(this).remove(); 
        }); 
    }, 5000);
});
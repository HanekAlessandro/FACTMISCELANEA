// Búsqueda en tiempo real
$("#searchInput").on("keyup", function() {
    var value = $(this).val().toLowerCase();
    $("#clientesTable tbody tr").each(function() {
        var texto = $(this).text().toLowerCase();
        $(this).toggle(texto.indexOf(value) > -1);
    });
});

// Auto-cerrar alertas después de 5 segundos
$(document).ready(function() {
    setTimeout(function() {
        $(".alert").fadeOut("slow", function() {
            $(this).remove();
        });
    }, 5000);
});
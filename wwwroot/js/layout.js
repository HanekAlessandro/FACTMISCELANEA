// Auto-cerrar alertas después de 5 segundos
$(document).ready(function() {
    setTimeout(function() {
        $(".alert").fadeOut("slow", function() {
            $(this).remove();
        });
    }, 5000);
});
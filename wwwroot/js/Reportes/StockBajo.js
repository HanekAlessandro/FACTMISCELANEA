function exportToExcel() {
    var table = document.getElementById("tablaStock");
    if (!table) {
        alert("No hay datos para exportar");
        return;
    }
    var html = table.outerHTML;
    var blob = new Blob([html], { type: "application/vnd.ms-excel" });
    var link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    var fecha = new Date();
    var fechaStr = fecha.getFullYear() + 
                  ("0" + (fecha.getMonth() + 1)).slice(-2) + 
                  ("0" + fecha.getDate()).slice(-2) + "_" +
                  ("0" + fecha.getHours()).slice(-2) + 
                  ("0" + fecha.getMinutes()).slice(-2);
    link.download = "StockBajo_" + fechaStr + ".xls";
    link.click();
    URL.revokeObjectURL(link.href);
}
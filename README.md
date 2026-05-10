`README.md`

```markdown
# Sistema de Facturación - Miscelánea Master

Sistema de facturación moderno y eficiente desarrollado con **ASP.NET Core 8**, **Entity Framework Core** y **SQL Server**.  
Ideal para la gestión de ventas, inventario y clientes en negocios tipo miscelánea.

---

## 🚀 Tecnologías utilizadas

- ASP.NET Core 8
- Entity Framework Core 8
- SQL Server / SQL LocalDB
- Razor Pages
- Bootstrap 5
- jQuery
- Chart.js (para reportes gráficos)
- SweetAlert2 (para alertas modernas)
- X.PagedList (para paginación)
- Visual Studio Code / Visual Studio 2022

---

## 🗄️ Base de Datos

Nombre de la base de datos:

```sql
MiscelaneaMaster
```

### Estructura de la BD:

- **Empresa** - Datos de la empresa
- **Categorías** - Clasificación de productos
- **Metodos_Pago** - Formas de pago disponibles
- **Usuarios** - Administradores y cajeros
- **Proveedores** - Registro de proveedores
- **Clientes** - Clientes con sistema de puntos de lealtad
- **Productos** - Inventario con control de stock
- **Precios_Promociones** - Precios y ofertas especiales
- **Facturas** - Cabecera de facturas
- **Factura_Detalle** - Detalle de productos facturados
- **Devoluciones** - Registro de devoluciones
- **Kardex** - Movimientos de inventario

### Procedimientos Almacenados:

- `sp_CreateProducto` - Registrar nuevo producto
- `sp_ReadProducto` - Buscar productos
- `sp_UpdateStock` - Actualizar inventario
- `sp_DeleteProducto` - Eliminación segura de productos
- `sp_BuscarProductoVenta` - Búsqueda optimizada para punto de venta
- `sp_CreateCliente` - Registrar cliente
- `sp_GestionarPromocion` - Administrar ofertas

### Triggers:

- `trg_ProcesarVenta` - Actualiza stock automáticamente al facturar

### Vistas:

- `Vista_Reporte_Mensual` - Resumen de ventas por mes
- `Vista_Alerta_Stock` - Productos con inventario crítico

> La base de datos se genera mediante el script `Database/script.sql` o mediante migraciones de Entity Framework Core.

---

## ⚙️ Funcionalidades principales

### Módulo de Productos
- ✅ CRUD completo de productos
- ✅ Control de stock con alertas de mínimo
- ✅ Búsqueda por código, nombre o categoría
- ✅ Filtro por stock bajo
- ✅ Paginación de productos (10 por página)
- ✅ Códigos de barras con limpieza automática
- ✅ Confirmación de eliminación con diseño moderno

### Módulo de Reportes
- ✅ **Dashboard de Reportes** - Menú principal
- ✅ **Stock Bajo** - Productos que necesitan reabastecimiento
  - Exportación a Excel
  - Impresión directa
  - Identificación de urgencia (🔥 URGENTE / ⚠️ Moderada)
- 🚧 **Ventas Mensuales** - Próximamente (gráficos interactivos)
- 🚧 **Top 10 Productos** - Próximamente
- 🚧 **Reporte Financiero** - Próximamente
- ✅ **Resumen General** - Estadísticas del negocio

### Próximas funcionalidades
- 📊 Gráficos interactivos con Chart.js
- 📈 Reportes financieros avanzados
- 🏆 Ranking de productos más vendidos

---

## 🎨 Características del Diseño

- **Diseño moderno** con gradientes y sombras
- **Animaciones suaves** (fadeIn, slideIn)
- **Responsive** - Funciona en dispositivos móviles
- **Alertas automáticas** con cierre después de 5 segundos
- **Búsqueda en tiempo real** sin recargar página
- **Filtros visuales** con botones activos
- **Modales de confirmación** elegantes

---

## 📁 Estructura del Proyecto

```
FactMiscelanea/
├── Data/
│   └── ApplicationDbContext.cs
├── Database/
│   ├── script.sql           # Script completo de BD
│   ├── cruds.sql            # Procedimientos almacenados
│   └── busqueda.sql         # Búsqueda avanzada
├── Models/
│   ├── Producto.cs
│   ├── Categoria.cs
│   ├── Cliente.cs
│   ├── Factura.cs
│   └── ... (otros modelos)
├── Pages/
│   ├── Productos/
│   │   ├── Index.cshtml     # Lista con paginación
│   │   ├── Create.cshtml    # Crear producto
│   │   ├── Edit.cshtml      # Editar producto
│   │   └── Delete.cshtml    # Confirmar eliminación
│   └── Reportes/
│       ├── Index.cshtml     # Dashboard de reportes
│       ├── StockBajo.cshtml # Reporte de stock bajo
│       ├── VentasMensuales.cshtml
│       ├── TopProductos.cshtml
│       ├── ResumenGeneral.cshtml
│       └── Financiero.cshtml
├── wwwroot/
│   └── css/
│       └── site.css
└── Program.cs
```

---

## ▶️ Cómo ejecutar el proyecto

### Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) o SQL LocalDB
- [Visual Studio Code](https://code.visualstudio.com/) o Visual Studio 2022

### Pasos de instalación

```bash
# 1. Clonar el repositorio
git clone https://github.com/HanekAlessandro/FACTMISCELANEA.git

# 2. Entrar al directorio
cd FACTMISCELANEA

# 3. Restaurar dependencias
dotnet restore

# 4. Configurar la base de datos (opción 1 - script SQL)
# Ejecutar Database/script.sql en SQL Server Management Studio

# 4. Configurar la base de datos (opción 2 - migraciones)
dotnet ef database update

# 5. Ejecutar la aplicación
dotnet run
```

### Configuración de conexión

Editar `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MiscelaneaMaster;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

---

## 📸 Capturas de pantalla

### Gestión de Productos
- Listado con paginación y búsqueda en tiempo real
- Indicadores visuales de stock bajo
- Botones de acción con diseño uniforme

### Reportes
- Dashboard con tarjetas de acceso rápido
- Exportación a Excel en reporte de stock bajo
- Estadísticas generales del negocio

---

## 🔧 Solución de problemas comunes

| Problema | Solución |
|----------|----------|
| Error de conexión a BD | Verificar cadena en `appsettings.json` |
| Código de barras con `\n` | El modelo lo limpia automáticamente |
| Página 404 | Asegurar que la ruta use `/Productos` o `/Reportes` |
| Doble confirmación al eliminar | Ya se corrigió (solo la página Delete) |
| Reportes vacíos | En desarrollo - muestra mensaje "Próximamente" |

---

## 👥 Autores

- **Hanek Alessandro Pineda Moreno**
- **Bismarck Humberto Penado Santamaría**
- **Franco Estrada**

---

## 📌 Nota

Asegúrate de tener configurada la cadena de conexión en `appsettings.json` apuntando a tu instancia de SQL Server.

Para ver los reportes, navega a: `http://localhost:5267/Reportes`

---

## 📄 Licencia

Este proyecto es de uso académico y educativo.

---

## 🚧 Estado del Proyecto

- ✅ Módulo de Productos - COMPLETO
- ✅ Paginación - COMPLETA
- ✅ Reporte de Stock Bajo - COMPLETO
- 🚧 Reportes avanzados - EN DESARROLLO (35%)
- 🚧 Módulo de Facturación - PRÓXIMAMENTE
- 🚧 Módulo de Clientes - PRÓXIMAMENTE

---

*Última actualización: Mayo 2026*
```
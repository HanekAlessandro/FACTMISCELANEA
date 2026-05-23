# Sistema de Facturación - Miscelánea Master

Sistema de facturación desarrollado con ASP.NET Core 8, Entity Framework Core y SQL Server para la gestión de ventas, inventario y clientes.

---

## Tecnologías utilizadas

- ASP.NET Core 8
- Entity Framework Core 8
- SQL Server
- Razor Pages
- Bootstrap 5
- jQuery
- Chart.js
- SweetAlert2
- X.PagedList
- Visual Studio Code
- Visual Studio 2022

---

## Base de datos

Nombre de la base de datos:

```sql
MiscelaneaMaster
```

### Tablas principales

- Empresa
- Categorias
- Metodos_Pago
- Usuarios
- Proveedores
- Clientes
- Productos
- Precios_Promociones
- Facturas
- Factura_Detalle
- Devoluciones
- Kardex
- Compras
- Detalle_Compras
- Logs_Login

### Procedimientos almacenados

- sp_CreateProducto
- sp_ReadProducto
- sp_UpdateStock
- sp_DeleteProducto
- sp_BuscarProductoVenta
- sp_CreateCliente
- sp_UpdateCliente
- sp_GestionarPromocion
- sp_RegistrarUsuario
- sp_LoginUsuario
- sp_CambiarPassword
- sp_CreateProveedor
- sp_RegistrarCompra

### Triggers

- trg_ProcesarVenta
- trg_ProcesarCompra
- trg_ActualizarSaldoProveedor

### Vistas

- Vista_Reporte_Mensual
- Vista_Alerta_Stock
- Vista_Proveedores_Productos
- Vista_Logs_Acceso

---

## Funcionalidades

### Productos

- CRUD completo
- Control de stock
- Alertas de stock mínimo
- Búsqueda por nombre o código
- Promociones
- Paginación

### Facturación

- Registro de facturas
- Detalle de productos vendidos
- Actualización automática de inventario
- Control de IVA

### Clientes

- Registro de clientes
- Sistema de puntos

### Usuarios

- Login seguro
- Roles administrador y cajero
- Bloqueo por intentos fallidos
- Recuperación de contraseña

### Proveedores

- Registro de proveedores
- Compras
- Control de saldo pendiente

### Reportes

- Reporte mensual
- Reporte de stock bajo
- Resumen general

---

## Diseño

- Responsive
- Diseño moderno
- Alertas dinámicas
- Dashboard administrativo
- Tablas con filtros
- Animaciones suaves

---

## Estructura del proyecto

```txt
FactMiscelanea/
├── Data/
├── Database/
├── Models/
├── Pages/
├── Controllers/
├── Filters/
├── wwwroot/
├── appsettings.json
└── Program.cs
```

---

## Cómo ejecutar el proyecto

### Requisitos

- .NET 8 SDK
- SQL Server
- Visual Studio Code o Visual Studio 2022

### Instalación

```bash
git clone https://github.com/HanekAlessandro/FACTMISCELANEA.git

cd FACTMISCELANEA

dotnet restore

dotnet ef database update

dotnet run
```

---

## Configuración de conexión

Editar `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MiscelaneaMaster;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

---

## Solución de problemas

| Problema | Solución |
|---|---|
| Error de conexión | Revisar appsettings.json |
| Página no encontrada | Revisar rutas |
| Stock incorrecto | Verificar triggers |
| Error EF Core | Ejecutar migraciones |

---

## Autores

- Hanek Alessandro Pineda Moreno
- Bismarck Humberto Penado Santamaría
- Franco Estrada

---

## Estado del proyecto

- Productos - Completo
- Paginación - Completo
- Reportes básicos - Completo
- Facturación - En desarrollo
- Clientes - En desarrollo

---

## Licencia

Proyecto académico y educativo.

---

Última actualización: Mayo 2026
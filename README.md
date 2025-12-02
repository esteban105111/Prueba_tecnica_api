# Prueba_tecnica_api

Tienda.Api

API REST para la gestión de productos y pedidos en una tienda.

---

Tecnologías

- .NET 8 / ASP.NET Core
- Entity Framework Core
- SQL Server
- C#



2. Configurar la cadena de conexión en appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TiendaDb;Trusted_Connection=True;"
  }
}

3. Ejecutar migraciones y actualizar la base de datos:

dotnet ef migrations add InitialCreate
dotnet ef database update

4. Ejecutar la API:

La API estará disponible en:

- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

---

Endpoints

Productos

Método | Endpoint | Descripción | JSON de ejemplo
-------|---------|------------|----------------
GET    | /api/productos | Obtener todos los productos | N/A
GET    | /api/productos/{id} | Obtener producto por ID | N/A
POST   | /api/productos | Crear un producto | { "nombre": "Mouse", "precio": 50.5, "stock": 100 }
PUT    | /api/productos/{id} | Actualizar un producto | { "nombre": "Mouse Gamer", "precio": 55.5, "stock": 90 }
DELETE | /api/productos/{id} | Eliminar un producto | N/A

Pedidos

Método | Endpoint | Descripción | JSON de ejemplo
-------|---------|------------|----------------
GET    | /api/pedidos | Obtener todos los pedidos con paginación y filtros opcionales | ?pagina=1&tamPagina=10&estado=Pendiente&cliente=Juan
GET    | /api/pedidos/{id} | Obtener un pedido por ID | N/A
POST   | /api/pedidos | Crear un pedido | { "nombreCliente": "Juan", "items": [ { "productoId": 1, "cantidad": 2, "precioUnitario": 50.5 } ] }
PUT    | /api/pedidos/{id}/status?estado=Pagado | Cambiar estado del pedido | N/A
DELETE | /api/pedidos/{id} | Eliminar un pedido | N/A

Notas:

- La creación de pedidos valida el stock disponible.
- Los estados posibles son: Pendiente = 0, Pagado = 1, Cancelado = 2.
- Al eliminar un pedido que tiene items relacionados, la API devuelve un error 409 Conflict.

Mensajes de respuesta comunes

- 200 OK: Solicitud exitosa.
- 201 Created: Recurso creado correctamente.
- 204 No Content: Eliminación o actualización exitosa.
- 400 Bad Request: Error de validación de datos.
- 404 Not Found: Recurso no encontrado.
- 409 Conflict: Conflicto de integridad referencial.
- 500 Internal Server Error: Error inesperado en el servidor.

Ejemplo de POST Pedido completo

{
  "nombreCliente": "Juan Montaño",
  "creadoEn": "2025-12-02T00:51:34.341Z",
  "estado": 0,
  "items": [
    {
      "productoId": 1,
      "cantidad": 2,
      "precioUnitario": 50.5
    },
    {
      "productoId": 2,
      "cantidad": 1,
      "precioUnitario": 120.0
    }
  ]
}

Swagger / Documentación interactiva

https://localhost:5001/swagger





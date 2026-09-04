# Products API

API REST para gestión de catálogo de productos y stock, construida con .NET 10 y PostgreSQL.

## Decisiones técnicas

- **Arquitectura en capas**: Domain (reglas de negocio) → Application (casos de uso) → Infrastructure (persistencia) → Controllers (HTTP).
- **Base de datos**: PostgreSQL ejecutado en contenedor Docker, utilizando Entity Framework Core con el proveedor `Npgsql`.
- **Regla de negocio clave**: el stock nunca puede quedar negativo; esta validación vive en la entidad `Product.AdjustStock()`, no en el controller.
- **Manejo de errores**: middleware centralizado (`ExceptionHandlingMiddleware`) que traduce excepciones a respuestas JSON estándar con formato `{ "error": "[Tipo]", "message": "[Detalle]" }` y códigos HTTP apropiados (400/404/500).

## Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Cómo correr localmente

### 1. Iniciar la base de datos PostgreSQL con Docker

```bash
docker run --name products-postgres \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=productsdb \
  -p 5432:5432 \
  -d postgres:18
```

### 2. Clonar y ejecutar la API

```bash
git clone https://github.com/ctoloza499/ProductsApi.git
cd ProductsApi
dotnet restore
dotnet ef database update
dotnet run
```

*Nota: La API también aplica automáticamente las migraciones pendientes al iniciar (`db.Database.Migrate()` en `Program.cs`).*

### 3. Documentación interactiva

Una vez iniciada la aplicación, accede a:
- **Swagger UI**: `http://localhost:5270/swagger` (la ruta raíz `http://localhost:5270/` redirige automáticamente a Swagger).

## Endpoints

| Método | Ruta                             | Descripción        |
| ------ | -------------------------------- | ------------------ |
| POST   | `/api/products`                  | Crea un producto   |
| GET    | `/api/products/{id}`             | Consulta por ID    |
| GET    | `/api/products?page=1&pageSize=10` | Lista paginada   |
| PATCH  | `/api/products/{id}/stock`       | Ajusta stock (+/-) |

## Estructura de errores

Todas las respuestas de error y validaciones devuelven un formato uniforme:

```json
{
  "error": "NotFound",
  "message": "Producto con id 99 no encontrado."
}
```

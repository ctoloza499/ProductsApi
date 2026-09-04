# Products API

API REST para gestión de catálogo de productos y stock, construida con .NET 10.

## Decisiones técnicas

- **Arquitectura en capas**: Domain (reglas de negocio) → Application (casos de uso) → Infrastructure (persistencia) → Controllers (HTTP).
- **Base de datos**: SQLite, por simplicidad de despliegue sin infraestructura externa.
- **Regla de negocio clave**: el stock nunca puede quedar negativo; esta validación vive en la entidad `Product.AdjustStock()`, no en el controller.
- **Manejo de errores**: middleware centralizado que traduce excepciones de dominio a códigos HTTP (400/404).

## Cómo correr localmente

```bash
git clone https://github.com/ctoloza499/ProductsApi.git
cd ProductsApi
dotnet restore
dotnet ef database update
dotnet run
```

Luego abre `http://localhost:5270/swagger`

## Endpoints

| Método | Ruta                             | Descripción        |
| ------ | -------------------------------- | ------------------ |
| POST   | /api/products                    | Crea un producto   |
| GET    | /api/products/{id}               | Consulta por ID    |
| GET    | /api/products?page=1&pageSize=10 | Lista paginada     |
| PATCH  | /api/products/{id}/stock         | Ajusta stock (+/-) |

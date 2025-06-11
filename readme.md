
# Catálogo de Productos – Prueba Técnica Full-Stack

_Solución basada en **.NET 9 Web API** (backend) y **Quasar 2 / Vue 3** (frontend)._
Permite gestionar un catálogo de productos con operaciones CRUD completas.

## Requisitos Previos
- .NET SDK 9.x (Preview 5 o superior)
- Node.js 18 LTS / 20 LTS
- Quasar CLI 2.5+
- Git 2.x

## Instalación

### 1. BackEnd

    cd BackEnd/ProductCatalog.Api
    dotnet run
    
    https://localhost:5080


### 2. FrontEnd

    cd FrontEnd/product-catalog-fullstack
    npm install
    quasar dev                 

	http://localhost:9000


## Endpoints principales

- GET    /api/products           → listar productos
- GET    /api/products/{id}      → producto por ID
- POST   /api/products           → crear producto
- PUT    /api/products/{id}      → actualizar producto
- DELETE /api/products/{id}      → eliminar producto


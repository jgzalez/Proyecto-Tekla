# Catálogo de Productos – Prueba Técnica Full-Stack

_Solución basada en **.NET 9 Web API** (backend) y **Quasar 2 / Vue 3** (frontend)._
Permite gestionar un catálogo de productos con operaciones CRUD completas.


• .NET SDK 9.x (Preview 5 o superior)
• Node.js 18 LTS / 20 LTS
• Quasar CLI 2.5+
• Git 2.x

───────────────────────────────────────────────────────────────────────────────
Puesta en marcha rápida (desarrollo)
───────────────────────────────────────────────────────────────────────────────
1. Backend
   cd BackEnd/ProductCatalog.Api
   dotnet run                 # https://localhost:5080

2. Frontend
   cd FrontEnd/product-catalog-fullstack
   npm install
   quasar dev                 # http://localhost:9000

───────────────────────────────────────────────────────────────────────────────
Endpoints principales
───────────────────────────────────────────────────────────────────────────────
GET    /api/products           → listar productos
GET    /api/products/{id}      → producto por ID
POST   /api/products           → crear producto
PUT    /api/products/{id}      → actualizar producto
DELETE /api/products/{id}      → eliminar producto

───────────────────────────────────────────────────────────────────────────────
Script útiles (frontend)
───────────────────────────────────────────────────────────────────────────────
npm run dev         # quasar dev
npm run build       # quasar build
npm run lint        # ESLint

───────────────────────────────────────────────────────────────────────────────
Despliegue API (self-contained)
───────────────────────────────────────────────────────────────────────────────
dotnet publish -c Release -r linux-x64 ^
  -p:PublishSingleFile=true -p:SelfContained=true ^
  -p:IncludeNativeLibrariesForSelfExtract=true ^
  -o ./publish
scp ./publish/ProductCatalog.Api  user@server:/opt/product-api/
chmod +x /opt/product-api/ProductCatalog.Api

systemd (/etc/systemd/system/product-api.service)
[Unit]
Description=Product Catalog API
After=network.target
[Service]
WorkingDirectory=/opt/product-api
ExecStart=/opt/product-api/ProductCatalog.Api
Environment=ASPNETCORE_URLS=http://127.0.0.1:5000
Restart=always
[Install]
WantedBy=multi-user.target

───────────────────────────────────────────────────────────────────────────────
Despliegue frontend
───────────────────────────────────────────────────────────────────────────────
quasar build
rsync -av dist/spa/  user@server:/var/www/my_webapp/www/

───────────────────────────────────────────────────────────────────────────────
CORS producción (Program.cs)
───────────────────────────────────────────────────────────────────────────────
policy.WithOrigins("https://code.jgonzalezfals.dev")
      .AllowAnyHeader()
      .AllowAnyMethod();

───────────────────────────────────────────────────────────────────────────────
Autor
───────────────────────────────────────────────────────────────────────────────
José González · 2025 · Licencia MIT

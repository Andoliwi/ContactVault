# ContactVault - Vault Electrónico de Contactos

Aplicación web sencilla de agenda de contactos construida con **ASP.NET Core Web API (C#)**, **Entity Framework Core** y **SQLite**. Contiene la aplicación en un contenedor **Docker** y expone una API REST con operaciones CRUD.

## Funcionalidades

- **Registrar contactos** (`POST /api/contacts`)
- **Listar contactos** (`GET /api/contacts`)
- **Consultar detalle** de un contacto (`GET /api/contacts/{id}`)
- **Editar contactos** (`PUT /api/contacts/{id}`)
- **Eliminar contactos** (`DELETE /api/contacts/{id}`)
- **Buscar contactos por nombre o apellido** (`GET /api/contacts?search={texto}` o `GET /api/contacts/search?term={texto}`)

Cada contacto contiene: **Nombre**, **Apellido**, **Teléfono**, **Correo electrónico** y **Empresa**.

Además incluye una página de inicio (`/`) con accesos directos a la API.

## Tecnologías

- ASP.NET Core 9 (C#)
- Entity Framework Core 9
- SQLite
- Docker / docker-compose
- Git y GitHub

## Estructura del proyecto

```
ContactVault/
├── ContactVault.sln               # Solución de Visual Studio 2022
├── Controllers/
│   └── ContactsController.cs       # API CRUD + búsqueda
├── Data/
│   └── ContactDbContext.cs         # Contexto de Entity Framework Core
├── Models/
│   └── Contact.cs                  # Entidad Contacto
├── appsettings.json                # Cadena de conexión SQLite
├── Program.cs                      # Servicios, pipeline y página de inicio
├── Dockerfile                      # Contenerización multi-etapa
├── docker-compose.yml              # Orquestación y volumen de datos
└── ContactVault.Api.csproj
```

## Requisitos previos

- [Visual Studio 2022](https://visualstudio.microsoft.com/) con la carga de trabajo **ASP.NET y desarrollo web** (o [.NET SDK 9](https://dotnet.microsoft.com/download/dotnet) para usar la CLI)
- [Docker](https://www.docker.com/products/docker-desktop/) (para ejecutar en contenedor)
- [Git](https://git-scm.com/)

## Instalación y ejecución

### 1. Ejecución con Visual Studio 2022

1. Abre `ContactVault.sln` en Visual Studio 2022.
2. Selecciona el perfil **http**.
3. Pulsa **F5** (o el botón de Iniciar).

La página de inicio quedará disponible en `http://localhost:5126` (o el puerto indicado en `Properties/launchSettings.json`).

La base de datos SQLite (`contactvault.db`) se crea automáticamente la primera vez que se ejecuta la aplicación.

### 2. Ejecución local con la CLI de .NET

```bash
# Restaurar dependencias y ejecutar
dotnet run
```

### 3. Ejecución con Docker

#### Opción A: con Dockerfile

```bash
# Construir la imagen
docker build -t contactvault-api .

# Ejecutar el contenedor (la app queda en http://localhost:8080)
docker run -d --name contactvault-api -p 8080:8080 contactvault-api
```

#### Opción B: con docker-compose (recomendado)

```bash
# Construye la imagen y levanta el contenedor con un volumen para persistir datos
docker compose up -d --build
```

Detener y eliminar el contenedor:

```bash
docker compose down
```

Los datos de la base de datos se conservan en el volumen `contactvault-data` incluso al eliminar el contenedor.

> **Nota:** La API expone el puerto **8080** dentro del contenedor. En entorno de desarrollo la documentación de OpenAPI está disponible en `/openapi/v1.json`.

## Uso de la API

Endpoints (base por defecto `http://localhost:8080`):

| Método | Endpoint                       | Descripción                                  |
|--------|--------------------------------|----------------------------------------------|
| GET    | `/`                            | Página de inicio                             |
| GET    | `/api/contacts`                | Lista todos los contactos                    |
| GET    | `/api/contacts?search={texto}` | Busca contactos por nombre o apellido        |
| GET    | `/api/contacts/{id}`           | Obtiene el detalle de un contacto            |
| POST   | `/api/contacts`                | Registra un nuevo contacto                   |
| PUT    | `/api/contacts/{id}`           | Actualiza un contacto existente              |
| DELETE | `/api/contacts/{id}`           | Elimina un contacto                          |

### Ejemplo de cuerpo JSON (POST/PUT)

```json
{
  "firstName": "Juan",
  "lastName": "Pérez",
  "phone": "+34 600 111 222",
  "email": "juan.perez@correo.com",
  "company": "Acme"
}
```

### Ejemplo con curl

```bash
# Crear un contacto
curl -X POST http://localhost:8080/api/contacts \
  -H "Content-Type: application/json" \
  -d '{"firstName":"Juan","lastName":"Pérez","phone":"+34 600 111 222","email":"juan.perez@correo.com","company":"Acme"}'

# Listar contactos
curl http://localhost:8080/api/contacts

# Buscar contactos por nombre o apellido
curl "http://localhost:8080/api/contacts?search=perez"

# Detalle de un contacto
curl http://localhost:8080/api/contacts/1

# Editar un contacto
curl -X PUT http://localhost:8080/api/contacts/1 \
  -H "Content-Type: application/json" \
  -d '{"id":1,"firstName":"Juan","lastName":"Pérez","phone":"+34 600 999 888","email":"juan.perez@correo.com","company":"Acme S.L."}'

# Eliminar un contacto
curl -X DELETE http://localhost:8080/api/contacts/1
```

## Persistencia de datos

La aplicación usa SQLite (archivo `contactvault.db`). La base de datos se crea automáticamente al iniciar por primera vez (`EnsureCreated`).

- **En local:** el archivo se crea en la carpeta del proyecto.
- **Con Docker:** se recomienda usar `docker-compose`, que monta el volumen `contactvault-data` en `/app` para conservar los datos entre reinicios.
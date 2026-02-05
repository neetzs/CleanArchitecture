# Clean Architecture (.NET 10)

> Una plantilla robusta, modular y escalable implementando Clean Architecture, CQRS y seguridad moderna en .NET.

Este repositorio sirve como una implementación de referencia para construir APIs RESTful empresariales utilizando las últimas tecnologías del ecosistema .NET. Está diseñado siguiendo estrictamente la **Regla de Dependencia**, asegurando que la lógica de negocio permanezca desacoplada de frameworks externos y bases de datos.

---

## Stack Tecnológico

- **Core:** .NET 10 (LTS) / C# 14
- **API:** Minimal APIs (con `Endpoint Groups`)
- **Datos:** Entity Framework Core 10 + SQL Server 2025
- **Arquitectura:** Clean Architecture + CQRS
- **Mediación:** MediatR
- **Seguridad:** ASP.NET Core Identity + JWT
- **Documentación:** Swagger / OpenAPI
- **Entorno:** Docker Support

---

## Arquitectura y Diseño

El proyecto está dividido en 4 capas concéntricas siguiendo la **Regla de Dependencia**: nada en los círculos internos puede saber nada de los círculos externos.

![Clean Architecture Diagram](https://blog.cleancoder.com/uncle-bob/images/2012-08-13-the-clean-architecture/CleanArchitecture.jpg)

### 1. Domain (Núcleo)

    - Contiene las **Entidades** (ej: `Product`) y la lógica de negocio pura.
    - No tiene dependencias de ningún otro proyecto ni librería de terceros.
    - Es el corazón del sistema.

### 2. Application (Casos de Uso)

    - Orquestador de la lógica. Define **QUÉ** puede hacer el sistema.
    - Implementa **CQRS**:
    - **Commands:** Modifican estado (ej: `CreateProductCommand`).
    - **Queries:** Leen estado.
    - Define interfaces (ej: `IApplicationDbContext`) que la infraestructura debe cumplir.
    - Usa **MediatR** para desacoplar la API de la lógica.

### 3. Infrastructure (Implementación)

    - Define **CÓMO** funciona el sistema externamente.
    - Implementa las interfaces de _Application_.
    - Contiene:
    - `ApplicationDbContext` (Entity Framework).
    - Identity (Configuración de Usuarios y Roles).
    - Migraciones de Base de Datos.
    - Servicios externos (Email, Archivos, etc.).

### 4. API (Presentación)

    - El punto de entrada (REST API).
    - Usa **Minimal APIs** organizadas por `Endpoints` en lugar de controladores clásicos.
    - Se encarga de la Autenticación y Autorización.

---

## Guía de Instalación (Linux/Windows/Mac)

### Prerrequisitos

1.  **.NET SDK:** Versión 10 (o 9.0 como mínimo).
2.  **Docker:** Para ejecutar la base de datos SQL Server.

### Paso 1: Clonar el repositorio

```bash
git clone [https://github.com/TU_USUARIO/CleanArchitecture.git](https://github.com/TU_USUARIO/CleanArchitecture.git)
cd CleanArchitecture
```

### Paso 2: Levantar Base de Datos (Docker)

Si no tienes SQL Server local, levanta un contenedor rápido con este comando (usando la imagen de SQL Server 2025):

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=TuPasswordFuerte123!" -p 1433:1433 --name sql_server -d [mcr.microsoft.com/mssql/server:2025-latest](https://mcr.microsoft.com/mssql/server:2025-latest)
```

Nota: Asegúrate de que la ConnectionString en CleanArchitecture.API/appsettings.json coincida con la contraseña que acabas de configurar.

### Paso 3: Aplicar Migraciones

Esto creará las tablas de negocio (Productos) y las de seguridad (Identity Users/Roles) en tu base de datos.

```bash
dotnet ef database update --project CleanArchitecture.Infrastructure --startup-project CleanArchitecture.API
```

### Paso 4: Ejecutar la API

```bash
dotnet run --project CleanArchitecture.API
```

La API estará disponible en: http://localhost:5XXX/swagger

## Guía de Seguridad y Autenticación

El sistema utiliza **JWT (JSON Web Tokens)**. Los endpoints sensibles (como crear productos) están protegidos y requieren un token válido.

### Cómo probar la seguridad paso a paso en Swagger:

1.  **Registrar un usuario:**
    - Ve al endpoint `POST /api/auth/register`.
    - Envía un JSON con un `email` válido y una `password` fuerte (debe tener mayúsculas, minúsculas y símbolos).
    - _Ejemplo:_ `Admin123!`

2.  **Iniciar Sesión:**
    - Ve al endpoint `POST /api/auth/login`.
    - Al ejecutarlo con tus credenciales, recibirás una respuesta con un `accessToken`. **Copia este token completo**.

3.  **Autorizar (El Candado):**
    - Sube al botón **Authorize** (candado gris) en la parte superior derecha de Swagger.
    - En el campo de texto escribe la palabra `Bearer` seguido de un espacio y tu token.
    - _Formato:_ `Bearer eyJhbGciOiJIUzI1NiIs...`
    - Haz clic en "Authorize" y luego en "Close". El candado ahora estará cerrado 🔒.

4.  **Probar Endpoint Protegido:**
    - Ahora puedes usar `POST /api/products` exitosamente. El sistema sabe quién eres gracias al token.

---

## Resolución de Problemas (Troubleshooting)

### Error: "Degradación del paquete" o conflictos de versión en .NET 10

Debido a que .NET 10 es una versión muy reciente, existe un conflicto conocido entre `Microsoft.AspNetCore.OpenApi` (que exige versiones v2.0+) y `Swashbuckle` (que usa versiones v1.6).

**Solución aplicada en este repositorio:**
Para garantizar estabilidad, se eliminó la dependencia conflictiva `Microsoft.AspNetCore.OpenApi` y se forzaron las versiones estables en el archivo `.csproj` de la API:

    - `Microsoft.OpenApi` -> **1.6.14**
    - `Swashbuckle.AspNetCore` -> **6.5.0**

Si actualizas paquetes, asegúrate de mantener esta compatibilidad.

---

## Estructura de Carpetas

```text
📂 CleanArchitecture
├── 📂 CleanArchitecture.Domain             # Entidades (Core)
├── 📂 CleanArchitecture.Application        # CQRS, Interfaces, DTOs
│   ├── 📂 Common
│   └── 📂 Products
│       └── 📂 Commands
│           └── 📂 CreateProduct            # Vertical Slice (Command + Handler)
├── 📂 CleanArchitecture.Infrastructure     # EF Core, Identity, Migrations
├── 📂 CleanArchitecture.API                # Program.cs, Endpoints, Auth Config
└── 📄 CleanArchitecture.sln
```

---

## Bibliografía y Recursos de Estudio

Este proyecto es el resultado de la investigación y aplicación de conceptos de diversas fuentes de ingeniería de software. Agradecimientos especiales y créditos a los siguientes recursos:

### Teoría Fundamental

- **"Clean Architecture: A Craftsman's Guide to Software Structure and Design"** por _Robert C. Martin (Uncle Bob)_. La fuente original de los principios de diseño.
- **"Domain-Driven Design: Tackling Complexity in the Heart of Software"** por _Eric Evans_. Para entender la importancia de la capa de Dominio.

### Implementación en .NET

- **Clean Architecture with ASP.NET Core** por _Jason Taylor_. Su charla en GOTO 2019 y su plantilla son la referencia de facto para esta arquitectura en el ecosistema .NET.
- **Milan Jovanović & Amichai Mantinband**. Por sus excelentes divulgaciones sobre CQRS, MediatR y patrones de diseño modernos en .NET.
- **Documentación de Microsoft**: [Architecture of Containerized .NET Applications](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/).

### Conceptos Clave Aplicados

- **The Dependency Rule**: La regla de oro que mantiene el núcleo independiente.
- **CQRS (Command Query Responsibility Segregation)**: Separación de lecturas y escrituras para optimizar rendimiento y seguridad.
- **Vertical Slice Architecture**: Influencia en la organización de carpetas por _Features_ en lugar de capas técnicas estrictas.

---

## Autor

**Matias Leonel Ramirez**

- **LinkedIn:** [@MatiasLeonelRamirez](https://www.linkedin.com/in/matias-leonel-ramirez/)
- **GitHub:** [@neetzs](https://github.com/neetzs)

---

_Este proyecto está distribuido bajo la licencia **MIT**. Eres libre de usarlo, modificarlo y aprender de él._

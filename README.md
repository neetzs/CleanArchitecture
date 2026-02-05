# Clean Architecture Template (.NET 10 + CQRS)

Plantilla de inicio para proyectos escalables en .NET utilizando Clean Architecture, patrones CQRS con MediatR y Minimal APIs. Diseñado para ser agnóstico de plataforma (Linux/Windows/Mac).

## 🚀 Tecnologías

* **.NET 10** (Preview/Latest)
* **Clean Architecture** (Domain, Application, Infrastructure, API)
* **CQRS** (MediatR)
* **Entity Framework Core** (SQL Server)
* **Minimal APIs** con Endpoint Groups
* **Docker** Support

## 🏗 Estructura del Proyecto

El proyecto sigue estrictamente la Regla de Dependencia:

* **Domain:** Entidades y lógica empresarial pura. (Sin dependencias externas).
* **Application:** Casos de uso, Interfaces, DTOs, CQRS. (Depende de Domain).
* **Infrastructure:** Base de datos, Archivos, Servicios Externos. (Depende de Application).
* **API:** Punto de entrada. (Depende de Application e Infrastructure).

## 🛠 Configuración Local (Linux/Fedora)

### Prerrequisitos
* .NET SDK
* Docker (con un contenedor de SQL Server corriendo)

### 1. Clonar el repositorio
```bash
git clone [https://github.com/TU_USUARIO/CleanArchitecture-Starter.git](https://github.com/TU_USUARIO/CleanArchitecture-Starter.git)
cd CleanArchitecture-Starter

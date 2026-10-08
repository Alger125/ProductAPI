# ProductAPI – Rama feature/ef-core-sqlserver

Dockerización de SQL Server y persistencia con Entity Framework Core para ProductAPI (.NET 8, Clean Architecture).

## Tabla de contenidos
1. [Objetivo de la rama](#1-objetivo-de-la-rama)
2. [Arquitectura y estructura](#2-arquitectura-y-estructura)
3. [Requisitos previos](#3-requisitos-previos)
4. [Puesta en marcha paso a paso](#4-puesta-en-marcha-paso-a-paso)
5. [Preparación del entorno Windows (Docker + WSL 2)](#5-preparación-del-entorno-windows-docker--wsl-2)
6. [Troubleshooting: problemas encontrados y soluciones](#6-troubleshooting-problemas-encontrados-y-soluciones)
7. [Comandos de referencia](#7-comandos-de-referencia)

---

## 1. Objetivo de la rama
Reemplazar una instalación local de SQL Server por un contenedor Docker para que el entorno de desarrollo sea:
* **Reproducible:** cualquier desarrollador levanta la misma versión de SQL Server con un solo comando.
* **Aislado:** no se instala nada en el sistema operativo anfitrión.
* **Desechable:** la base de datos se puede destruir y recrear aplicando migraciones.

Esta rama incluye:
* `docker-compose.yml` con SQL Server 2022.
* Cadena de conexión en `appsettings.Development.json`.
* Migración inicial de EF Core (`InitialCreate`).
* Ajuste en `Program.cs` para que Swagger UI funcione con Swashbuckle 6.6.2.

---

## 2. Arquitectura y estructura
```text
ProductAPI/
├── docker-compose.yml                  # Contenedor de SQL Server
├── ProductAPI.Api/                     # Capa de presentación (startup project)
│   ├── Program.cs
│   └── appsettings.Development.json    # Connection string
├── ProductAPI.Application/             # Casos de uso / interfaces
├── ProductAPI.Domain/                  # Entidades
└── ProductAPI.Infrastructure/          # DbContext, migraciones (migrations project)
```

**Punto clave para EF Core:** en Clean Architecture el `DbContext` y las migraciones viven en `Infrastructure`, pero el host que lee la configuración (`appsettings`) es `Api`. Por eso cada comando `dotnet ef` necesita indicar ambos proyectos (ver paso 4.5).
**Tablas generadas:** `Products`, `Brands`, `Categories`, `Reviews`.

---

## 3. Requisitos previos

| Herramienta | Versión | Verificación |
| --- | --- | --- |
| **.NET SDK** | 8.0 | `dotnet --version` |
| **Docker Desktop** | Con motor WSL 2 | `docker --version` |
| **dotnet-ef (CLI)** | Compatible con EF Core 8 | `dotnet ef --version` |
| **Windows** | 10/11 con virtualización activa | Ver sección 5 |

---

## 4. Puesta en marcha paso a paso

### 4.1 Clonar y cambiar a la rama
```bash
git clone https://github.com/Alger125/ProductAPI.git
cd ProductAPI
git checkout feature/ef-core-sqlserver
```

### 4.2 Levantar SQL Server con Docker Compose
El archivo `docker-compose.yml` en la raíz descarga la imagen oficial `mcr.microsoft.com/mssql/server:2022-latest`.

**Nota:** la contraseña de `sa` debe cumplir la política de complejidad de SQL Server (mínimo 8 caracteres, con mayúsculas, minúsculas, números y símbolos). Si no la cumple, el contenedor se detiene al arrancar.

Iniciar en segundo plano y verificar:
```bash
docker-compose up -d
docker ps # el contenedor debe estar "Up"
docker logs productapi-sqlserver --tail 20 # buscar "SQL Server is now ready for client connections"
```

### 4.3 Configurar la cadena de conexión
En `ProductAPI.Api/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ProductAPI_DB;User Id=sa;Password=<TU_PASSWORD_SEGURA>;TrustServerCertificate=True;"
  }
}
```

### 4.4 Instalar la herramienta CLI de EF Core
Si `dotnet ef` no es reconocido:
```bash
dotnet tool install --global dotnet-ef
dotnet ef --version
```
Para actualizarla: `dotnet tool update --global dotnet-ef`.

### 4.5 Crear y aplicar migraciones
Ejecutar desde la raíz de la solución, indicando siempre ambos proyectos:
```bash
# Crear la migración
dotnet ef migrations add InitialCreate \
  --project ProductAPI.Infrastructure \
  --startup-project ProductAPI.Api

# Aplicarla al contenedor
dotnet ef database update \
  --project ProductAPI.Infrastructure \
  --startup-project ProductAPI.Api
```

### 4.6 Ejecutar la API
```bash
dotnet run --project ProductAPI.Api
```
Abrir `http://localhost:5213/swagger` (el puerto puede variar según `launchSettings.json`).

---

## 5. Preparación del entorno Windows (Docker + WSL 2)
Docker Desktop en Windows requiere los siguientes componentes del sistema operativo, además de la virtualización en BIOS.
1. Abrir "Activar o desactivar las características de Windows" (`optionalfeatures.exe`).
2. Marcar: **Plataforma de máquina virtual** (Virtual Machine Platform) y **Subsistema de Windows para Linux** (Windows Subsystem for Linux).
3. Reiniciar el equipo.
4. Iniciar Docker Desktop y confirmar que usa el motor WSL 2 (`Settings > General > Use the WSL 2 based engine`).

**Mover el disco virtual de Docker a otra unidad (ej. F:\)**
1. Abrir Docker Desktop.
2. Ir a `Settings > Resources > Advanced` (o Virtual Disk).
3. En `Disk image location` seleccionar la nueva ruta, por ejemplo `F:\DockerDesktopWSL`.
4. Pulsar `Apply & restart`. Docker migra los datos de forma nativa.
*(No usar symlinks (mklink) para mover el .vhdx).*

---

## 6. Troubleshooting: problemas encontrados y soluciones

### Problema 1: Virtualization support not detected
* **Causa raíz:** La virtualización estaba activa a nivel de hardware (AMD-V en BIOS), pero Windows no tenía habilitados los componentes de software que permiten crear máquinas virtuales (hipervisor y subsistema Linux).
* **Solución:** Habilitar Virtual Machine Platform y Windows Subsystem for Linux y reiniciar.

### Problema 2: Mover el disco de Docker a F:\ con symlinks (intento fallido)
* **Causa raíz:** El motor de virtualización de WSL 2 monta el `.vhdx` directamente y no resuelve enlaces simbólicos de NTFS.
* **Solución:** Eliminar el symlink, restaurar el archivo original y usar la opción oficial `Settings > Resources > Disk image location`.

### Problema 3: dotnet ef no es reconocido
* **Solución:** `dotnet tool install --global dotnet-ef` y reabrir la terminal.

### Problema 4: No project was found al crear migraciones
* **Causa raíz:** En Clean Architecture no hay un `.csproj` en la raíz, y el `DbContext` está en un proyecto distinto al ejecutable.
* **Solución:** Pasar `--project ProductAPI.Infrastructure --startup-project ProductAPI.Api` en cada comando.

### Problema 5: Swagger UI muestra "La definición proporcionada no especifica un campo de versión válido"
* **Causa raíz:** Swashbuckle.AspNetCore 6.6.2 genera el documento con `openapi: 3.0.4`, versión que el Swagger UI embebido no reconoce.
* **Solución:** Forzar la serialización como Swagger 2.0 en `Program.cs`:
```csharp
app.UseSwagger(options => { options.SerializeAsV2 = true; });
app.UseSwaggerUI();
```

---

## 7. Comandos de referencia

```bash
# Docker
docker-compose up -d            # Levantar SQL Server
docker-compose down             # Detener (conserva datos)
docker-compose down -v          # Detener y borrar volumen (borra datos)

# EF Core
dotnet ef migrations list --project ProductAPI.Infrastructure --startup-project ProductAPI.Api
dotnet ef migrations remove --project ProductAPI.Infrastructure --startup-project ProductAPI.Api
dotnet ef database drop --project ProductAPI.Infrastructure --startup-project ProductAPI.Api

# API
dotnet run --project ProductAPI.Api
```

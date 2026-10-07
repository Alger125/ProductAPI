# Product API

API robusta construida con **.NET 8**, **Clean Architecture** y el patrón **CQRS**.

---

## 📖 Evolución del Proyecto (Diario de Desarrollo)

### 🌿 Rama: eature/ef-core-sqlserver (EN PROGRESO)
**Objetivo:** Conectar nuestro dominio puro con una base de datos real (SQL Server) usando Entity Framework Core, respetando la Arquitectura Limpia.

**Pasos y Evolución:**
1. *(Próximamente)* Configuración de Docker para levantar SQL Server en local.
2. *(Próximamente)* Instalación de paquetes EF Core en la capa de Infraestructura.
3. *(Próximamente)* Creación del ApplicationDbContext (el puente entre C# y la base de datos).
4. *(Próximamente)* Generación y ejecución de la primera Migración.

---

### 🌿 Rama: eature/product-domain (COMPLETADA)
**Objetivo:** Crear el núcleo de la aplicación sin dependencias externas.
**Logros:**
- Se crearon las entidades base: Product, Category, Brand y Review.
- Se aplicó el concepto de **Modelo de Dominio Rico**: las propiedades usan private set para encapsulamiento.
- Se crearon métodos guardianes (ej. UpdatePrice, RemoveStock) que arrojan excepciones (ArgumentException, InvalidOperationException) si se violan las reglas de negocio.
- Se configuró **xUnit** y se escribieron pruebas unitarias para asegurar que las reglas del Product funcionan correctamente.

---

## 🚀 Progreso del PRD General
- [x] Estructura Base (Solución y capas).
- [x] Dominio y Reglas de Negocio.
- [ ] Bases de datos (EF Core + SQL Server).
- [ ] MediatR y CQRS.
- [ ] Endpoints de la API.

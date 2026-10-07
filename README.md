# ProductAPI – feature/ef-core-sqlserver

## 📌 ¿De qué trata este proyecto? (Explicación para no programadores)
Imagina que este proyecto es como construir **un restaurante muy organizado**:
- Tiene una **"Puerta Principal" (API)** donde los clientes hacen sus pedidos.
- Tiene **"Meseros y Gerentes" (Aplicación)** que toman el pedido, verifican que esté bien escrito y lo envían a la cocina.
- Tiene **"Las Reglas del Negocio" (Dominio)** (ej. "no puedes vender algo con precio negativo" o "no puedes vender si no hay stock").
- Tiene **"La Bodega y el Archivo" (Infraestructura)** donde se guarda toda la información (Base de datos) para que no se pierda.

Usamos una forma de organizar el código llamada **"Arquitectura Limpia"**, lo que significa que si mañana cambiamos la "Bodega" por una más moderna, las "Reglas del Negocio" no se enteran ni tienen que cambiar. ¡Todo sigue funcionando!

---

## 📌 Resumen de esta etapa
Actualmente estamos trabajando en **La Bodega** (persistencia de datos). Estamos conectando las reglas del negocio con el archivador (SQL Server) usando una herramienta que traduce nuestro código a lenguaje de base de datos (Entity Framework Core). También estamos preparando "cajas de envío" (Docker) para que el proyecto funcione igual en la computadora de cualquier persona.

## 🧭 Evolución del Proyecto (Diario de Desarrollo)
- **Posición**: Etapa 3.
- **Qué se heredó**: Las reglas del negocio ya construidas y probadas (de la etapa anterior `feature/product-domain`).
- **Qué se agregó**: Planificación para guardar información en base de datos, uso de cajas contenedoras (Docker) para asegurar el entorno, y preparación de herramientas para buscar datos rápido (LINQ).
- **Decisiones técnicas**: 
  - La configuración para guardar datos se hará *solo* en la carpeta de Infraestructura. Las Reglas del Negocio (Dominio) seguirán sin saber que existe una base de datos.
  - Usaremos **Docker**. En palabras sencillas: Docker crea una "computadora virtual portátil" que tiene instalado todo lo necesario (como la base de datos). Así evitamos el famoso "en mi máquina sí funciona".
  - Usaremos **LINQ**, que es una herramienta para buscar y filtrar información en la base de datos de forma muy rápida y segura.

## 🛠️ Tecnologías utilizadas (Diccionario simple)
| Tecnología | ¿Qué es técnicamente? | ¿Qué es en palabras sencillas? |
|---|---|---|
| **.NET / C#** | Framework base y Lenguaje | El idioma principal y las herramientas de construcción que usamos para programar el sistema. |
| **SQL Server** | Motor de base de datos | El archivador digital gigante donde se guardan los productos de forma permanente. |
| **EF Core** | ORM (Object-Relational Mapper) | Un "Traductor" automático. Convierte nuestro código en C# a instrucciones que el archivador (SQL Server) entiende. |
| **Docker** | Contenedorización | Una "Caja de envío" que empaqueta nuestra aplicación con todo lo que necesita para funcionar en cualquier computadora del mundo sin tener que instalar cosas extra. |
| **LINQ** | Lenguaje de consultas | Un buscador súper rápido. Como un filtro de Excel avanzado pero dentro del código. |

## 🚀 Instalación y ejecución
*(Nota: Pasos temporales, la base de datos y contenedores aún no se han generado)*
1. Clonar repositorio y cambiar a rama: `git checkout feature/ef-core-sqlserver`.
2. Levantar servicios en Docker (próximamente): `docker compose up -d`.
3. Restaurar solución: `dotnet restore`.

## 📁 Estructura del proyecto en esta etapa
```text
├── docker-compose.yml (Futuro manual para encender el proyecto completo con 1 clic)
├── ProductAPI.Infrastructure/ (La Bodega)
│   ├── (Futuro) ApplicationDbContext.cs (El mapa de dónde guardar cada cosa)
│   └── (Futuro) Migrations/ (El historial de cambios estructurales del archivador)
```

## ➡️ Siguiente etapa
Desarrollo de los comandos y consultas (CQRS) en la capa de Aplicación (Los Meseros).

---

## 🌐 Evolución Global del Proyecto
El proyecto comenzó con la estructura vacía (`main`). Luego creamos las reglas del negocio (`feature/product-domain`). Actualmente estamos conectando la base de datos (`feature/ef-core-sqlserver`).

| Etapa | Rama | Qué se logró | Link a la rama |
|---|---|---|---|
| 1 | main | Estructura base Clean Architecture. | [main](https://github.com/Alger125/ProductAPI/tree/main) |
| 2 | feature/product-domain | Entidades y reglas encapsuladas con pruebas. | [feature/product-domain](https://github.com/Alger125/ProductAPI/tree/feature/product-domain) |
| 3 | feature/ef-core-sqlserver | (En progreso) Conexión a Base de Datos y Docker. | [feature/ef-core-sqlserver](https://github.com/Alger125/ProductAPI/tree/feature/ef-core-sqlserver) |

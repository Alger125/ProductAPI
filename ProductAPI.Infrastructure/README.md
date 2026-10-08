# Entity Framework Core + SQL Server (La Bodega)

Rama: `feature/ef-core-sqlserver`

## 📌 ¿Para qué sirve esta capa? (Explicación para no programadores)
Imagina que esta capa de **Infraestructura** es el sótano donde están los **Archiveros** (Base de Datos), la **Planta de Luz** (Servicios Externos) y la sala de envíos. 

Es la única parte de nuestro código que tiene permiso para conectarse con el mundo exterior: bases de datos, enviar correos reales, conectarse al sistema de cobros, etc. Las otras capas del sistema no saben cómo funciona el archivero, solo le dicen a Infraestructura: *"Guarda esto, por favor"*.

---

## 🛠️ Herramientas que usaremos en esta etapa

### SQL Server (El Archivador Digital)
- **Qué es:** Es el motor de base de datos relacional.
- **En palabras sencillas:** Es un sistema gigante de tablas (como en Excel) donde la información se guarda permanentemente y no se borra cuando apagamos la computadora.

### Docker (La Caja Mágica)
- **Qué es:** Herramienta de contenedorización.
- **En palabras sencillas:** En lugar de hacer que cada programador instale SQL Server en su computadora (y sufra porque las configuraciones son distintas), metemos el archivador dentro de una "caja virtual" (Contenedor). Cualquier persona que descargue el proyecto, con un solo clic, tendrá exactamente el mismo archivador funcionando en segundos.

### Entity Framework Core (El Traductor)
- **Qué es:** Un ORM (Object-Relational Mapper).
- **En palabras sencillas:** Las bases de datos hablan un idioma (SQL) y nuestro código habla otro (C#). Entity Framework es un traductor en tiempo real. Nosotros escribimos código en C#, y él va y lo convierte a idioma de base de datos para buscar o guardar cosas en el archivador.

### ApplicationDbContext (El Mapa del Archivador)
- **Qué es:** La clase principal de EF Core.
- **En palabras sencillas:** Es un índice que le dice al Traductor qué cosas (Entidades: Productos, Categorías, Marcas) van en qué cajones (Tablas). 

### Migraciones (El Historial de Mudanzas)
- **Qué es:** El control de versiones de la base de datos.
- **En palabras sencillas:** Si mañana decidimos que los Productos deben tener una "Fecha de caducidad", el archivador tiene que agregar una nueva columna. Las migraciones son las instrucciones paso a paso de cómo hacer esos cambios en el archivador sin perder los datos que ya están guardados.

---

## 📝 Estado actual de esta etapa

**EN PROGRESO**
Actualmente estamos configurando todas estas herramientas. Todavía está pendiente:
- Crear la caja de Docker para el archivador.
- Configurar al Traductor (Entity Framework Core).
- Crear las tablas en el Archivador (Migraciones).

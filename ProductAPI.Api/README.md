# Capa de Presentación (ProductAPI.Api)

## 📌 ¿Para qué sirve esta capa? (Explicación para no programadores)
Esta es la **Puerta de Entrada** o la **Ventanilla de Atención** de nuestro sistema. Es la única parte de la aplicación que se expone al mundo exterior a través de Internet (usando HTTP).

Imagina un restaurante de comida rápida:
- El cliente no entra a la cocina (Dominio) ni busca en la bodega (Infraestructura).
- El cliente se acerca a una ventanilla y dice su orden.
- Esta capa es la cajera que atiende la ventanilla.

**Importante:** La cajera (API) no cocina ni toma decisiones, solo anota la orden y se la pasa a los Meseros (Capa de Aplicación) para que hagan el trabajo pesado.

## 🛡️ Responsabilidades y Conceptos Técnicos

### 1. Controladores (Controllers / Endpoints)
- **Qué son:** Las "ventanillas" específicas (ej. Ventanilla de Productos, Ventanilla de Clientes).
- **En palabras sencillas:** Reciben las peticiones de los usuarios (como un mensaje que dice "Quiero la lista de productos") y se las entregan a la capa de Aplicación. ¡Aquí no se escriben reglas de negocio!

### 2. Configuración Inicial (El Cableado)
- **Qué es:** El archivo `Program.cs`.
- **En palabras sencillas:** Es el lugar donde se enciende el sistema. Aquí es donde conectamos todas las piezas: le decimos a la ventanilla quiénes son los meseros (Aplicación) y dónde está la bodega (Infraestructura) para que todo funcione como un equipo.

### 3. Middlewares (Filtros Globales de Seguridad y Errores)
- **Qué son:** Código que intercepta las peticiones.
- **En palabras sencillas:** Imagina que en la puerta del restaurante hay un cadenero de seguridad. Antes de que llegues a la ventanilla, él revisa si tienes permiso de entrar (Validación de tokens de seguridad/Usuarios). Si ocurre un accidente en la cocina, este mismo cadenero se asegura de disculparse contigo formalmente en lugar de mostrarte el incendio (Manejo global de errores).

### 4. Contenedorización (Docker)
- **Qué es:** Alojar el `Dockerfile`.
- **En palabras sencillas:** Así como guardamos la Base de Datos en una caja de transporte (Contenedor), aquí también creamos una caja para nuestra Ventanilla. Esto nos permite subir fácilmente nuestro restaurante completo a servidores en la nube (como Amazon, Google o Microsoft) para que clientes reales puedan visitarlo.

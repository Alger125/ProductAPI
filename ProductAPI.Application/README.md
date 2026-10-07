# Capa de Aplicación (ProductAPI.Application)

## 📌 ¿Para qué sirve esta capa? (Explicación para no programadores)
Imagina que el Dominio son las **Reglas del Negocio** y la API es la **Ventanilla de Atención**. La capa de Aplicación son los **Gerentes y Meseros**.

Cuando un cliente llega a la ventanilla y pide "Crear un nuevo producto", el Mesero (Capa de Aplicación):
1. Toma la orden.
2. Revisa que el papel de la orden tenga sentido (Validación).
3. Va al almacén, ejecuta las reglas del negocio.
4. Devuelve el resultado a la ventanilla.

Esta capa coordina *el flujo* de las cosas, pero **no** dicta las reglas (eso lo hace el Dominio).

## 🛡️ Responsabilidades y Conceptos Técnicos
A continuación te explicamos las herramientas que usarán los "Meseros":

### 1. CQRS (Separar Lecturas de Escrituras)
- **Concepto técnico:** Command Query Responsibility Segregation.
- **En palabras sencillas:** Significa que separamos nuestro sistema en dos equipos. Un equipo solo se encarga de hacer preguntas (Queries: "dime qué productos hay") y el otro solo de hacer modificaciones (Commands: "crea un producto nuevo"). Esto hace que el sistema sea más rápido y ordenado. Usamos una librería llamada **MediatR** para repartir estos mensajes al mesero correcto.

### 2. LINQ (El Buscador Rápido)
- **Concepto técnico:** Language Integrated Query.
- **En palabras sencillas:** En el equipo de "Lecturas" (Queries), usamos LINQ para buscar exactamente la información que el cliente necesita en el archivador. Es como un filtro de Excel avanzado dentro del código que nos permite ordenar y buscar súper rápido sin sobrecargar el sistema.

### 3. DTOs (Formularios Simplificados)
- **Concepto técnico:** Data Transfer Objects.
- **En palabras sencillas:** Al cliente nunca le entregamos el documento original (la Entidad). Le entregamos una "fotocopia simplificada" con solo los datos que le interesan. Si un producto tiene 50 datos confidenciales, el DTO es un papelito que solo dice "Nombre y Precio".

### 4. Validación (Filtro Anti-Errores)
- **Concepto técnico:** Usaremos la librería **FluentValidation**.
- **En palabras sencillas:** Antes de siquiera intentar crear el producto, revisamos el formulario. Si el cliente olvidó poner el nombre del producto, el sistema detiene el proceso y le dice "Oye, te faltó llenar este campo". Ahorra tiempo y previene errores graves.

### 5. Interfaces de Servicios Externos
- **Concepto técnico:** Contratos de Inversión de Dependencias.
- **En palabras sencillas:** Aquí definimos qué necesita la aplicación. Por ejemplo, la aplicación dice: "Necesito un botón para Enviar Correos". No sabe *cómo* se envía el correo, solo sabe que debe haber un botón. La creación de ese botón real se hace en la capa de Infraestructura.

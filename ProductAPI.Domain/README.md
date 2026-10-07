# Product Domain (Las Reglas del Negocio)

Rama: `feature/product-domain`

## 📌 ¿Para qué sirve esta capa? (Explicación para no programadores)
Imagina que esta capa es **El Cerebro o el Manual de Reglas** de nuestra empresa. Aquí es donde definimos qué es un Producto, qué es una Marca, y las reglas inquebrantables de nuestro negocio.

Por ejemplo:
- "Un producto no puede tener precio menor a cero".
- "No puedes descontar inventario si no hay productos disponibles".

Lo más importante de esta capa es que **no sabe nada del mundo exterior**. No sabe que existe Internet, no sabe que hay bases de datos, no sabe nada. Solo conoce el negocio. Esto hace que las reglas sean puras, seguras y nunca se rompan accidentalmente.

## 🧱 Entidades creadas (Conceptos de negocio)

Se implementaron las siguientes entidades (objetos principales):

- **Product** (Producto): La mercancía que vendemos.
- **Category** (Categoría): Para agrupar los productos (ej. "Electrónica").
- **Brand** (Marca): El fabricante del producto.
- **Review** (Reseña): Las opiniones y calificaciones de los clientes.

Estas clases se encuentran dentro de la carpeta:
```text
ProductAPI.Domain/Entities
```

## 🔒 Reglas de negocio (Cómo protegemos los datos)

La entidad `Product` contiene las reglas principales. Sus propiedades utilizan "candados" (técnicamente llamado `private set`) para que **nadie** pueda modificar un precio o el inventario desde fuera sin usar el método correcto.

### 💰 Actualizar Precio (UpdatePrice)
Permite modificar el precio del producto, pero **bloquea cualquier intento de poner precios negativos**.

### 📦 Agregar Inventario (AddStock)
Permite agregar unidades al almacén. No permite cantidades negativas.

### 📤 Retirar Inventario (RemoveStock)
Permite vender o sacar mercancía. Valida estrictamente que:
- La cantidad a retirar sea mayor a cero.
- Exista suficiente producto en el almacén (no podemos quedar en -5 productos).

## 🧪 Pruebas Unitarias (El Control de Calidad)

¿Cómo sabemos que estas reglas realmente funcionan? Creamos "Pruebas Unitarias" (código que prueba nuestro código). Es como un robot que simula ser un usuario intentando hacer cosas prohibidas para ver si el sistema lo detiene.

Se creó el proyecto de pruebas: `ProductAPI.Domain.Tests`.
Actualmente el robot comprueba de forma automática que:
- Si alguien intenta poner un precio negativo, el sistema lanza una alarma (excepción).
- Si alguien pone un precio válido, el sistema lo actualiza correctamente.

## Estado actual
**COMPLETADA**
Las reglas del negocio están blindadas y listas para la siguiente fase.

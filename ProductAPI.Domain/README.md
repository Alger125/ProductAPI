# Capa de Dominio (ProductAPI.Domain)

Esta capa es el **corazón** de la Arquitectura Limpia. No tiene ninguna dependencia externa (ni bases de datos, ni frameworks web).

## 🛡️ Responsabilidades
1. **Entidades de Negocio:** Clases principales como Product, Category, Brand y Review.
2. **Reglas de Negocio:** Comportamiento centralizado mediante un Modelo de Dominio Rico (ej. UpdatePrice, RemoveStock). Las entidades se protegen a sí mismas validando su propio estado.
3. **Excepciones de Dominio:** Errores específicos del negocio.
4. **Interfaces de Repositorios:** Contratos que la capa de Infraestructura deberá implementar en el futuro.

## 📦 Estructura Actual
* /Entities: Contiene las clases de dominio que mapean nuestro modelo relacional.

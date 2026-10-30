# Taller 1 · Videojuegos — Frontend (Vue)
 
Catálogo de videojuegos: listado, búsqueda, paginación y CRUD (crear/editar/eliminar) protegido con login.
 
## Requisitos
 
- Node.js 18 o superior
- npm
## Instalación
 
```bash
npm install
cp .env.example .env
npm run dev
```
 
Abre `http://localhost:5173` en el navegador.
 
Por defecto la app corre con datos de prueba (`VITE_USE_MOCK=true` en `.env`), así que no necesitas
tener el backend corriendo para probarla.
 
## Guía de uso
 
### Ver el catálogo
 
Al entrar, cualquier persona (con o sin sesión) puede:
 
- Ver todos los juegos en tarjetas, con su imagen, nombre y categoría.
- Buscar por nombre o categoría escribiendo en la barra de búsqueda (el filtro se aplica solo
  con dejar de escribir un momento, no hace falta dar Enter).
- Moverse entre páginas de resultados con los botones "Anterior" / "Siguiente" en la parte de abajo.
### Crear una cuenta e iniciar sesión
 
- En la esquina superior derecha, botón **"Crear cuenta"**: pide correo y contraseña.
- Una vez creada, inicia sesión con **"Iniciar sesión"**.
- Con la sesión iniciada, el botón cambia a **"Cerrar sesión"** y aparecen las opciones para
  agregar y modificar juegos.
> Importante: el login **no separa juegos por usuario**. Cualquier persona con sesión iniciada
> puede crear, editar o eliminar cualquier juego del catálogo — el login es solo el requisito
> para poder escribir, no una forma de tener "tu propia lista".
 
### Agregar un juego (requiere sesión)
 
1. Botón **"+ Nuevo juego"** en la barra superior.
2. Completa nombre, URL de la imagen y categoría.
3. Al guardar, vuelves automáticamente al catálogo y el juego nuevo aparece en la lista.
### Editar o eliminar un juego (requiere sesión)
 
- En cada tarjeta, con sesión iniciada, aparecen los botones **"Editar"** y **"Eliminar"**.
- Editar abre el mismo formulario de creación, pero con los datos ya cargados.
- Eliminar pide una confirmación antes de borrar el juego definitivamente.
### Cerrar sesión
 
Botón **"Cerrar sesión"** en la barra superior. Al hacerlo, ya no podrás crear, editar ni eliminar
juegos hasta volver a iniciar sesión (el catálogo sigue siendo visible para todos).
 
## Conectar con el backend real
 
Cuando el backend de juegos (`/juegos`) y el de autenticación (`/auth`) estén listos:
 
1. En `.env`, cambia `VITE_USE_MOCK=false` y `VITE_API_URL` a la URL real del backend.
2. No hay que tocar ninguna vista ni componente: toda la comunicación pasa por
   `src/services/juegosService.js` y `src/services/authService.js`.
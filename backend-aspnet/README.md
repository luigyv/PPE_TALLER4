# API de Juegos — ASP.NET Core Web API (Proyecto final)

Reemplazo del backend NestJS del Taller #1. Mismo contrato HTTP, así el cliente Vue funciona sin tocar su lógica.

**Stack:** .NET 10 · Controllers · EF Core + SQLite (migraciones) · JWT Bearer · BCrypt · OpenAPI + Scalar.

## 1. Requisitos

- **.NET SDK 10** → `dotnet --version` debe mostrar `10.x`.
  (Si tienes .NET 8 o 9: en `JuegosApi.csproj` cambia `net10.0` por `net8.0`/`net9.0` y los `Version="10.0.*"` por `8.0.*`/`9.0.*`.)
- Node.js 18+ (para Vue).
- Opcional: `dotnet tool install --global dotnet-ef` (solo si quieres regenerar migraciones).

## 2. Levantar la API

```bash
cd backend-aspnet
dotnet restore
dotnet run
```

**Qué debes ver en la consola:**

```
Now listening on: http://localhost:5080
Application started. Press Ctrl+C to shut down.
```

Al arrancar, la app aplica las migraciones y crea `juegos.db`. En modo Development carga además los 13 juegos del mock de Vue (solo si la tabla está vacía).
Se abre el navegador en **http://localhost:5080/scalar** (interfaz interactiva).

Comprobación rápida:

```bash
curl http://localhost:5080/juegos?limit=2
```

Debe devolver JSON con `data`, `total`, `page`, `limit` (más `items` y `totalPages`, ver nota abajo).

## 3. Apuntar el cliente Vue a la nueva API

```bash
cd frontend
cp .env.aspnet.example .env     # en Windows: copy .env.aspnet.example .env
npm install
npm run dev
```

El `.env` queda así:

```
VITE_API_URL=http://localhost:5080
VITE_USE_MOCK=false
```

> `VITE_USE_MOCK=false` es necesario: por defecto el cliente usa datos en memoria y nunca llamaría a ninguna API.

Abre **http://localhost:5173**. Si cambiaste el `.env` con Vite ya corriendo, reinícialo (`Ctrl+C` y `npm run dev`).

## 4. Qué debes ver (verificación del punto 12)

| Prueba | Acción en Vue | Resultado esperado |
|---|---|---|
| Listado | Abrir `/` | 8 tarjetas (limit=8) con los juegos; abajo "Página 1 de 2" |
| Paginación | Clic en "Siguiente →" | Página 2 con los 5 juegos restantes; pestaña Network: `GET /juegos?search=&page=2&limit=8` → 200 |
| Búsqueda | Escribir `hollow` | Solo "Hollow Knight" y "Hollow Knight: Silksong" (no distingue mayúsculas) |
| Sin sesión | Mirar tarjetas | No hay botones Editar/Eliminar ni "+ Nuevo juego" |
| Registro | "Crear cuenta" → correo + clave (mín. 6) | "Cuenta creada. Redirigiendo al login..." (`POST /auth/register` → 201) |
| Registro repetido | Mismo correo otra vez | "No se pudo crear la cuenta..." (409) |
| Login | Iniciar sesión | Redirige al catálogo; aparecen "+ Nuevo juego", Editar, Eliminar (`POST /auth/login` → 200 con `access_token`) |
| Login incorrecto | Clave errada | "Correo o contraseña incorrectos." (401) |
| Crear | "+ Nuevo juego" → llenar → Guardar | Vuelve al catálogo; `POST /juegos` → 201 con header `Authorization: Bearer …` |
| Editar | "Editar" → cambiar nombre → Guardar | `PATCH /juegos/{id}` → 200; la tarjeta muestra el nuevo nombre |
| Eliminar | "Eliminar" → confirmar | `DELETE /juegos/{id}` → 204; la tarjeta desaparece |
| Cerrar sesión | "Cerrar sesión" | Desaparecen los botones de escritura |
| Protección | `curl -X POST http://localhost:5080/juegos -H "Content-Type: application/json" -d "{}"` | 401 `{"message":"Unauthorized","statusCode":401}` |

## 5. Contrato implementado

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| GET | `/juegos?search=&page=1&limit=10` | No | 200 `{ data, total, page, limit }` | — |
| GET | `/juegos/{id}` | No | 200 juego | 400 (id no numérico), 404 |
| POST | `/juegos` | **JWT** | 201 juego | 400, 401 |
| PATCH | `/juegos/{id}` | **JWT** | 200 juego | 400, 401, 404 |
| DELETE | `/juegos/{id}` | **JWT** | 204 sin cuerpo | 401, 404 |
| POST | `/auth/register` | No | 201 `{ id, email }` | 400, 409 |
| POST | `/auth/login` | No | 200 `{ access_token }` | 400, 401 |

Juego: `{ id, nombre, imagen, categoria, creadoEn }` (camelCase). Errores con la forma de NestJS: `{ message, error, statusCode }`.

### Decisiones sobre el contrato (importante para la defensa)

1. **`items` y `totalPages` extra en el listado.** El backend NestJS responde `{ data, total, page, limit }`, pero la vista `JuegosListView.vue` lee `resultado.items` y `resultado.totalPages` (así es el mock). Con el backend NestJS tal cual, la lista no se pinta. Para cumplir "no tocar el cliente", esta API devuelve `data` **y además** `items` (misma lista) y `totalPages`. Es un superconjunto del contrato NestJS: nada de lo anterior se rompe.
2. **`access_token` en snake_case.** `stores/auth.js` lee `data.access_token`; por eso el login lo emite con ese nombre exacto (el resto del JSON es camelCase).
3. **El backend NestJS del repo no incluye módulo de auth** (solo `juegos`; `/auth/*` se dedujo de `authService.js` y `stores/auth.js`): `POST /auth/register` y `POST /auth/login`, body `{ email, password }`.
4. **Búsqueda solo por `nombre`**, igual que el `findAll` de NestJS (el mock de Vue filtraba también por categoría, pero eso no es parte del contrato del servidor).
5. **Puerto 5080** (NestJS usa 3000) para poder tener ambas APIs corriendo y que el cambio de `VITE_API_URL` sea real.

## 6. Arquitectura

```
Controllers/   JuegosController, AuthController     (HTTP, códigos de estado, [Authorize])
Services/      IJuegosService/JuegosService, IAuthService/AuthService, ITokenService/TokenService
               → registrados en Program.cs con AddScoped (inyección de dependencias)
Dtos/          Create/Update/JuegoDto, PaginatedJuegosDto, Register/Login/LoginResponse
Models/        Juego, Usuario                       (entidades EF Core)
Data/          AppDbContext, DbSeeder
Migrations/    InitialCreate + ModelSnapshot
Infrastructure NestError (cuerpo de errores), JwtOptions, OpenApiSecurity (plus)
```

Flujo de seguridad: `register` guarda `BCrypt(password)` → `login` verifica el hash y firma un JWT HS256 (claims `sub`, `email`, `jti`; expira en 60 min) → el middleware JwtBearer valida firma, issuer, audience y expiración → `[Authorize]` deja pasar solo POST/PATCH/DELETE con token válido. CORS permite `http://localhost:5173`.

## 7. Plus: OpenAPI + Scalar

- Documento OpenAPI generado por `Microsoft.AspNetCore.OpenApi`: **http://localhost:5080/openapi/v1.json**
- UI interactiva Scalar: **http://localhost:5080/scalar**

Probar un endpoint protegido: `POST /auth/login` (botón *Test Request*) → copia `access_token` → en el panel de autenticación (candado) elige **Bearer** y pega el token → ejecuta `POST /juegos`. Sin token responde 401.

## 8. Configuración

`appsettings.json` → `Jwt` (clave, issuer, audience, expiración), `Cors:Origins`, `ConnectionStrings:Default`, `Seed:Juegos`.
La clave incluida es solo para desarrollo; en otro entorno: `export Jwt__Key="una-clave-larga-de-32+-caracteres"`.
Para empezar con la BD vacía: borra `juegos.db` y pon `Seed:Juegos` en `false`.

## 9. Migraciones

Ya incluidas (`Migrations/`), se aplican solas al arrancar. Para regenerarlas desde cero:

```bash
dotnet tool install --global dotnet-ef
rm -rf Migrations juegos.db
dotnet ef migrations add InitialCreate
dotnet run
```

## 10. Problemas frecuentes

| Síntoma | Causa / solución |
|---|---|
| Vue no muestra datos y sigue con el mock | Falta `VITE_USE_MOCK=false` o no reiniciaste Vite |
| Error CORS en consola del navegador | Vue corre en otro puerto: agrégalo en `Cors:Origins` |
| `Jwt:Key debe tener al menos 32 caracteres` | Clave demasiado corta en `appsettings.json` / `Jwt__Key` |
| Puerto 5080 ocupado | Cambia `applicationUrl` en `Properties/launchSettings.json` y `VITE_API_URL` |
| No compila la parte de OpenAPI (versiones de paquetes) | Borra `AddDocumentTransformer<...>` en `Program.cs` y el archivo `Infrastructure/OpenApiSecurity.cs`; el resto no depende de eso |

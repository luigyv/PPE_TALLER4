# Preguntas probables — defensa individual

**1. ¿Por qué el cliente Vue no cambia?** Porque la API mantiene el contrato: mismas rutas, verbos, códigos de estado, JSON en camelCase y mismos parámetros (`search`, `page`, `limit`). Solo cambia `VITE_API_URL`.

**2. ¿Qué es la inyección de dependencias y dónde se ve?** El controller recibe `IJuegosService` por constructor; no crea el servicio. En `Program.cs`: `AddScoped<IJuegosService, JuegosService>()`. Facilita pruebas (se puede inyectar un falso) y desacopla capas.

**3. ¿Por qué `AddScoped`?** El `DbContext` es scoped (uno por petición); los servicios que lo usan deben tener el mismo ciclo de vida.

**4. ¿Para qué sirven los DTOs?** Separan lo que entra/sale por HTTP de la entidad de BD. Evitan *overposting* (el cliente no puede fijar `id` ni `creadoEn`) y nunca exponen `PasswordHash`.

**5. ¿Qué hacen las migraciones?** Versionan el esquema como código C#. `Up` crea tablas, `Down` las revierte; `ModelSnapshot` guarda el modelo actual para calcular el siguiente cambio. Se aplican con `Database.MigrateAsync()`.

**6. ¿Cómo se guarda la contraseña?** Con BCrypt: hash con sal y costo adaptable; nunca texto plano. En login se usa `BCrypt.Verify`.

**7. ¿Qué contiene el JWT y cómo se valida?** Header + payload (`sub`, `email`, `jti`, `iss`, `aud`, `exp`) + firma HMAC-SHA256 con la clave secreta. JwtBearer valida firma, emisor, audiencia y expiración en cada petición. El payload va codificado, **no cifrado**: no se guardan secretos en él.

**8. Diferencia entre autenticación y autorización.** Autenticación: ¿quién eres? (`UseAuthentication`, el token). Autorización: ¿qué puedes hacer? (`[Authorize]`). Falla la primera → 401; falla la segunda → 403.

**9. ¿Por qué el orden `UseCors → UseAuthentication → UseAuthorization → MapControllers`?** CORS debe responder el preflight `OPTIONS` antes de exigir token; la autorización necesita que la autenticación ya haya construido el usuario.

**10. ¿Qué es CORS y por qué hizo falta?** El navegador bloquea llamadas entre orígenes distintos (`:5173` → `:5080`). La API declara en cabeceras qué origen permite.

**11. ¿Por qué la lista devuelve `items` y `totalPages` además de `data`?** Porque `JuegosListView.vue` los lee; NestJS devolvía solo `data`. Es un superconjunto compatible.

**12. ¿Por qué `access_token` y no `accessToken`?** El cliente (`stores/auth.js`) lee `data.access_token`; se fijó con `[JsonPropertyName]`.

**13. ¿Por qué `EF.Functions.Like` para buscar?** `Contains` en SQLite se traduce a `instr` (distingue mayúsculas); `LIKE` no, igual que Prisma.

**14. ¿Qué códigos de estado devuelve cada endpoint?** Ver tabla de contrato en el README (200/201/204/400/401/404/409).

**15. ¿Qué es OpenAPI y qué aporta Scalar?** OpenAPI es la especificación estándar que describe la API en JSON; `Microsoft.AspNetCore.OpenApi` la genera. Scalar es la interfaz que la consume para probar endpoints, incluido el esquema Bearer para los protegidos.

**16. ¿Qué mejorarías?** Refresh tokens, roles, límite de intentos de login, HTTPS + clave en un gestor de secretos, pruebas automatizadas con `WebApplicationFactory`.

using System.Text;
using JuegosApi.Data;
using JuegosApi.Infrastructure;
using JuegosApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ---------- Base de datos: EF Core + SQLite ----------
builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=juegos.db")
        // Si el modelo cambia sin generar migración, EF 9+ lanza error al migrar. Lo dejamos como aviso.
        .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));

// ---------- Inyección de dependencias ----------
builder.Services.AddScoped<IJuegosService, JuegosService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// ---------- JWT ----------
var jwtSection = builder.Configuration.GetSection(JwtOptions.Section);
var jwt = jwtSection.Get<JwtOptions>() ?? new JwtOptions();
if (Encoding.UTF8.GetByteCount(jwt.Key) < 32)
    throw new InvalidOperationException("Jwt:Key debe tener al menos 32 caracteres (configura Jwt__Key).");
builder.Services.Configure<JwtOptions>(jwtSection);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };

        // 401 con el mismo cuerpo que NestJS: { message: "Unauthorized", statusCode: 401 }
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return context.Response.WriteAsJsonAsync(new ErrorResponse("Unauthorized", null, 401));
            }
        };
    });
builder.Services.AddAuthorization();

// ---------- CORS para el cliente Vue ----------
var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>()
              ?? ["http://localhost:5173"];
builder.Services.AddCors(options =>
    options.AddPolicy("Vue", policy => policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod()));

// ---------- Controllers + errores de validación con la forma de NestJS ----------
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var mensajes = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();
            return NestError.Make(400, mensajes, "Bad Request");
        };
    });

// ---------- OpenAPI (plus) ----------
builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecurityDocumentTransformer>());

var app = builder.Build();

// ---------- Migraciones (y datos de demo) al arrancar ----------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    if (app.Configuration.GetValue<bool>("Seed:Juegos"))
        await DbSeeder.SeedAsync(db);
}

// ---------- Pipeline ----------
app.MapOpenApi();                 // /openapi/v1.json
app.MapScalarApiReference();      // /scalar  (interfaz interactiva)

app.UseCors("Vue");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

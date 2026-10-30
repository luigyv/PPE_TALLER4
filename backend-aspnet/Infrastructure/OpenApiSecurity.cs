using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace JuegosApi.Infrastructure;

// PLUS: declara el esquema Bearer JWT en el documento OpenAPI y lo marca en los
// endpoints de escritura (POST/PATCH/DELETE de /juegos). Así Scalar muestra el
// candado y permite pegar el token para probar los endpoints protegidos.
internal sealed class BearerSecurityDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Pega el access_token devuelto por POST /auth/login"
            }
        };

        foreach (var (path, item) in document.Paths)
        {
            if (!path.StartsWith("/juegos", StringComparison.OrdinalIgnoreCase) || item.Operations is null)
                continue;

            foreach (var (method, operation) in item.Operations)
            {
                if (method == HttpMethod.Post || method == HttpMethod.Patch || method == HttpMethod.Delete)
                {
                    operation.Security ??= [];
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                    });
                }
            }
        }

        return Task.CompletedTask;
    }
}

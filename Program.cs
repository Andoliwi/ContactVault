using ContactVault.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContactVault")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContactDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Content("""
    <!DOCTYPE html>
    <html lang="es">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>ContactVault - Vault Electrónico de Contactos</title>
        <style>
            body { font-family: 'Segoe UI', Verdana, sans-serif; background: #f4f6fb; color: #222; margin: 0; padding: 0; }
            header { background: #1e2a4a; color: #fff; padding: 28px 40px; }
            header h1 { margin: 0; font-size: 26px; }
            header p { margin: 6px 0 0; opacity: .85; }
            main { max-width: 900px; margin: 32px auto; padding: 0 24px; }
            .card { background: #fff; border: 1px solid #e1e5ee; border-radius: 10px; padding: 20px 24px; margin-bottom: 20px; box-shadow: 0 1px 3px rgba(0,0,0,.06); }
            h2 { margin: 0 0 12px; font-size: 18px; color: #1e2a4a; }
            a { color: #2563eb; text-decoration: none; }
            a:hover { text-decoration: underline; }
            ul { margin: 0; padding-left: 20px; line-height: 1.9; }
            code { background: #eef1f8; padding: 2px 6px; border-radius: 4px; font-size: 13px; }
        </style>
    </head>
    <body>
        <header>
            <h1>Vault Electrónico de Contactos</h1>
            <p>Agenda personal - API REST en ASP.NET Core con SQLite y Docker</p>
        </header>
        <main>
            <div class="card">
                <h2>Accesos directos</h2>
                <ul>
                    <li><a href="/api/contacts">Listar todos los contactos</a></li>
                    <li><a href="/api/contacts?search=perez">Buscar contactos por nombre o apellido (ej. "perez")</a></li>
                    <li><a href="/openapi/v1.json">Documentación de la API (OpenAPI)</a></li>
                </ul>
            </div>
            <div class="card">
                <h2>Endpoints de la API</h2>
                <ul>
                    <li><code>GET</code>&nbsp; /api/contacts - Lista contactos</li>
                    <li><code>GET</code>&nbsp; /api/contacts?search={texto} - Busca por nombre o apellido</li>
                    <li><code>GET</code>&nbsp; /api/contacts/{id} - Detalle de un contacto</li>
                    <li><code>POST</code> /api/contacts - Registrar contacto</li>
                    <li><code>PUT</code>&nbsp; /api/contacts/{id} - Editar contacto</li>
                    <li><code>DELETE</code> /api/contacts/{id} - Eliminar contacto</li>
                </ul>
            </div>
        </main>
    </body>
    </html>
    """, "text/html; charset=utf-8"));

app.Run();
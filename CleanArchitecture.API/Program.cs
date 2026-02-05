using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Identity; // Necesario para ApplicationUser
using CleanArchitecture.API.Endpoints;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios al contenedor 
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

// Servicios básicos de API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- CAMBIO: Configuración de Swagger con soporte para JWT (El botón Authorize) ---
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CleanArchitecture API", Version = "v1" });

    // Definimos la seguridad JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});
// ---------------------------------------------------------------------------------

var app = builder.Build();

// 2. Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// El Orden aca es Clave
app.UseAuthentication();  // 1. Quien sos?
app.UseAuthorization();  //  2. Tenes Permiso?

// Mapear los endpoints automaticos (Login, Register, Refresh) Esto esta re cheto
app.MapGroup("/api/auth").MapIdentityApi<ApplicationUser>(); // Esto mapea los endpoints de Identity de IdentityApi

app.MapControllers();
app.MapProductEndpoints(); // Aca nuestros endpoints customs

app.Run();
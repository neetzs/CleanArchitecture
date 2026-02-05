using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.API.Endpoints; 

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios al contenedor 
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration); // Se cambio la firma para pasar la configuración

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapProductEndpoints();

app.Run();
using Microsoft.EntityFrameworkCore;
using BarberiaSaasApi.Infrastructure.Data;
using BarberiaSaasApi.Core.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuración de Entity Framework Core con SQL Server (asegúrate de que tu cadena de conexión esté en appsettings.json)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de Swagger y OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- BLOQUE DE SEED DATA AUTOMÁTICO ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        // Aseguramos que la base de datos esté creada/migrada
        context.Database.Migrate();

        // Verificamos si ya existen barberías para no duplicar datos
        if (!context.Barberias.Any())
        {
            var barberiaDefault = new Barberia
            {
                Nombre = "Barbería Elite SaaS",
                Direccion = "Av. Principal #456, Centro"
            };

            context.Barberias.Add(barberiaDefault);
            context.SaveChanges(); // Guardamos para generar el ID

            // Agregamos un servicio por defecto
            context.Servicios.Add(new Servicio
            {
                Nombre = "Corte Clásico + Barba",
                Precio = 25.00m,
                DuracionMinutos = 45,
                BarberiaId = barberiaDefault.Id
            });

            // Agregamos un barbero por defecto
            context.Barberos.Add(new Barbero
            {
                Nombre = "Carlos 'El Maestro'",
                Especialidad = "Fade y Diseños",
                BarberiaId = barberiaDefault.Id
            });

            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al insertar los datos de prueba en la base de datos.");
    }
}
// ---------------------------------------

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
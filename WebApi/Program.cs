using Application.Services;
using Core.Interfaces;
using Infrastructure.Repositorios;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuración de Serilog hac
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Configura para escribir en la consola hac
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day) // Configura para escribir en un archivo hac
    .CreateLogger();

// Añadir Serilog al logging de la aplicación 
builder.Logging.ClearProviders(); // Limpiar los proveedores de log predeterminados hac
builder.Logging.AddSerilog(); // Agregar Serilog al pipeline de logging hac



// Configuración JWT hac
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ClaveSecretaSuperSegura1234567899999999"; // Cambia esto por una clave secreta más segura
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "https://localhost:7219";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(); //hac


// Lee la cadena de conexión desde el archivo de configuración
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); //hac

// Registra la cadena de conexión en el contenedor de dependencias
builder.Services.AddSingleton(connectionString);  // O AddScoped/AddTransient dependiendo del caso  hac


// Registra el repositorio con la cadena de conexión inyectada  hac
builder.Services.AddScoped<IPersonaRepository>(provider =>
    new PersonaRepository(connectionString));

// Registra el repositorio con la cadena de conexión inyectada  hac
builder.Services.AddScoped<IUsuarioRepository>(provider =>
    new UsuarioRepository(connectionString));

// Registra el repositorio
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>(); //hac
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>(); //hac


//builder.Services.AddControllers();

// Configurar servicios
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Deshabilitar el formato camelCase si es necesario hac
});


// Registra el servicio 
builder.Services.AddScoped<PersonaService>();  //hac
builder.Services.AddScoped<UsuarioService>();  //hac


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => //hac
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Personas",
        Version = "v1"
    });

    // Configuración del esquema de seguridad para JWT
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Introduce el token JWT en el campo siguiente. Ejemplo: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// Agregar CORS antes de Build() hac
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7138") // Cambia esto según la URL del frontend
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



var app = builder.Build();


// Usa Serilog en tu aplicación
app.MapGet("/", () => "Hello World!"); //hac

// Cerrar loggers cuando la aplicación termine
app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush); //hac

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Habilita autenticación hac
app.UseAuthorization();  // Habilita autorización hac

app.UseAuthorization();

// Aplicar la política de CORS
app.UseCors("AllowFrontend"); //hac

app.MapControllers();

app.Run();

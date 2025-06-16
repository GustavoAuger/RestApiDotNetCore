using Microsoft.EntityFrameworkCore;
using SupabaseApiDemo.Data;
using SupabaseApiDemo.Services;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Cargar el archivo .env
Env.Load();
Console.WriteLine($"Directorio actual: {Directory.GetCurrentDirectory()}");
Console.WriteLine($"¿Existe .env? {File.Exists(".env")}");

// Cargar el archivo .env


// 🔍 Debug: Ver qué variables se cargaron
Console.WriteLine($"DB_HOST: '{Environment.GetEnvironmentVariable("DB_HOST")}'");
Console.WriteLine($"DB_PORT: '{Environment.GetEnvironmentVariable("DB_PORT")}'");
// Validar variables de entorno requeridas
var requiredEnvVars = new[] { "DB_HOST", "DB_PORT", "DB_USER", "DB_PASSWORD", "DB_NAME" };
foreach (var envVar in requiredEnvVars)
{
    if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(envVar)))
    {
        throw new InvalidOperationException($"Variable de entorno requerida '{envVar}' no está configurada.");
    }
}

// Crear connection string
string connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
    $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
    $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
    $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
    $"Pooling=false;MinPoolSize=0;MaxPoolSize=10;ConnectionIdleLifetime=60;" +
    $"CommandTimeout=30;Timeout=30;SslMode=Require;"; // SSL obligatorio para Supabase

// Configure Entity Framework with PostgreSQL (Supabase)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register custom services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure database is created (optional, for development)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
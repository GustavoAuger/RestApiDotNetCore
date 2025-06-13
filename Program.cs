using DotNetEnv;
using SupabaseApiDemo.Services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

string connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                          $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                          $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                          $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                          $"Database={Environment.GetEnvironmentVariable("DB_NAME")}";

builder.Services.AddSingleton(new UsuarioService(connectionString));
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();


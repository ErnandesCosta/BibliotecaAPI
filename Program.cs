using BibliotecaAPI.Data;
using BibliotecaAPI.Middleware;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registra os Controllers da API.
builder.Services.AddControllers();

// Configura o Entity Framework Core com SQLite.
builder.Services.AddDbContext<BibliotecaContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString(
            "DefaultConnection");

    options.UseSqlite(connectionString);
});

// Configura respostas padronizadas para erros.
builder.Services.AddProblemDetails();

// Registra o tratamento global de exceções.
builder.Services.AddExceptionHandler<
    GlobalExceptionHandler>();

// Repository de autores.
builder.Services.AddScoped<
    IAutorRepository,
    AutorRepository>();

// Service de autores.
builder.Services.AddScoped<
    IAutorService,
    AutorService>();

// Repository de livros.
builder.Services.AddScoped<
    ILivroRepository,
    LivroRepository>();

// Service de livros.
builder.Services.AddScoped<
    ILivroService,
    LivroService>();

builder.Services.AddScoped<
    IAlunoRepository,
    AlunoRepository>();

builder.Services.AddScoped<
    IAlunoService,
    AlunoService>();

// Configura Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ativa o tratamento global de exceções.
// Deve ficar antes do MapControllers.
app.UseExceptionHandler();

// Ativa o Swagger no ambiente de desenvolvimento.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();
using BibliotecaAPI.Data;
using BibliotecaAPI.Middleware;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BibliotecaContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString(
            "DefaultConnection");

    options.UseSqlite(connectionString);
});

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<
    GlobalExceptionHandler>();

builder.Services.AddScoped<
    IAutorRepository,
    AutorRepository>();

builder.Services.AddScoped<
    IAutorService,
    AutorService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
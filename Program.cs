using BibliotecaAPI.Data;
using BibliotecaAPI.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<
    GlobalExceptionHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Desativado temporariamente porque a aplicação está usando HTTP.
// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories;

public class AutorRepository : IAutorRepository
{
    private readonly BibliotecaContext _context;

    public AutorRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<List<Autor>> ListarAsync()
    {
        return await _context.Autores
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Autor?> BuscarPorIdAsync(int id)
    {
        return await _context.Autores
            .AsNoTracking()
            .FirstOrDefaultAsync(autor => autor.Id == id);
    }

    public async Task AdicionarAsync(Autor autor)
    {
        await _context.Autores.AddAsync(autor);
    }

    public async Task SalvarAlteracoesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
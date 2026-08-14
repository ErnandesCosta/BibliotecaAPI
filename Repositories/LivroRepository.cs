using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories;

public class LivroRepository : ILivroRepository
{
    private readonly BibliotecaContext _context;

    public LivroRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<List<Livro>> ListarAsync(
        string? titulo,
        string? autor)
    {
        var query = _context.Livros
            .Include(livro => livro.Autor)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(titulo))
        {
            query = query.Where(livro =>
                livro.Titulo.ToLower()
                    .Contains(titulo.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(autor))
        {
            query = query.Where(livro =>
                livro.Autor != null &&
                livro.Autor.Nome.ToLower()
                    .Contains(autor.ToLower()));
        }

        return await query
            .OrderBy(livro => livro.Titulo)
            .ToListAsync();
    }

    public async Task<Livro?> BuscarPorIdAsync(int id)
    {
        return await _context.Livros
            .Include(livro => livro.Autor)
            .AsNoTracking()
            .FirstOrDefaultAsync(livro => livro.Id == id);
    }

    public async Task<Livro?> BuscarPorIdComRastreamentoAsync(
        int id)
    {
        return await _context.Livros
            .FirstOrDefaultAsync(livro => livro.Id == id);
    }

    public async Task AdicionarAsync(Livro livro)
    {
        await _context.Livros.AddAsync(livro);
    }

    public async Task SalvarAlteracoesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
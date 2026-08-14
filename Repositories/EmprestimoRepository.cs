using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories;

public class EmprestimoRepository : IEmprestimoRepository
{
    private readonly BibliotecaContext _context;

    public EmprestimoRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<List<Emprestimo>> ListarAsync()
    {
        return await _context.Emprestimos
            .Include(emprestimo => emprestimo.Aluno)
            .Include(emprestimo => emprestimo.Livro)
            .AsNoTracking()
            .OrderByDescending(
                emprestimo => emprestimo.DataEmprestimo)
            .ToListAsync();
    }

    public async Task<Emprestimo?> BuscarPorIdAsync(int id)
    {
        return await _context.Emprestimos
            .Include(emprestimo => emprestimo.Aluno)
            .Include(emprestimo => emprestimo.Livro)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                emprestimo => emprestimo.Id == id);
    }

    public async Task<bool> ExisteEmprestimoAtivoAsync(
        int alunoId,
        int livroId)
    {
        return await _context.Emprestimos
            .AnyAsync(emprestimo =>
                emprestimo.AlunoId == alunoId &&
                emprestimo.LivroId == livroId &&
                emprestimo.Status ==
                    StatusEmprestimo.Ativo);
    }

    public async Task AdicionarAsync(
        Emprestimo emprestimo)
    {
        await _context.Emprestimos.AddAsync(emprestimo);
    }

    public async Task SalvarAlteracoesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
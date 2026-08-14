using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories;

public class AlunoRepository : IAlunoRepository
{
    private readonly BibliotecaContext _context;

    public AlunoRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<Aluno?> BuscarPorIdAsync(int id)
    {
        return await _context.Alunos
            .AsNoTracking()
            .FirstOrDefaultAsync(aluno => aluno.Id == id);
    }

    public async Task<Aluno?> BuscarPorMatriculaAsync(
        string matricula)
    {
        return await _context.Alunos
            .AsNoTracking()
            .FirstOrDefaultAsync(aluno =>
                aluno.Matricula == matricula);
    }

    public async Task<List<Aluno>> ListarAsync()
    {
        return await _context.Alunos
            .AsNoTracking()
            .OrderBy(aluno => aluno.Nome)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Aluno aluno)
    {
        await _context.Alunos.AddAsync(aluno);
    }

    public async Task SalvarAlteracoesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
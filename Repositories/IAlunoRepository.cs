using BibliotecaAPI.Models;

namespace BibliotecaAPI.Repositories;

public interface IAlunoRepository
{
    Task<Aluno?> BuscarPorIdAsync(int id);

    Task<Aluno?> BuscarPorMatriculaAsync(
        string matricula);

    Task<List<Aluno>> ListarAsync();

    Task AdicionarAsync(Aluno aluno);

    Task SalvarAlteracoesAsync();
}
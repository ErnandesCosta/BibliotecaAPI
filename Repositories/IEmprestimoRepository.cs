using BibliotecaAPI.Models;

namespace BibliotecaAPI.Repositories;

public interface IEmprestimoRepository
{
    Task<List<Emprestimo>> ListarAsync();

    Task<Emprestimo?> BuscarPorIdAsync(int id);

    Task<bool> ExisteEmprestimoAtivoAsync(
        int alunoId,
        int livroId);

    Task AdicionarAsync(Emprestimo emprestimo);

    Task SalvarAlteracoesAsync();
}
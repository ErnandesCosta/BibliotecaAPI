using BibliotecaAPI.Models;

namespace BibliotecaAPI.Repositories;

public interface ILivroRepository
{
    Task<List<Livro>> ListarAsync(
        string? titulo,
        string? autor);

    Task<Livro?> BuscarPorIdAsync(int id);

    Task AdicionarAsync(Livro livro);

    Task SalvarAlteracoesAsync();
}
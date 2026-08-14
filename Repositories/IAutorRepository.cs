using BibliotecaAPI.Models;

namespace BibliotecaAPI.Repositories;

public interface IAutorRepository
{
    Task<List<Autor>> ListarAsync();

    Task<Autor?> BuscarPorIdAsync(int id);

    Task AdicionarAsync(Autor autor);

    Task SalvarAlteracoesAsync();
}
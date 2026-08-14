using BibliotecaAPI.DTOs;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;

namespace BibliotecaAPI.Services;

public class AutorService : IAutorService
{
    private readonly IAutorRepository _autorRepository;

    public AutorService(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public async Task<AutorResponseDto> CriarAsync(
        CriarAutorDto dto)
    {
        var autor = new Autor
        {
            Nome = dto.Nome,
            DataNascimento = dto.DataNascimento,
            Nacionalidade = dto.Nacionalidade
        };

        await _autorRepository.AdicionarAsync(autor);
        await _autorRepository.SalvarAlteracoesAsync();

        return ConverterParaResponse(autor);
    }

    public async Task<List<AutorResponseDto>> ListarAsync()
    {
        var autores = await _autorRepository.ListarAsync();

        return autores
            .Select(ConverterParaResponse)
            .ToList();
    }

    public async Task<AutorResponseDto> BuscarPorIdAsync(
        int id)
    {
        var autor = await _autorRepository.BuscarPorIdAsync(id);

        if (autor is null)
        {
            throw new NotFoundException(
                $"O autor com ID {id} não foi encontrado.");
        }

        return ConverterParaResponse(autor);
    }

    private static AutorResponseDto ConverterParaResponse(
        Autor autor)
    {
        return new AutorResponseDto
        {
            Id = autor.Id,
            Nome = autor.Nome,
            DataNascimento = autor.DataNascimento,
            Nacionalidade = autor.Nacionalidade
        };
    }
}
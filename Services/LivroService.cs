using BibliotecaAPI.DTOs;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;

namespace BibliotecaAPI.Services;

public class LivroService : ILivroService
{
    private readonly ILivroRepository _livroRepository;
    private readonly IAutorRepository _autorRepository;

    public LivroService(
        ILivroRepository livroRepository,
        IAutorRepository autorRepository)
    {
        _livroRepository = livroRepository;
        _autorRepository = autorRepository;
    }

    public async Task<LivroResponseDto> CriarAsync(
        CriarLivroDto dto)
    {
        var autor = await _autorRepository
            .BuscarPorIdAsync(dto.AutorId);

        if (autor is null)
        {
            throw new NotFoundException(
                $"O autor com ID {dto.AutorId} não foi encontrado.");
        }

        var livro = new Livro
        {
            ISBN = dto.ISBN,
            Titulo = dto.Titulo,
            AnoPublicacao = dto.AnoPublicacao,
            Quantidade = dto.Quantidade,
            AutorId = dto.AutorId
        };

        await _livroRepository.AdicionarAsync(livro);
        await _livroRepository.SalvarAlteracoesAsync();

        livro.Autor = autor;

        return ConverterParaResponse(livro);
    }

    public async Task<List<LivroResponseDto>> ListarAsync(
        string? titulo,
        string? autor)
    {
        var livros = await _livroRepository
            .ListarAsync(titulo, autor);

        return livros
            .Select(ConverterParaResponse)
            .ToList();
    }

    public async Task<LivroResponseDto> BuscarPorIdAsync(
        int id)
    {
        var livro = await _livroRepository
            .BuscarPorIdAsync(id);

        if (livro is null)
        {
            throw new NotFoundException(
                $"O livro com ID {id} não foi encontrado.");
        }

        return ConverterParaResponse(livro);
    }

    private static LivroResponseDto ConverterParaResponse(
        Livro livro)
    {
        return new LivroResponseDto
        {
            Id = livro.Id,
            ISBN = livro.ISBN,
            Titulo = livro.Titulo,
            AnoPublicacao = livro.AnoPublicacao,
            Quantidade = livro.Quantidade,
            AutorId = livro.AutorId,
            NomeAutor = livro.Autor?.Nome
        };
    }
}
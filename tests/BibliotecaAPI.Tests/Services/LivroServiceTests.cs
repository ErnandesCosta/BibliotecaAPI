using BibliotecaAPI.DTOs;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Services;
using Moq;

namespace BibliotecaAPI.Tests.Services;

public class LivroServiceTests
{
    private readonly Mock<ILivroRepository> _livroRepository = new();
    private readonly Mock<IAutorRepository> _autorRepository = new();
    private readonly LivroService _service;

    public LivroServiceTests()
    {
        _service = new LivroService(
            _livroRepository.Object,
            _autorRepository.Object);
    }

    [Fact]
    public async Task CriarAsync_AutorExistente_PersisteERetornaComNomeDoAutor()
    {
        var dto = new CriarLivroDto
        {
            ISBN = "978-1",
            Titulo = "Dom Casmurro",
            AnoPublicacao = 1899,
            Quantidade = 3,
            AutorId = 7
        };
        _autorRepository
            .Setup(r => r.BuscarPorIdAsync(7))
            .ReturnsAsync(new Autor { Id = 7, Nome = "Machado" });

        var resultado = await _service.CriarAsync(dto);

        Assert.Equal("Dom Casmurro", resultado.Titulo);
        Assert.Equal("Machado", resultado.NomeAutor);
        _livroRepository.Verify(r => r.AdicionarAsync(It.IsAny<Livro>()), Times.Once);
        _livroRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_AutorInexistente_LancaNotFoundENaoPersiste()
    {
        var dto = new CriarLivroDto { AutorId = 7, Titulo = "X" };
        _autorRepository
            .Setup(r => r.BuscarPorIdAsync(7))
            .ReturnsAsync((Autor?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.CriarAsync(dto));

        _livroRepository.Verify(r => r.AdicionarAsync(It.IsAny<Livro>()), Times.Never);
    }

    [Fact]
    public async Task BuscarPorIdAsync_LivroInexistente_LancaNotFound()
    {
        _livroRepository
            .Setup(r => r.BuscarPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Livro?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.BuscarPorIdAsync(1));
    }
}

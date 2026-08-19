using BibliotecaAPI.DTOs;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Services;
using Moq;

namespace BibliotecaAPI.Tests.Services;

public class AutorServiceTests
{
    private readonly Mock<IAutorRepository> _autorRepository = new();
    private readonly AutorService _service;

    public AutorServiceTests()
    {
        _service = new AutorService(_autorRepository.Object);
    }

    [Fact]
    public async Task CriarAsync_DadosValidos_PersisteERetornaResponse()
    {
        var dto = new CriarAutorDto
        {
            Nome = "Machado de Assis",
            DataNascimento = new DateTime(1839, 6, 21),
            Nacionalidade = "Brasileira"
        };

        var resultado = await _service.CriarAsync(dto);

        Assert.Equal(dto.Nome, resultado.Nome);
        Assert.Equal(dto.Nacionalidade, resultado.Nacionalidade);
        _autorRepository.Verify(r => r.AdicionarAsync(It.IsAny<Autor>()), Times.Once);
        _autorRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task BuscarPorIdAsync_AutorExistente_RetornaResponse()
    {
        var autor = new Autor { Id = 5, Nome = "Clarice Lispector" };
        _autorRepository
            .Setup(r => r.BuscarPorIdAsync(5))
            .ReturnsAsync(autor);

        var resultado = await _service.BuscarPorIdAsync(5);

        Assert.Equal(5, resultado.Id);
        Assert.Equal("Clarice Lispector", resultado.Nome);
    }

    [Fact]
    public async Task BuscarPorIdAsync_AutorInexistente_LancaNotFound()
    {
        _autorRepository
            .Setup(r => r.BuscarPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Autor?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.BuscarPorIdAsync(99));
    }

    [Fact]
    public async Task ListarAsync_RetornaTodosOsAutores()
    {
        _autorRepository
            .Setup(r => r.ListarAsync())
            .ReturnsAsync(new List<Autor>
            {
                new() { Id = 1, Nome = "A" },
                new() { Id = 2, Nome = "B" }
            });

        var resultado = await _service.ListarAsync();

        Assert.Equal(2, resultado.Count);
    }
}

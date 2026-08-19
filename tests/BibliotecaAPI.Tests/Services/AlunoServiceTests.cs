using BibliotecaAPI.DTOs;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Services;
using Moq;

namespace BibliotecaAPI.Tests.Services;

public class AlunoServiceTests
{
    private readonly Mock<IAlunoRepository> _alunoRepository = new();
    private readonly AlunoService _service;

    public AlunoServiceTests()
    {
        _service = new AlunoService(_alunoRepository.Object);
    }

    [Fact]
    public async Task CriarAsync_MatriculaInedita_PersisteERetornaResponse()
    {
        var dto = new CriarAlunoDto
        {
            Nome = "Ana",
            Matricula = "2024001",
            Email = "ana@escola.com"
        };
        _alunoRepository
            .Setup(r => r.BuscarPorMatriculaAsync(dto.Matricula))
            .ReturnsAsync((Aluno?)null);

        var resultado = await _service.CriarAsync(dto);

        Assert.Equal(dto.Matricula, resultado.Matricula);
        _alunoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Aluno>()), Times.Once);
        _alunoRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_MatriculaDuplicada_LancaConflitoENaoPersiste()
    {
        var dto = new CriarAlunoDto
        {
            Nome = "Ana",
            Matricula = "2024001",
            Email = "ana@escola.com"
        };
        _alunoRepository
            .Setup(r => r.BuscarPorMatriculaAsync(dto.Matricula))
            .ReturnsAsync(new Aluno { Id = 1, Matricula = dto.Matricula });

        await Assert.ThrowsAsync<BusinessConflictException>(
            () => _service.CriarAsync(dto));

        _alunoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Aluno>()), Times.Never);
        _alunoRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
    }

    [Fact]
    public async Task BuscarPorIdAsync_AlunoInexistente_LancaNotFound()
    {
        _alunoRepository
            .Setup(r => r.BuscarPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Aluno?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.BuscarPorIdAsync(42));
    }
}

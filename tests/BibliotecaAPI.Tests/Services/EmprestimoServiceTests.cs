using BibliotecaAPI.DTOs;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Services;
using Moq;

namespace BibliotecaAPI.Tests.Services;

public class EmprestimoServiceTests
{
    private readonly Mock<IEmprestimoRepository> _emprestimoRepository = new();
    private readonly Mock<IAlunoRepository> _alunoRepository = new();
    private readonly Mock<ILivroRepository> _livroRepository = new();
    private readonly EmprestimoService _service;

    public EmprestimoServiceTests()
    {
        _service = new EmprestimoService(
            _emprestimoRepository.Object,
            _alunoRepository.Object,
            _livroRepository.Object);
    }

    private CriarEmprestimoDto DtoValido() => new()
    {
        AlunoId = 1,
        LivroId = 2,
        DataPrevistaDevolucao = DateTime.UtcNow.AddDays(7)
    };

    private void SetupAlunoELivroDisponiveis(int quantidade = 1)
    {
        _alunoRepository
            .Setup(r => r.BuscarPorIdAsync(1))
            .ReturnsAsync(new Aluno { Id = 1, Nome = "Ana" });
        _livroRepository
            .Setup(r => r.BuscarPorIdComRastreamentoAsync(2))
            .ReturnsAsync(new Livro { Id = 2, Titulo = "Livro", Quantidade = quantidade });
        _emprestimoRepository
            .Setup(r => r.ExisteEmprestimoAtivoAsync(1, 2))
            .ReturnsAsync(false);
    }

    [Fact]
    public async Task CriarAsync_DadosValidos_DecrementaEstoqueEPersiste()
    {
        var livro = new Livro { Id = 2, Titulo = "Livro", Quantidade = 3 };
        _alunoRepository
            .Setup(r => r.BuscarPorIdAsync(1))
            .ReturnsAsync(new Aluno { Id = 1, Nome = "Ana" });
        _livroRepository
            .Setup(r => r.BuscarPorIdComRastreamentoAsync(2))
            .ReturnsAsync(livro);
        _emprestimoRepository
            .Setup(r => r.ExisteEmprestimoAtivoAsync(1, 2))
            .ReturnsAsync(false);

        var resultado = await _service.CriarAsync(DtoValido());

        Assert.Equal(2, livro.Quantidade);
        Assert.Equal(StatusEmprestimo.Ativo, resultado.Status);
        _emprestimoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Emprestimo>()), Times.Once);
        _emprestimoRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_AlunoInexistente_LancaNotFound()
    {
        _alunoRepository
            .Setup(r => r.BuscarPorIdAsync(1))
            .ReturnsAsync((Aluno?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.CriarAsync(DtoValido()));
    }

    [Fact]
    public async Task CriarAsync_EmprestimoAtivoDuplicado_LancaConflito()
    {
        _alunoRepository
            .Setup(r => r.BuscarPorIdAsync(1))
            .ReturnsAsync(new Aluno { Id = 1 });
        _livroRepository
            .Setup(r => r.BuscarPorIdComRastreamentoAsync(2))
            .ReturnsAsync(new Livro { Id = 2, Quantidade = 5 });
        _emprestimoRepository
            .Setup(r => r.ExisteEmprestimoAtivoAsync(1, 2))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<BusinessConflictException>(
            () => _service.CriarAsync(DtoValido()));

        _emprestimoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Emprestimo>()), Times.Never);
    }

    [Fact]
    public async Task CriarAsync_SemExemplaresDisponiveis_LancaConflito()
    {
        SetupAlunoELivroDisponiveis(quantidade: 0);

        await Assert.ThrowsAsync<BusinessConflictException>(
            () => _service.CriarAsync(DtoValido()));

        _emprestimoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Emprestimo>()), Times.Never);
    }

    [Fact]
    public async Task DevolverAsync_EmprestimoAtivo_IncrementaEstoqueEMarcaDevolvido()
    {
        var livro = new Livro { Id = 2, Quantidade = 1 };
        _emprestimoRepository
            .Setup(r => r.BuscarPorIdAsync(10))
            .ReturnsAsync(new Emprestimo
            {
                Id = 10,
                LivroId = 2,
                Status = StatusEmprestimo.Ativo
            });
        _livroRepository
            .Setup(r => r.BuscarPorIdComRastreamentoAsync(2))
            .ReturnsAsync(livro);

        var resultado = await _service.DevolverAsync(10);

        Assert.Equal(StatusEmprestimo.Devolvido, resultado.Status);
        Assert.NotNull(resultado.DataDevolucao);
        Assert.Equal(2, livro.Quantidade);
        _emprestimoRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task DevolverAsync_JaDevolvido_LancaConflito()
    {
        _emprestimoRepository
            .Setup(r => r.BuscarPorIdAsync(10))
            .ReturnsAsync(new Emprestimo
            {
                Id = 10,
                LivroId = 2,
                Status = StatusEmprestimo.Devolvido
            });

        await Assert.ThrowsAsync<BusinessConflictException>(
            () => _service.DevolverAsync(10));
    }

    [Fact]
    public async Task DevolverAsync_EmprestimoInexistente_LancaNotFound()
    {
        _emprestimoRepository
            .Setup(r => r.BuscarPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Emprestimo?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.DevolverAsync(1));
    }
}

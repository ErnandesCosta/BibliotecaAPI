using BibliotecaAPI.DTOs;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;

namespace BibliotecaAPI.Services;

public class EmprestimoService : IEmprestimoService
{
    private readonly IEmprestimoRepository
        _emprestimoRepository;

    private readonly IAlunoRepository
        _alunoRepository;

    private readonly ILivroRepository
        _livroRepository;

    public EmprestimoService(
        IEmprestimoRepository emprestimoRepository,
        IAlunoRepository alunoRepository,
        ILivroRepository livroRepository)
    {
        _emprestimoRepository = emprestimoRepository;
        _alunoRepository = alunoRepository;
        _livroRepository = livroRepository;
    }

    public async Task<List<EmprestimoResponseDto>> ListarAsync()
    {
        var emprestimos = await _emprestimoRepository
            .ListarAsync();

        return emprestimos
            .Select(ConverterParaResponse)
            .ToList();
    }

    public async Task<EmprestimoResponseDto> CriarAsync(
        CriarEmprestimoDto dto)
    {
        var aluno = await _alunoRepository
            .BuscarPorIdAsync(dto.AlunoId);

        if (aluno is null)
        {
            throw new NotFoundException(
                $"O aluno com ID {dto.AlunoId} não foi encontrado.");
        }

        var livro = await _livroRepository
            .BuscarPorIdComRastreamentoAsync(dto.LivroId);

        if (livro is null)
        {
            throw new NotFoundException(
                $"O livro com ID {dto.LivroId} não foi encontrado.");
        }

        var emprestimoDuplicado =
            await _emprestimoRepository
                .ExisteEmprestimoAtivoAsync(
                    dto.AlunoId,
                    dto.LivroId);

        if (emprestimoDuplicado)
        {
            throw new BusinessConflictException(
                "O aluno já possui um empréstimo ativo deste livro.");
        }

        if (livro.Quantidade <= 0)
        {
            throw new BusinessConflictException(
                "O livro não possui exemplares disponíveis.");
        }

        livro.Quantidade--;

        var emprestimo = new Emprestimo
        {
            AlunoId = dto.AlunoId,
            LivroId = dto.LivroId,
            DataEmprestimo = DateTime.UtcNow,
            DataPrevistaDevolucao =
                dto.DataPrevistaDevolucao,
            Status = StatusEmprestimo.Ativo
        };

        await _emprestimoRepository
            .AdicionarAsync(emprestimo);

        await _emprestimoRepository
            .SalvarAlteracoesAsync();

        emprestimo.Aluno = aluno;
        emprestimo.Livro = livro;

        return ConverterParaResponse(emprestimo);
    }

    public async Task<EmprestimoResponseDto> BuscarPorIdAsync(
        int id)
    {
        var emprestimo = await _emprestimoRepository
            .BuscarPorIdAsync(id);

        if (emprestimo is null)
        {
            throw new NotFoundException(
                $"O empréstimo com ID {id} não foi encontrado.");
        }

        return ConverterParaResponse(emprestimo);
    }

    public async Task<EmprestimoResponseDto> DevolverAsync(
        int id)
    {
        var emprestimo = await _emprestimoRepository
            .BuscarPorIdAsync(id);

        if (emprestimo is null)
        {
            throw new NotFoundException(
                $"O empréstimo com ID {id} não foi encontrado.");
        }

        if (emprestimo.Status ==
            StatusEmprestimo.Devolvido)
        {
            throw new BusinessConflictException(
                "Este empréstimo já foi devolvido.");
        }

        var livro = await _livroRepository
            .BuscarPorIdComRastreamentoAsync(
                emprestimo.LivroId);

        if (livro is null)
        {
            throw new NotFoundException(
                $"O livro com ID {emprestimo.LivroId} não foi encontrado.");
        }

        emprestimo.Status = StatusEmprestimo.Devolvido;
        emprestimo.DataDevolucao = DateTime.UtcNow;

        livro.Quantidade++;

        await _emprestimoRepository
            .SalvarAlteracoesAsync();

        emprestimo.Livro = livro;

        return ConverterParaResponse(emprestimo);
    }

    private static EmprestimoResponseDto
        ConverterParaResponse(Emprestimo emprestimo)
    {
        return new EmprestimoResponseDto
        {
            Id = emprestimo.Id,
            AlunoId = emprestimo.AlunoId,
            NomeAluno = emprestimo.Aluno?.Nome,
            LivroId = emprestimo.LivroId,
            TituloLivro = emprestimo.Livro?.Titulo,
            DataEmprestimo =
                emprestimo.DataEmprestimo,
            DataPrevistaDevolucao =
                emprestimo.DataPrevistaDevolucao,
            DataDevolucao =
                emprestimo.DataDevolucao,
            Status = emprestimo.Status
        };
    }
}
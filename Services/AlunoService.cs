using BibliotecaAPI.DTOs;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;

namespace BibliotecaAPI.Services;

public class AlunoService : IAlunoService
{
    private readonly IAlunoRepository _alunoRepository;

    public AlunoService(IAlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public async Task<AlunoResponseDto> CriarAsync(
        CriarAlunoDto dto)
    {
        var alunoExistente = await _alunoRepository
            .BuscarPorMatriculaAsync(dto.Matricula);

        if (alunoExistente is not null)
        {
            throw new BusinessConflictException(
                $"A matrícula '{dto.Matricula}' já está cadastrada.");
        }

        var aluno = new Aluno
        {
            Nome = dto.Nome,
            Matricula = dto.Matricula,
            Email = dto.Email
        };

        await _alunoRepository.AdicionarAsync(aluno);
        await _alunoRepository.SalvarAlteracoesAsync();

        return ConverterParaResponse(aluno);
    }

    public async Task<List<AlunoResponseDto>> ListarAsync()
    {
        var alunos = await _alunoRepository.ListarAsync();

        return alunos
            .Select(ConverterParaResponse)
            .ToList();
    }

    public async Task<AlunoResponseDto> BuscarPorIdAsync(
        int id)
    {
        var aluno = await _alunoRepository
            .BuscarPorIdAsync(id);

        if (aluno is null)
        {
            throw new NotFoundException(
                $"O aluno com ID {id} não foi encontrado.");
        }

        return ConverterParaResponse(aluno);
    }

    private static AlunoResponseDto ConverterParaResponse(
        Aluno aluno)
    {
        return new AlunoResponseDto
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            Matricula = aluno.Matricula,
            Email = aluno.Email
        };
    }
}
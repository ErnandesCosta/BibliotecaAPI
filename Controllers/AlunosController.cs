using BibliotecaAPI.DTOs;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/alunos")]
public class AlunosController : ControllerBase
{
    private readonly IAlunoService _alunoService;

    public AlunosController(IAlunoService alunoService)
    {
        _alunoService = alunoService;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(AlunoResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AlunoResponseDto>> Criar(
        CriarAlunoDto dto)
    {
        var aluno = await _alunoService.CriarAsync(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = aluno.Id },
            aluno);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(List<AlunoResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AlunoResponseDto>>> Listar()
    {
        var alunos = await _alunoService.ListarAsync();

        return Ok(alunos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(AlunoResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AlunoResponseDto>> BuscarPorId(
        int id)
    {
        var aluno = await _alunoService
            .BuscarPorIdAsync(id);

        return Ok(aluno);
    }
}
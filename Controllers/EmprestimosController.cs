using BibliotecaAPI.DTOs;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/emprestimos")]
public class EmprestimosController : ControllerBase
{
    private readonly IEmprestimoService
        _emprestimoService;

    public EmprestimosController(
        IEmprestimoService emprestimoService)
    {
        _emprestimoService = emprestimoService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(List<EmprestimoResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<
        ActionResult<List<EmprestimoResponseDto>>> Listar()
    {
        var emprestimos = await _emprestimoService
            .ListarAsync();

        return Ok(emprestimos);
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(EmprestimoResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EmprestimoResponseDto>> Criar(
        CriarEmprestimoDto dto)
    {
        var emprestimo = await _emprestimoService
            .CriarAsync(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = emprestimo.Id },
            emprestimo);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(EmprestimoResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmprestimoResponseDto>> BuscarPorId(
        int id)
    {
        var emprestimo = await _emprestimoService
            .BuscarPorIdAsync(id);

        return Ok(emprestimo);
    }

    [HttpPut("{id:int}/devolucao")]
    [ProducesResponseType(
        typeof(EmprestimoResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EmprestimoResponseDto>> Devolver(
        int id)
    {
        var emprestimo = await _emprestimoService
            .DevolverAsync(id);

        return Ok(emprestimo);
    }
}
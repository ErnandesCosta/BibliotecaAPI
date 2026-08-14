using BibliotecaAPI.DTOs;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/livros")]
public class LivrosController : ControllerBase
{
    private readonly ILivroService _livroService;

    public LivrosController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(LivroResponseDto),
        StatusCodes.Status201Created)]
    public async Task<ActionResult<LivroResponseDto>> Criar(
        CriarLivroDto dto)
    {
        var livro = await _livroService.CriarAsync(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = livro.Id },
            livro);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(List<LivroResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<List<LivroResponseDto>>> Listar(
        [FromQuery] string? titulo,
        [FromQuery] string? autor)
    {
        var livros = await _livroService
            .ListarAsync(titulo, autor);

        return Ok(livros);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(LivroResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LivroResponseDto>> BuscarPorId(
        int id)
    {
        var livro = await _livroService
            .BuscarPorIdAsync(id);

        return Ok(livro);
    }
}
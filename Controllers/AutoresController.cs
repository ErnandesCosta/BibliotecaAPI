using BibliotecaAPI.DTOs;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/autores")]
public class AutoresController : ControllerBase
{
    private readonly IAutorService _autorService;

    public AutoresController(IAutorService autorService)
    {
        _autorService = autorService;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(AutorResponseDto),
        StatusCodes.Status201Created)]
    public async Task<ActionResult<AutorResponseDto>> Criar(
        CriarAutorDto dto)
    {
        var autor = await _autorService.CriarAsync(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = autor.Id },
            autor);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(List<AutorResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AutorResponseDto>>> Listar()
    {
        var autores = await _autorService.ListarAsync();

        return Ok(autores);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(AutorResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AutorResponseDto>> BuscarPorId(
        int id)
    {
        var autor = await _autorService.BuscarPorIdAsync(id);

        return Ok(autor);
    }
}
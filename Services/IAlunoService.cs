using BibliotecaAPI.DTOs;

namespace BibliotecaAPI.Services;

public interface IAlunoService
{
    Task<AlunoResponseDto> CriarAsync(
        CriarAlunoDto dto);

    Task<List<AlunoResponseDto>> ListarAsync();

    Task<AlunoResponseDto> BuscarPorIdAsync(int id);
}
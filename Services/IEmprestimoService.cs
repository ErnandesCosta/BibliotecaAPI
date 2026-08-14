using BibliotecaAPI.DTOs;

namespace BibliotecaAPI.Services;

public interface IEmprestimoService
{
    Task<EmprestimoResponseDto> CriarAsync(
        CriarEmprestimoDto dto);

    Task<List<EmprestimoResponseDto>> ListarAsync();

    Task<EmprestimoResponseDto> BuscarPorIdAsync(
        int id);

    Task<EmprestimoResponseDto> DevolverAsync(
        int id);
}
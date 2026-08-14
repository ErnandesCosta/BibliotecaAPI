using BibliotecaAPI.Models;

namespace BibliotecaAPI.DTOs;

public class EmprestimoResponseDto
{
    public int Id { get; set; }

    public int AlunoId { get; set; }

    public string? NomeAluno { get; set; }

    public int LivroId { get; set; }

    public string? TituloLivro { get; set; }

    public DateTime DataEmprestimo { get; set; }

    public DateTime DataPrevistaDevolucao { get; set; }

    public DateTime? DataDevolucao { get; set; }

    public StatusEmprestimo Status { get; set; }
}
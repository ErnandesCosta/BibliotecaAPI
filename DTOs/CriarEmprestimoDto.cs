using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs;

public class CriarEmprestimoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "O AlunoId deve ser informado.")]
    public int AlunoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O LivroId deve ser informado.")]
    public int LivroId { get; set; }

    [Required(ErrorMessage = "A data prevista de devolução é obrigatória.")]
    public DateTime DataPrevistaDevolucao { get; set; }
}
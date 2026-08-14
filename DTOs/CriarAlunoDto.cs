using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs;

public class CriarAlunoDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A matrícula é obrigatória.")]
    [MaxLength(30, ErrorMessage = "A matrícula deve ter no máximo 30 caracteres.")]
    public string Matricula { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [MaxLength(150, ErrorMessage = "O e-mail deve ter no máximo 150 caracteres.")]
    public string Email { get; set; } = string.Empty;
}
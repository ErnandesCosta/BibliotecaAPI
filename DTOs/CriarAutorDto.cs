using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs;

public class CriarAutorDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    [MaxLength(100, ErrorMessage = "A nacionalidade deve ter no máximo 100 caracteres.")]
    public string? Nacionalidade { get; set; }
}
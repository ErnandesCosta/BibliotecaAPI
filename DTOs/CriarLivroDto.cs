using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs;

public class CriarLivroDto
{
    [Required(ErrorMessage = "O ISBN é obrigatório.")]
    [MaxLength(20, ErrorMessage = "O ISBN deve ter no máximo 20 caracteres.")]
    public string ISBN { get; set; } = string.Empty;

    [Required(ErrorMessage = "O título é obrigatório.")]
    [MaxLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Range(0, 3000, ErrorMessage = "O ano de publicação deve ser válido.")]
    public int AnoPublicacao { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa.")]
    public int Quantidade { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O AutorId deve ser informado.")]
    public int AutorId { get; set; }
}
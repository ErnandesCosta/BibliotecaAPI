using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;

public class Aluno
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Matricula { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    public ICollection<Emprestimo> Emprestimos { get; set; } =
        new List<Emprestimo>();
}
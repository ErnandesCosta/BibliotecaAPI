using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;

public class Livro
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    public int AnoPublicacao { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantidade { get; set; }

    public int AutorId { get; set; }

    public Autor? Autor { get; set; }

    public ICollection<Emprestimo> Emprestimos { get; set; } =
        new List<Emprestimo>();
}
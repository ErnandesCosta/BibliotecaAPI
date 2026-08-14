using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;

public class Autor
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    [MaxLength(100)]
    public string? Nacionalidade { get; set; }

    public ICollection<Livro> Livros { get; set; } = new List<Livro>();
}
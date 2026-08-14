using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;

public class Emprestimo
{
    public int Id { get; set; }

    public int AlunoId { get; set; }
    public Aluno? Aluno { get; set; }

    public int LivroId { get; set; }
    public Livro? Livro { get; set; }

    public DateTime DataEmprestimo { get; set; }

    public DateTime DataPrevistaDevolucao { get; set; }

    public DateTime? DataDevolucao { get; set; }

    public StatusEmprestimo Status { get; set; }
}
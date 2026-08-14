using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Data;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
        : base(options)
    {
    }

    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<Livro> Livros => Set<Livro>();
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Emprestimo> Emprestimos => Set<Emprestimo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Aluno>()
            .HasIndex(aluno => aluno.Matricula)
            .IsUnique();

        modelBuilder.Entity<Livro>()
            .HasIndex(livro => livro.ISBN)
            .IsUnique();

        modelBuilder.Entity<Autor>()
            .HasMany(autor => autor.Livros)
            .WithOne(livro => livro.Autor)
            .HasForeignKey(livro => livro.AutorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Emprestimo>()
            .HasOne(emprestimo => emprestimo.Aluno)
            .WithMany(aluno => aluno.Emprestimos)
            .HasForeignKey(emprestimo => emprestimo.AlunoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Emprestimo>()
            .HasOne(emprestimo => emprestimo.Livro)
            .WithMany(livro => livro.Emprestimos)
            .HasForeignKey(emprestimo => emprestimo.LivroId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
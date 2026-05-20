using Microsoft.EntityFrameworkCore;
using CP3ConcessionariaAPI.Models;

namespace CP3ConcessionariaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Concessionaria> Concessionarias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<PessoaFisica> PessoasFisicas { get; set; }
        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Financiamento> Financiamentos { get; set; }
        public DbSet<Contratacao> Contratacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Herança Cliente → discriminator
            modelBuilder.Entity<Cliente>()
                .HasDiscriminator<string>("TIPO_CLIENTE")
                .HasValue<PessoaFisica>("PF")
                .HasValue<PessoaJuridica>("PJ");

            // Herança Produto → discriminator
            modelBuilder.Entity<Produto>()
                .HasDiscriminator<string>("TIPO_PRODUTO")
                .HasValue<Financiamento>("FINANCIAMENTO");

            // Concessionaria → Cliente (1 para N)
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Concessionaria)
                .WithMany()
                .HasForeignKey(c => c.IdConcessionaria)
                .OnDelete(DeleteBehavior.Restrict);

            // Cliente → Contratacao (1 para N)
            modelBuilder.Entity<Contratacao>()
                .HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            // Produto → Contratacao (1 para N)
            modelBuilder.Entity<Contratacao>()
                .HasOne(c => c.Produto)
                .WithMany()
                .HasForeignKey(c => c.IdProduto)
                .OnDelete(DeleteBehavior.Restrict);

            // Precisão dos campos decimais do Financiamento
            modelBuilder.Entity<Financiamento>()
                .Property(e => e.ValorVeiculo)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Financiamento>()
                .Property(e => e.ValorEntrada)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Financiamento>()
                .Property(e => e.TaxaJuros)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Financiamento>()
                .Property(e => e.ValorParcela)
                .HasPrecision(18, 2);
        }
    }
}
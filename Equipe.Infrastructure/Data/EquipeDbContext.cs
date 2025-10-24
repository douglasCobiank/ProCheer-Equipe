using Equipe.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Equipe.Infrastructure.Data
{
    public class EquipeDbContext(DbContextOptions<EquipeDbContext> options) : DbContext(options)
    {
        public DbSet<EquipeData> Equipe { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔹 EQUIPE
            modelBuilder.Entity<EquipeData>(entity =>
            {
                entity.ToTable("Equipe");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            // 🔹 EQUIPEATLETA (tabela de relacionamento)
            modelBuilder.Entity<EquipeAtleta>(entity =>
            {
                entity.ToTable("EquipeAtleta");

                entity.HasKey(ea => new { ea.IdEquipe, ea.IdAtleta });

                entity.HasOne(ea => ea.Equipe)
                    .WithMany(e => e.EquipeAtletas)
                    .HasForeignKey(ea => ea.IdEquipe);

                // Se Atleta está fora do contexto / excluído da migração:
                entity.HasOne(ea => ea.Atleta)
                    .WithMany(a => a.EquipeAtletas)
                    .HasForeignKey(ea => ea.IdAtleta)
                    .IsRequired(false); // evita erro se a tabela não for gerenciada pelo EF
            });

            // 🔹 OUTRAS ENTIDADES
            modelBuilder.Entity<UsuarioData>().ToTable("Usuario");
            modelBuilder.Entity<GinasioData>().ToTable("Ginasio");
            modelBuilder.Entity<EnderecoData>().ToTable("Endereco");
            modelBuilder.Entity<DocumentoData>().ToTable("Documento");
            modelBuilder.Entity<AtletaData>().ToTable("Atleta");

            // 🔹 EXCLUIR TABELAS DE OUTROS CONTEXTOS
            modelBuilder.Entity<UsuarioData>().Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<GinasioData>().Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<EnderecoData>().Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<DocumentoData>().Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<AtletaData>().Metadata.SetIsTableExcludedFromMigrations(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
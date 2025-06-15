using FrotaVisionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        // tabelas como DbSet
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Motorista> Motoristas { get; set; }
        public DbSet<Viagem> Viagens { get; set; }
        public DbSet<ManutencaoRealizada> ManutencaoRealizadas { get; set; }
        public DbSet<Manutencao> Manutencoes { get; set; }
        public DbSet<TipoCaminhao> TiposCaminhoes { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }

        public DbSet<Models.Plano> Planos { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("usuario");
            modelBuilder.Entity<Empresa>().ToTable("empresa");
            modelBuilder.Entity<Motorista>().ToTable("motorista");
            modelBuilder.Entity<Veiculo>().ToTable("veiculo");
            modelBuilder.Entity<Viagem>().ToTable("viagem");
            modelBuilder.Entity<ManutencaoRealizada>().ToTable("manutencao_realizada");
            modelBuilder.Entity<Manutencao>().ToTable("manutencao");
            modelBuilder.Entity<TipoCaminhao>().ToTable("tipo_caminhao");
            modelBuilder.Entity<Permissao>().ToTable("permissoes_usuario");
            modelBuilder.Entity<Models.Plano>().ToTable("plano");
            base.OnModelCreating(modelBuilder);
        }
    }
}

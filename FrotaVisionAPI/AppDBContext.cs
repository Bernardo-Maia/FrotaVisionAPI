using FrotaVisionAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FrotaVisionAPI
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        // Adicione aqui suas tabelas como DbSet
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Motorista> Motoristas { get; set; }
        public DbSet<Viagem> Viagens { get; set; }
        public DbSet<ManutencaoRealizada> ManutencaoRealizadas { get; set; }
        public DbSet<Manutencao> Manutencoes{ get; set; }
        public DbSet<TipoCaminhao> TiposCaminhoes { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseNpgsql("Host=db.apimpqpegxjlygictble.supabase.co;Database=postgres;Username=postgres;Password=frotavisionPIT;SSL Mode=Require;Trust Server Certificate=true");
        //    }
        //}

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
            base.OnModelCreating(modelBuilder);
        }
    }
}

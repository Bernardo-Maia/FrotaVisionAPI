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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "User Id=postgres.apimpqpegxjlygictble;Password=riCaS2kVXbLOderQ;Server=aws-0-sa-east-1.pooler.supabase.com;Port=6543;Database=postgres";
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("usuario");
            modelBuilder.Entity<Empresa>().ToTable("empresa");
            modelBuilder.Entity<Motorista>().ToTable("motorista");
            modelBuilder.Entity<Veiculo>().ToTable("veiculo");
            modelBuilder.Entity<Viagem>().ToTable("viagem");
            modelBuilder.Entity<Viagem>().ToTable("manutencao_realizada");
            base.OnModelCreating(modelBuilder);
        }
    }
}

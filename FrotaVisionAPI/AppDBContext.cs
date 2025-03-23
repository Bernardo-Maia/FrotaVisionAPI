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
            base.OnModelCreating(modelBuilder);
        }
    }
}

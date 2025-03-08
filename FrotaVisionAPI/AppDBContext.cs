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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

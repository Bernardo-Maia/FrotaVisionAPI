using FrotaVisionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Context
{
    public class ViagemContext : DbContext
    {
        public ViagemContext(DbContextOptions<ViagemContext> options)
            : base(options)
        {
        }
        public DbSet<Viagem> Viagens { get; set; } = null!;
    }
    
}


using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Models
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


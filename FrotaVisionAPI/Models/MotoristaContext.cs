using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Models
{
    public class MotoristaContext : DbContext
    {
        public MotoristaContext(DbContextOptions<MotoristaContext> options)
            : base(options)
        {
        }
        public DbSet<Motorista> Motoristas { get; set; } = null!;
    }
    
}


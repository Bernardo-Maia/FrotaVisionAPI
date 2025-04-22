using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Models
{
    public class ManutencaoContext : DbContext
    {
        public ManutencaoContext(DbContextOptions<ManutencaoContext> options)
            : base(options)
        {
        }
        public DbSet<Manutencao> Manutencao{ get; set; } = null!;
    }
    
}


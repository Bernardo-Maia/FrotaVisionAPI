using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Models
{
    public class ManutencaoRealizadaContext : DbContext
    {
        public ManutencaoRealizadaContext(DbContextOptions<ManutencaoRealizadaContext> options)
            : base(options)
        {
        }
        public DbSet<ManutencaoRealizada> ManutencoesRealizadas { get; set; } = null!;
    }
    
}


using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Models
{
    public class VeiculoContext : DbContext
    {
        public VeiculoContext(DbContextOptions<VeiculoContext> options)
            : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; } = null!;
    }
    
}


using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Models
{
    public class tipoCaminhaoContext : DbContext
    {
         
        public tipoCaminhaoContext(DbContextOptions<tipoCaminhaoContext> options)
            : base(options)
        {
        }
        public DbSet<TipoCaminhao> Tipos { get; set; } = null!;
    }

}

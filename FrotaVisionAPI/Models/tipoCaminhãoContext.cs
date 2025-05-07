using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Models
{
    public class tipoCaminhãoContext : DbContext
    {
         
        public tipoCaminhãoContext(DbContextOptions<tipoCaminhãoContext> options)
            : base(options)
        {
        }
        public DbSet<TipoCaminhão> Tipos { get; set; } = null!;
    }

}

using FrotaVisionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Context
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


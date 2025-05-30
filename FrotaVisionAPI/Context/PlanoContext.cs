using FrotaVisionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Context
{
    public class PlanoContext : DbContext
    {
        public PlanoContext(DbContextOptions<PlanoContext> options)
           : base(options)
        {
        }
        public DbSet<Plano> planos { get; set; } = null!;
    }
}

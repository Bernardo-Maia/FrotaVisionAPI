using FrotaVisionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Context
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

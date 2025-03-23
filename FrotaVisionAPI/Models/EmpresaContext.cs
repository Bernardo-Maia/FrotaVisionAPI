using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Models
{
    public class EmpresaContext : DbContext
    {
        public EmpresaContext(DbContextOptions<UsuarioContext> options)
            : base(options)
        {
        }
        public DbSet<Empresa> Empresa { get; set; } = null!;
    }
    
}


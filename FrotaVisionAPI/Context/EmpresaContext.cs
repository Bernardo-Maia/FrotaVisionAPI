using FrotaVisionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Context
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


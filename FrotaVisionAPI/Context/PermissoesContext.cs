using FrotaVisionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Context
{
    public class PermissoesContext :  DbContext
    {
        
         
        public PermissoesContext(DbContextOptions<PermissoesContext> options)
            : base(options)
        {
        }
        public DbSet<Permissao> Permissoes { get; set; } = null!;
    }


}

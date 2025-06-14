using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PermissoesController : ControllerBase
    {
        private readonly AppDBContext _context;

        public PermissoesController(AppDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<Permissao>>> GetPermissoes()
        {
            return await _context.Permissoes.Where(x => x.habilitado == true).ToListAsync();
        }
        [HttpGet("Pesquisar/{id}")]
        public async Task<ActionResult<Permissao>> GetPermissao(int id)
        {
            Permissao? permissao = await _context.Permissoes.FindAsync(id);
            if (permissao == null)
                return NotFound(new { message = "Permissão não encontrada" });
            return permissao;
        }
    }
}

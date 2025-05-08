using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Controllers
{
    public class TipoCaminhaoController : ControllerBase
    {
        private readonly AppDBContext _context;

        public TipoCaminhaoController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<TipoCaminhao>>> GetTipoCaminhao()
        {
            var tipos = await _context.TiposCaminhoes.ToListAsync();
            if (tipos == null)
                return NotFound(new { message = "Nenhum tipo de caminhão encontrado" });
            return Ok(tipos);
        }

    }
}

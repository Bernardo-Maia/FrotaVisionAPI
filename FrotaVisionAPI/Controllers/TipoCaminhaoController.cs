using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            List<TipoCaminhao> tipos = await _context.TiposCaminhoes.ToListAsync();
            return Ok(tipos);
        }

    }
}

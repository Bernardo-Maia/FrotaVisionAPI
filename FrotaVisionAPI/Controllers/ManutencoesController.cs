using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManutencoesController
    {
        private readonly AppDBContext _context;
        public ManutencoesController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<Manutencao>>> GetManutencao()
        {
            return await _context.Manutencoes.Where(x => x.habilitado == true).ToListAsync();
        }

    }
}

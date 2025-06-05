using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PlanoController : ControllerBase
    {
        private readonly AppDBContext _context;

        public PlanoController(AppDBContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<Plano>>> GetPlano()
        {
            List<Plano> planos = await _context.Planos.ToListAsync();
            return Ok(planos);
        }

        [HttpGet]
        [Route("Pesquisar/{id}")]
        public async Task<ActionResult<Plano>> Getplano(int id)
        {
            Plano? plano = await _context.Planos.FindAsync(id);
            if (plano == null)
                return NotFound(new { message = "Plano não encontrado" });

            return plano;
        }



    }
}

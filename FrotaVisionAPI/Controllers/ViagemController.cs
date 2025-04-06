using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FrotaVisionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViagemController : ControllerBase
    {
        private readonly AppDBContext _context;
        public ViagemController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<Viagem>>> GetViagem()
        {
            return await _context.Viagens.ToListAsync();
        }

        [HttpGet("Pesquisar/{id}")]
        public async Task<ActionResult<Viagem>> GetViagem(int id)
        {
            var viagem = await _context.Viagens.FindAsync(id);
            if (viagem == null)
                return NotFound(new { message = "Viagem não encontrada" });

            return viagem;
        }


        [HttpPost("Cadastrar")]
        public async Task<ActionResult<Viagem>> PostViagem(Viagem viagem)
        {

            _context.Viagens.Add(viagem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetViagem), new { viagem.id_viagem }, viagem);
        }

        [HttpPut("Atualizar/{id}")]
        public async Task<IActionResult> PutViagem(int id, Viagem viagem)
        {
            if (id != viagem.id_viagem)
                return BadRequest(new { message = "viagem não corresponde ao informado" });

            _context.Entry(viagem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Viagens.Any(e => e.id_viagem == id))
                    return NotFound(new { message = "Viagem não encontrada" });

                throw;
            }

            return Ok(new { message = "Viagem atualizada com sucesso", viagem = viagem });
        }

        [HttpDelete("Deletar/{id}")]
        [SwaggerOperation(Summary = "Remove uma viagem", Description = "Deleta uma viagem do banco de dados.")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var viagem = await _context.Viagens.FindAsync(id);
            if (viagem == null)
                return NotFound(new { message = "viagem não encontrada" });

            _context.Viagens.Remove(viagem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "viagem deletada com sucesso" });
        }


    }
}

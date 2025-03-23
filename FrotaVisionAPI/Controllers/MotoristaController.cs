using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FrotaVisionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotoristaController : ControllerBase
    {
        private readonly AppDBContext _context;
        public MotoristaController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<Motorista>>> GetMotorista()
        {
            return await _context.Motoristas.ToListAsync();
        }

        [HttpGet("Pesquisar/{ID}")]
        public async Task<ActionResult<Motorista>> GetMotorista(int ID)
        {
            var motorista = await _context.Motoristas.FindAsync(ID);
            if (motorista == null)
                return NotFound(new { message = "Motorista não encontrado" });

            return motorista;
        }


        [HttpPost("Cadastrar")]
        public async Task<ActionResult<Motorista>> PostMotorista(Motorista motorista)
        {

            _context.Motoristas.Add(motorista);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMotorista), new { motorista.id_motorista }, motorista);
        }

        [HttpPut("Atualizar/{ID}")]
        public async Task<IActionResult> PutMotorista(int ID, Motorista motorista)
        {
            if (ID != motorista.id_motorista)
                return BadRequest(new { message = "id do motorista não corresponde ao informado" });

            _context.Entry(motorista).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Motoristas.Any(e => e.id_motorista  == ID))
                    return NotFound(new { message = "Motorista não encontrado" });

                throw;
            }

            return Ok(new { message = "Motorista atualizado com sucesso", motorista = motorista });
        }


        [HttpDelete("Deletar/{id}")]
        [SwaggerOperation(Summary = "Remove um motorista", Description = "Deleta um motorista do banco de dados.")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var motorista = await _context.Motoristas.FindAsync(id);
            if (motorista == null)
                return NotFound(new { message = "Motorista não encontrado" });

            _context.Motoristas.Remove(motorista);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Motorista deletado com sucesso" });
        }


    }
}

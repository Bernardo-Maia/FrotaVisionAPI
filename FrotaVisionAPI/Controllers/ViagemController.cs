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
        [Route("Listar/{cnpj}")]
        public async Task<ActionResult<IEnumerable<Viagem>>> GetViagem(string cnpj)
        {
            return await _context.Viagens.Where(x => x.cnpj == cnpj).ToListAsync();
        }

        [HttpGet]
        [Route("ListarDetalhado")]
        public async Task<ActionResult<IEnumerable<object>>> GetViagemDetalhada()
        {
            var viagens = await (
            from v in _context.Viagens
            join veic in _context.Veiculos on v.id_veiculo equals veic.id_veiculo
            join mot in _context.Motoristas on v.id_motorista equals mot.id_motorista
            select new
            {
                v.id_viagem,
                v.data_inicio,
                v.data_fim,
                v.quilometragem_viagem,
                nome_motorista = mot.nome,
                apelido_veiculo = veic.apelido,
                placa_veiculo = veic.placa,
                v.origem,
                v.destino,
                v.descricao

            }).ToListAsync();

            return Ok(viagens);
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
            
            var veiculo = await _context.Veiculos.FindAsync(viagem.id_veiculo);
            if (veiculo == null)
                return NotFound("Veículo não encontrado.");

            veiculo.quilometragem += viagem.quilometragem_viagem;

            _context.Viagens.Add(viagem);
            _context.Veiculos.Update(veiculo);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetViagem), new { id = viagem.id_viagem }, viagem);
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
        public async Task<IActionResult> DeleteViagem(int id)
        {
            var viagem = await _context.Viagens.FindAsync(id);
            if (viagem == null)
                return NotFound(new { message = "Viagem não encontrada" });

            viagem.habilitado = false;
            _context.Entry(viagem).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Viagem desativada com sucesso" });
        }


    }
}

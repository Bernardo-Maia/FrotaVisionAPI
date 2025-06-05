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
        [Route("Listar/{cnpj}")]
        public async Task<ActionResult<IEnumerable<Motorista>>> GetMotorista(string cnpj)
        {
            return await _context.Motoristas.Where(x => x.cnpj == cnpj && x.habilitado == true).ToListAsync();
        }

        [HttpGet]
        [Route("ListarDetalhado/{cnpj}")]
        public async Task<ActionResult<IEnumerable<object>>> GetMotoristaDetalhado(string cnpj)
        {

            try
            {

                return Ok(await (
                    from m in _context.Motoristas
                    where m.habilitado == true && m.cnpj == cnpj

                    // Viagem mais recente de cada motorista
                    let ultimaViagem = (
                        from v in _context.Viagens
                        where v.id_motorista == m.id_motorista
                        orderby v.data_fim descending
                        select v
                    ).FirstOrDefault()

                    join ve in _context.Veiculos on ultimaViagem.id_veiculo equals ve.id_veiculo into veiculoJoin
                    from veiculo in veiculoJoin.DefaultIfEmpty()

                    select new
                    {
                        m.id_motorista,
                        m.nome,
                        data_ultima_viagem = ultimaViagem.data_fim,
                        veiculo.placa,
                        veiculo.apelido
                    }
                ).ToListAsync());
            }
            catch
            {
                return BadRequest(new { message = "Erro ao listar motoristas" });
            }
        }

        [HttpGet("Pesquisar/{ID}")]
        public async Task<ActionResult<Motorista>> GetMotorista(int ID)
        {
            Motorista? motorista = await _context.Motoristas.FindAsync(ID);
            if (motorista == null)
                return NotFound(new { message = "Motorista não encontrado" });

            return motorista;
        }


        [HttpPost("Cadastrar")]
        public async Task<ActionResult<Motorista>> PostMotorista(Motorista motorista)
        {

            _context.Motoristas.Add(motorista);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostMotorista), new { motorista.id_motorista }, motorista);
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
        public async Task<IActionResult> DeleMotorista(int id)
        {
            Motorista? motorista = await _context.Motoristas.FindAsync(id);
            if (motorista == null)
                return NotFound(new { message = "Motorista não encontrado" });

            motorista.habilitado = false;
            _context.Entry(motorista).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Motorista desativado com sucesso" });
        }


    }
}

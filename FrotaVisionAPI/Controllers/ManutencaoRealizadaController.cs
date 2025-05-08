using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FrotaVisionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManutencaoRealizadaController : ControllerBase
    {
        private readonly AppDBContext _context;
        public ManutencaoRealizadaController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<ManutencaoRealizada>>> GetManutencaoRealizada()
        {
            return await _context.ManutencaoRealizadas.ToListAsync();
        }

        [HttpGet]
        [Route("ListarDetalhado")]
        public async Task<ActionResult<IEnumerable<object>>> GetManutencaoRealizadaDetalhada()
        {
            var manutencaoRealizada = await (
            from mr in _context.ManutencaoRealizadas
            join m in _context.Manutencoes on mr.id_manutencao equals m.id_manutencao
            join v in _context.Veiculos on mr.id_veiculo equals v.id_veiculo
            where mr.habilitado == true
            select new
            {
                mr.id_manutencao_realizada,
                mr.data_manutencao,
                mr.quilometragem_ultima_manutencao,
                quilometragemRecomendada = mr.quilometragem_ultima_manutencao + m.quilometragem_preventiva,
                descricaoRealizada = mr.descricao,
                mr.data_cadastro,
                mr.valor_manutencao,
                mr.horasMotorManutencao,
                mr.eManuntencaoPreventiva,
                v.apelido,
                v.placa,
                v.quilometragem,
                descricaoManutencao = m.descricao,
                m.nome

            }).ToListAsync();
            return Ok(manutencaoRealizada);
        }

        [HttpGet("Pesquisar/{ID}")]
        public async Task<ActionResult<ManutencaoRealizada>> GetManutencaoRealizada(int ID)
        {
            var manutencaoRealizada = await _context.ManutencaoRealizadas.FindAsync(ID);
            if (manutencaoRealizada == null)
                return NotFound(new { message = "ManutencaoRealizada não encontrado" });

            return manutencaoRealizada;
        }

        [HttpGet("PesquisarPorVeiculo/{IDVeiculo}")]
        public async Task<ActionResult<IEnumerable<ManutencaoRealizada>>> GetManutencaoRealizadaVeiculo(int IDVeiculo)
        {
            List<ManutencaoRealizada> manutencaoRealizada = await _context.ManutencaoRealizadas.Where(x => x.id_veiculo == IDVeiculo).ToListAsync();

            if (manutencaoRealizada == null)
                return NotFound(new { message = "Nenhuma manutenção para o veiculo" });

            return manutencaoRealizada;
        }


        [HttpPost("Cadastrar")]
        public async Task<ActionResult<ManutencaoRealizada>> PostManutencaoRealizada(ManutencaoRealizada manutencaoRealizada)
        {

            _context.ManutencaoRealizadas.Add(manutencaoRealizada);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetManutencaoRealizada), new { manutencaoRealizada.id_manutencao_realizada }, manutencaoRealizada);
        }

        [HttpPut("Atualizar/{ID}")]
        public async Task<IActionResult> PutManutencaoRealizada(int ID, ManutencaoRealizada manutencaoRealizada)
        {
            if (ID != manutencaoRealizada.id_manutencao_realizada)
                return BadRequest(new { message = "id do manutencaoRealizada não corresponde ao informado" });

            _context.Entry(manutencaoRealizada).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ManutencaoRealizadas.Any(e => e.id_manutencao_realizada  == ID))
                    return NotFound(new { message = "ManutencaoRealizada não encontrado" });

                throw;
            }

            return Ok(new { message = "ManutencaoRealizada atualizado com sucesso", manutencaoRealizada = manutencaoRealizada });
        }


        [HttpDelete("Deletar/{id}")]
        [SwaggerOperation(Summary = "Remove um manutencaoRealizada", Description = "Deleta um manutencaoRealizada do banco de dados.")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var manutencaoRealizada = await _context.ManutencaoRealizadas.FindAsync(id);
            if (manutencaoRealizada == null)
                return NotFound(new { message = "ManutencaoRealizada não encontrado" });

            _context.ManutencaoRealizadas.Remove(manutencaoRealizada);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ManutencaoRealizada deletado com sucesso" });
        }


    }
}

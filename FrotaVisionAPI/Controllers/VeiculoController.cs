using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FrotaVisionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController : ControllerBase
    {
        private readonly AppDBContext _context;
        public VeiculoController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculo()
        {
            return await _context.Veiculos.ToListAsync();
        }

        [HttpGet]
        [Route("ListarDetalhado")]
        public async Task<ActionResult<IEnumerable<object>>> GetVeiculoDetalhado()
        {
            var veiculos = await (
            from v in _context.Veiculos
            join t in _context.TiposCaminhoes on v.tipo equals t.id
            where v.habilitado == true
            select new
            {
                v.id_veiculo,
                v.apelido,
                v.placa,
                v.quilometragem,
                v.horas_motor,
                v.ano,
                v.descricao,
                tipo = t.nome,
            }).ToListAsync();
            return Ok(veiculos);
        }

        [HttpGet("Pesquisar/{id}")]
        public async Task<ActionResult<Veiculo>> GetVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound(new { message = "Veiculo não encontrado" });

            return veiculo;
        }


        [HttpPost("Cadastrar")]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {

            _context.Veiculos.Add(veiculo);
           

            // Busca todas as manutenções habilitadas
            var manutencoes = await _context.Manutencoes
                .Where(m => m.habilitado)
                .ToListAsync();

            // Cria uma ManutencaoRealizada para cada manutenção
            foreach (var manutencao in manutencoes)
            {
                var manutencaoRealizada = new ManutencaoRealizada
                {
                    id_veiculo = veiculo.id_veiculo,
                    id_manutencao = manutencao.id_manutencao,
                    data_manutencao = DateTime.UtcNow,
                    quilometragem_ultima_manutencao = 0,
                    descricao = manutencao.descricao,
                    data_cadastro = DateTime.UtcNow,
                    valor_manutencao = 0, // valor inicial
                    horasMotorManutencao = 0, // valor inicial
                    eManuntencaoPreventiva = true,
                    habilitado = true
                };

                _context.ManutencaoRealizadas.Add(manutencaoRealizada);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVeiculo), new { veiculo.id_veiculo }, veiculo);
        }

        [HttpPut("Atualizar/{id}")]
        public async Task<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            if (id != veiculo.id_veiculo)
                return BadRequest(new { message = "Placa do veiculo não corresponde ao informado" });

            _context.Entry(veiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Veiculos.Any(e => e.id_veiculo == id))
                    return NotFound(new { message = "Veiculo não encontrado" });

                throw;
            }

            return Ok(new { message = "Veiculo atualizado com sucesso", veiculo = veiculo });
        }

        [HttpDelete("Deletar/{id}")]
        [SwaggerOperation(Summary = "Remove um veiculo", Description = "Deleta um veiculo do banco de dados.")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound(new { message = "veiculo não encontrado" });

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "veiculo deletado com sucesso" });
        }


    }
}

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
        [Route("Listar/{cnpj}")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculo(string cnpj)
        {
            return await _context.Veiculos.Where(x => x.cnpj == cnpj).ToListAsync();
        }

        [HttpGet]
        [Route("ListarDetalhado/{cnpj}")]
        public async Task<ActionResult<IEnumerable<object>>> GetVeiculoDetalhado(string cnpj)
        {


            var veiculos = await (
            from v in _context.Veiculos
            join t in _context.TiposCaminhoes on v.tipo equals t.id
            where v.habilitado == true && v.cnpj == cnpj
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

        [HttpGet("PesquisarDetalhado/{id}")]
        public async Task<ActionResult<IEnumerable<object>>> GetVeiculoDetalhado(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            var cnpj = veiculo.cnpj;

            if (cnpj == null)
                return NotFound(new { message = "Veiculo não encontrado" });

            var countManutencao = await _context.ManutencaoRealizadas.CountAsync(mr => mr.habilitado == true && mr.id_veiculo == id);

            var ultimaManutencao = await _context.ManutencaoRealizadas
                .Where(m => m.id_veiculo == id && m.habilitado == true)
                .OrderByDescending(m => m.data_manutencao).FirstOrDefaultAsync();

            var nomeUltimaManutencao = await _context.Manutencoes
                .Where(m => m.id_manutencao == ultimaManutencao.id_manutencao)
                .Select(m => m.nome)
                .FirstOrDefaultAsync();


            var notificacaoController = new NotificacaoController(_context);

            var notificacoes = await notificacaoController.GerarNotificacoes(cnpj);

            var ultimaNotificacao = notificacoes.Value.OrderBy(x => x.quilometragemManutencao).FirstOrDefault(x => x.id_veiculo == id);

            var ultimaManutencaoUrgenteNome = await _context.Manutencoes
                .Where(m => m.id_manutencao == ultimaNotificacao.id_manutencao)
                .Select(m => m.nome)
                .FirstOrDefaultAsync();

            var ultimaManutencaoUrgenteData = await _context.ManutencaoRealizadas
                .Where(m => m.id_manutencao_realizada == ultimaNotificacao.idManutencaoRealizada && m.habilitado == true)
                .OrderByDescending(m => m.data_manutencao)
                .Select(m => m.data_manutencao)
                .FirstOrDefaultAsync();

            var notificacoescount = notificacoes.Value.Where(x => x.id_veiculo == id);

            var countViagens = await _context.Viagens.CountAsync(v => v.id_veiculo == id && v.habilitado == true);
            var NomeUltimoMotorista = "Sem viagens registradas";

            var ultimaViagemData = "Sem viagens registradas";
            if (countViagens != 0) {

                var ultimaViagem = await _context.Viagens
                    .Where(v => v.id_veiculo == id && v.habilitado == true)
                    .OrderByDescending(v => v.data_fim)
                    .FirstOrDefaultAsync();

                ultimaViagemData = await _context.Viagens
                .Where(v => v.id_veiculo == id && v.habilitado == true)
                .OrderByDescending(v => v.data_fim)
                .Select(x => x.data_fim.ToString("dd/MM/yyyy"))
                .FirstOrDefaultAsync();

                NomeUltimoMotorista = await _context.Motoristas
                .Where(m => m.id_motorista == ultimaViagem.id_motorista)
                .Select(m => m.nome)
                .FirstOrDefaultAsync();

                 
            }
           

                var resposta = new
                {
                    cnpj = cnpj,
                    veiculo = veiculo,
                    countManutencao = countManutencao,
                    countViagens = countViagens,
                    countUrgente = notificacoescount.Count(),
                    nomeUltimaManitencao = nomeUltimaManutencao,
                    dataUltimaManutecao = ultimaManutencao.data_manutencao.ToString("dd/MM/yyyy"),
                    ultimaManutencaoUrgenteNome = ultimaManutencaoUrgenteNome,
                    ultimaMantuenacaoUrgenteData = ultimaManutencaoUrgenteData.ToString("dd/MM/yyyy"),
                    ultimaViagemData = ultimaViagemData,
                    ultimaViagemMotorista = NomeUltimoMotorista,


                };



            return Ok(resposta);
            
        }


        [HttpPost("Cadastrar")]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {

            _context.Veiculos.Add(veiculo);

            await _context.SaveChangesAsync();
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
                    habilitado = true,
                    cnpj = veiculo.cnpj
                };

                _context.ManutencaoRealizadas.Add(manutencaoRealizada);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostVeiculo), new { veiculo.id_veiculo }, veiculo);
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
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound(new { message = "Veículo não encontrado" });

            veiculo.habilitado = false;
            _context.Entry(veiculo).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Veículo desativado com sucesso" });
        }


    }
}

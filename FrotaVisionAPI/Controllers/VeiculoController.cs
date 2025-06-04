using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.LibraryModel;
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


            return Ok(await (
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
            }).ToListAsync());
        }

        [HttpGet("Pesquisar/{id}")]
        public async Task<ActionResult<Veiculo>> GetVeiculo(int id)
        {
            Veiculo? veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound(new { message = "Veiculo não encontrado" });

            return veiculo;
        }

        [HttpGet("PesquisarDetalhado/{id}")]
        public async Task<ActionResult<IEnumerable<object>>> GetVeiculoDetalhado(int id)
        {
            Veiculo? veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound(new { message = "Veiculo não encontrado" });

            string cnpj = veiculo.cnpj;


            int countManutencao = await _context.ManutencaoRealizadas.CountAsync(mr => mr.habilitado == true && mr.id_veiculo == id);

            ManutencaoRealizada? ultimaManutencao = await _context.ManutencaoRealizadas
                .Where(m => m.id_veiculo == id && m.habilitado == true)
                .OrderByDescending(m => m.data_manutencao).FirstOrDefaultAsync();

            string? nomeUltimaManutencao = await _context.Manutencoes
                .Where(m => m.id_manutencao == ultimaManutencao.id_manutencao)
                .Select(m => m.nome)
                .FirstOrDefaultAsync();


            NotificacaoController notificacaoController = new NotificacaoController(_context);

            ActionResult<IEnumerable<Notificacao>> notificacoes = await notificacaoController.GerarNotificacoes(cnpj);

            Notificacao? ultimaNotificacao = notificacoes.Value.OrderBy(x => x.quilometragemManutencao).FirstOrDefault(x => x.id_veiculo == id);

            string? ultimaManutencaoUrgenteNome = await _context.Manutencoes
                .Where(m => m.id_manutencao == ultimaNotificacao.id_manutencao)
                .Select(m => m.nome)
                .FirstOrDefaultAsync();

            DateTime ultimaManutencaoUrgenteData = await _context.ManutencaoRealizadas
                .Where(m => m.id_manutencao_realizada == ultimaNotificacao.idManutencaoRealizada && m.habilitado == true)
                .OrderByDescending(m => m.data_manutencao)
                .Select(m => m.data_manutencao)
                .FirstOrDefaultAsync();

            IEnumerable<Notificacao> notificacoescount = notificacoes.Value.Where(x => x.id_veiculo == id);

            int countViagens = await _context.Viagens.CountAsync(v => v.id_veiculo == id && v.habilitado == true);
            string? NomeUltimoMotorista = "Sem viagens registradas";

            string?  ultimaViagemData = "Sem viagens registradas";
            if (countViagens != 0) {

                Viagem? ultimaViagem = await _context.Viagens
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


            return Ok(new
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


            });
            
        }


        [HttpPost("Cadastrar/{IDUsuario}")]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo, int IDUsuario)
        {

            int? planoEmpresa = _context.Empresas.FindAsync(veiculo.cnpj).Result?.id_plano ?? 0;

            if (planoEmpresa == 0)
                return BadRequest(new { message = "Empresa não encontrada " });

            int? limite = _context.Planos.Where(p => p.id_plano == planoEmpresa).FirstOrDefault()?.qntd_veiculos ?? null;
            int countVeiculos = await _context.Veiculos.CountAsync(v => v.cnpj == veiculo.cnpj && v.habilitado == true);


            if (limite == null || (limite <= countVeiculos && limite != 0))
                return BadRequest(new { message = "Limite de veiculos atingido" });


            _context.Veiculos.Add(veiculo);

            await _context.SaveChangesAsync();
            // Busca todas as manutenções habilitadas
            List<Manutencao> manutencoes = await _context.Manutencoes
                .Where(m => m.habilitado)
                .ToListAsync();

            // Cria uma ManutencaoRealizada para cada manutenção
            foreach (var manutencao in manutencoes)
            {
                ManutencaoRealizada manutencaoRealizada = new ManutencaoRealizada
                {
                    id_veiculo = veiculo.id_veiculo,
                    id_manutencao = manutencao.id_manutencao,
                    data_manutencao = DateTime.UtcNow,
                    quilometragem_ultima_manutencao = 0,
                    descricao = manutencao.descricao,
                    data_cadastro = DateTime.UtcNow,
                    valor_manutencao = 0, 
                    horasMotorManutencao = 0, 
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
            Veiculo? veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound(new { message = "Veículo não encontrado" });

            veiculo.habilitado = false;
            _context.Entry(veiculo).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Veículo desativado com sucesso" });
        }


    }
}

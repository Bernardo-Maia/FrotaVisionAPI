using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FrotaVisionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacaoController : ControllerBase
    {
        private readonly AppDBContext _context;

        public NotificacaoController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Notifacar/{cnpj}")]
        [SwaggerOperation(Summary = "Gera a lista de notificação", Description = "")]

        public async Task<ActionResult<IEnumerable<Notificacao>>> GerarNotificacoes(string cnpj)
        {
            // 1. Pega TODAS as manutenções relacionadas aos veículos do CNPJ
            var baseQuery = await (
                from mr in _context.ManutencaoRealizadas
                join v in _context.Veiculos on mr.id_veiculo equals v.id_veiculo
                join m in _context.Manutencoes on mr.id_manutencao equals m.id_manutencao
                //join vi in _context.Viagens on v.id_veiculo equals vi.id_veiculo
                where v.cnpj == cnpj
                && v.habilitado == true
                select new
                {
                    mr,
                    v,
                    m,
                    //vi
                }
            ).ToListAsync(); // executa no banco
            var tipos = await _context.TiposCaminhoes.ToListAsync();

            // 2. Agrupa pela combinação de veículo + tipo de manutenção
            var notificacoes = baseQuery
                .GroupBy(x => new { x.v.id_veiculo, x.m.id_manutencao })
                .Select(g =>
                {
                    // Pega a manutenção mais recente (maior data)
                    var maisRecente = g.OrderByDescending(x => x.mr.data_manutencao).First();

                    var kmLimite = maisRecente.mr.quilometragem_ultima_manutencao + maisRecente.m.quilometragem_preventiva;
                    var kmAlerta = maisRecente.mr.quilometragem_ultima_manutencao + (maisRecente.m.quilometragem_preventiva * 0.8);

                    // Verifica se o veículo já passou do ponto de alerta
                    bool vencida = maisRecente.v.quilometragem >= kmAlerta;

                    // seleciona o tipo de caminhão
                    var tipoCaminhao = tipos.First(t => t.id == maisRecente.v.tipo);

                    if (!vencida)
                        return null; // não retorna notificações desnecessárias

                    return new Notificacao
                    {
                        id_veiculo = maisRecente.v.id_veiculo,
                        nomeVeiculo = maisRecente.v.apelido,
                        id_manutencao = maisRecente.m.id_manutencao,
                        nomeManutencao = maisRecente.m.nome,
                        quilometragemAtual = maisRecente.v.quilometragem,
                        quilometragemManutencao = kmLimite,
                        urgencia = maisRecente.v.quilometragem >= kmLimite,
                        tipo_caminhao = tipoCaminhao.nome,
                        descricao_manutencao = maisRecente.m.descricao,
                        idManutencaoRealizada = maisRecente.mr.id_manutencao
                    };
                })
                .Where(n => n != null) 
                .ToList();

            return notificacoes;
        }

    }
}

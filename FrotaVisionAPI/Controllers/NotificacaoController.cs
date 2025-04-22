using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.CompilerServices;

namespace FrotaVisionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificacaoController : ControllerBase
    {
        private readonly AppDBContext _context;

        public NotificacaoController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Notifacar")]
        [SwaggerOperation(Summary = "Gera a lista de notificação", Description = "")]
        public async Task<ActionResult<IEnumerable<Notificacao>>> GerarNotificacoes(string cnpj)
        {

            var notificacoes = await (
            from mr in _context.ManutencaoRealizadas
            join v in _context.Veiculos on mr.id_veiculo equals v.id_veiculo
            join m in _context.Manutencoes on mr.id_manutencao equals m.id_manutencao
            where v.quilometragem >= mr.quilometragem_ultima_manutencao + m.quilometragem_preventiva
            && v.cnpj == cnpj
            select new Notificacao
            {
                id_veiculo = v.id_veiculo,
                nomeVeiculo = v.apelido,
                id_manutencao = m.id_manutencao,
                nomeManutencao = m.nome,
                quilometragemAtual = v.quilometragem,
                quilometragemManutencao = mr.quilometragem_ultima_manutencao + m.quilometragem_preventiva,
            }
             ).ToListAsync();
            return notificacoes;
        }

    }
}

using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FrotaVisionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly AppDBContext _context;

        public EmpresaController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Listar")]
        [SwaggerOperation(Summary = "Lista todas as empresa", Description = "Retorna todas as empresa cadastradas.")]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresa()
        {
            return await _context.Empresas.ToListAsync();
        }

        [HttpGet("Pesquisar/{cnpj}")]
        [SwaggerOperation(Summary = "Obtém uma empresa", Description = "Retorna uma empresa pelo ID informado.")]
        public async Task<ActionResult<Empresa>> GetEmpresa(string cnpj)
        {
            var empresa = await _context.Empresas.FindAsync(cnpj);
            if (empresa == null)
                return NotFound(new { message = "Empresa não encontrado" });

            return empresa;
        }

        /// <summary>
        /// Cadastra um novo emrpesa.
        /// </summary>
        [HttpPost("Cadastrar")]
        [SwaggerOperation(Summary = "Cria uma nova empresa", Description = "Adiciona uma nova emrpesa ao banco de dados.")]
        public async Task<ActionResult<Empresa>> PostEmpresa(Empresa empresa)
        {

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmpresa), new { empresa.cnpj }, empresa);
        }

        /// <summary>
        /// Atualiza uma Empresa pelo ID.
        /// </summary>
        [HttpPut("Atualizar/{cnpj}")]
        [SwaggerOperation(Summary = "Atualiza uma empresa", Description = "Edita os dados de uma empresa existente.")]
        public async Task<IActionResult> PutEmpresa(string cnpj, Empresa empresa)
        {
            if (cnpj != empresa.cnpj)
                return BadRequest(new { message = "CNPJ da empresa não corresponde ao informado" });

            _context.Entry(empresa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Empresas.Any(e => e.cnpj == cnpj))
                    return NotFound(new { message = "Empresa não encontrado" });

                throw;
            }

            return Ok(new { message = "Empresa atualizado com sucesso", empresa = empresa });
        }

        /// <summary>
        /// Exclui um empresa pelo ID.
        /// </summary>
      

        [HttpDelete("Deletar/{cnpj}")]
        [SwaggerOperation(Summary = "Remove uma empresa", Description = "Deleta uma empresa do banco de dados.")]
        public async Task<IActionResult> DeleteEmpresa(string cnpj)
        {
            var empresa = await _context.Empresas.FindAsync(cnpj);
            if (empresa == null)
                return NotFound(new { message = "Empresa não encontrada" });

            empresa.habilitado = false;
            _context.Entry(empresa).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Empresa desativada com sucesso" });
        }

    }
}

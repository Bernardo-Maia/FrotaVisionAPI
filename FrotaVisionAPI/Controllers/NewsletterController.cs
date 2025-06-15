using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FrotaVisionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsletterController : ControllerBase
    {
        private readonly AppDBContext _context;

        public NewsletterController(AppDBContext context)
        {
            _context = context;
        }

        [HttpPost("Cadastrar")]
        [SwaggerOperation(Summary = "Cadastra novo newsletter", Description = "Adiciona uma novo email ao banco de dados.")]
        public async Task<ActionResult<Newsletter>> PostEmpresa(Newsletter newsletter)
        {

            _context.Newsletters.Add(newsletter);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostEmpresa), new { newsletter.id }, newsletter);
        }


    }
}

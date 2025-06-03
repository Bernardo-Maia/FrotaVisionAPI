using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace FrotaVisionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
       
        private readonly AppDBContext _context;

        public UsuarioController(AppDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os usuários.
        /// </summary>
        [HttpGet]
        [Route("Listar/{cnpj}")]
        [SwaggerOperation(Summary = "Lista todos os usuários", Description = "Retorna todos os usuários cadastrados.")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios(string cnpj)
        {
            var teset = _context.Usuarios.ToListAsync();
            return await _context.Usuarios.Where(x => x.cnpj == cnpj).ToListAsync();
        }

        [HttpGet]
        [Route("ListarDetalhado/{cnpj}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsuariosDetalhado(string cnpj)
        {
            var usuarios = await (
                from u in _context.Usuarios
                where u.habilitado == true && u.cnpj == cnpj
                join p in _context.Permissoes on u.permissoes_usuario equals p.id_permissao
                select new
                {
                    u.id_usuario,
                    u.nome_usuario,
                    u.email,
                    u.cnpj,
                    nomePermissao = p.nome,
                    p.descricao
                }
            ).ToListAsync();
            return Ok(usuarios);
        }

        /// <summary>
        /// Obtém um usuário pelo ID.
        /// </summary>
        [HttpGet("Pesquisar/{id}")]
        [SwaggerOperation(Summary = "Obtém um usuário", Description = "Retorna um usuário pelo ID informado.")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(new { message = "Usuário não encontrado" });

            return usuario;
        }

        /// <summary>
        /// Cadastra um novo usuário.
        /// </summary>
        [HttpPost("Cadastrar")]
        [SwaggerOperation(Summary = "Cria um novo usuário", Description = "Adiciona um novo usuário ao banco de dados.")]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            
            // Verifica se já existe um usuário com esse email
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == usuario.email);
            if (usuarioExistente != null)
            {
                return Conflict(new { message = "Já existe um usuário cadastrado com esse e-mail." });
            }
            usuario.senha = PasswordHasher.HashPassword(usuario.senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.id_usuario }, usuario);
        }

        /// <summary>
        /// Atualiza um usuário pelo ID.
        /// </summary>
        [HttpPut("Atualizar/{id}")]
        [SwaggerOperation(Summary = "Atualiza um usuário", Description = "Edita os dados de um usuário existente.")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.id_usuario)
                return BadRequest(new { message = "ID do usuário não corresponde ao informado" });

            usuario.senha = PasswordHasher.HashPassword(usuario.senha);
            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.id_usuario == id))
                    return NotFound(new { message = "Usuário não encontrado" });

                throw;
            }

            return Ok(new { message = "Usuário atualizado com sucesso", usuario = usuario });
        }

        /// <summary>
        /// Exclui um usuário pelo ID.
        /// </summary>
        [HttpDelete("Deletar/{id}")]
        [SwaggerOperation(Summary = "Remove um usuário", Description = "Deleta um usuário do banco de dados.")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(new { message = "Usuário não encontrado" });

            usuario.habilitado = false;
            _context.Entry(usuario).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário desabilitado com sucesso" });
        }

        [HttpPost("login/{login},{senha}")]
        public async Task<IActionResult> Login( string login, string senha)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == login);
            if (usuario == null || usuario.habilitado == false)
                return Unauthorized(new { message = "Usuário não encontrado" });

            // Verifica se a senha informada corresponde ao hash armazenado
            if (!PasswordHasher.VerifyPassword(/*request.Password*/ senha , usuario.senha))
                return Unauthorized(new { message = "Senha incorreta" });

            return Ok(new { message = "Login realizado com sucesso!",
                id = usuario.id_usuario,
                nome = usuario.nome_usuario,
                cnpj = usuario.cnpj,
                permissao = usuario.permissoes_usuario

            });
        }

    }
}

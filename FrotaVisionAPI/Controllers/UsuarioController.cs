using FrotaVisionAPI.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Numerics;

namespace FrotaVisionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
       
        private readonly AppDBContext _context;
        private readonly IConfiguration _config;

        public UsuarioController(AppDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Lista todos os usuários.
        /// </summary>
        [HttpGet]
        [Route("Listar/{cnpj}")]
        [SwaggerOperation(Summary = "Lista todos os usuários", Description = "Retorna todos os usuários cadastrados.")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios(string cnpj)
        {
            return await _context.Usuarios.Where(x => x.cnpj == cnpj && x.habilitado == true).ToListAsync();
        }

        [HttpGet]
        [Route("ListarDetalhado/{cnpj}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsuariosDetalhado(string cnpj)
        {
            return Ok(await (
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
            ).ToListAsync());
        }

        /// <summary>
        /// Obtém um usuário pelo ID.
        /// </summary>
        [HttpGet("Pesquisar/{id}")]
        [SwaggerOperation(Summary = "Obtém um usuário", Description = "Retorna um usuário pelo ID informado.")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            Usuario? usuario = await _context.Usuarios.FindAsync(id);
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
            int? planoEmpresa = _context.Empresas.FindAsync(usuario.cnpj).Result?.id_plano ?? 0;

            if (planoEmpresa == 0)
                return NotFound(new { message = "Empresa não encontrada" });

            int qntdUsuarios = _context.Usuarios.Count(u => u.cnpj == usuario.cnpj && u.habilitado == true);
            int? limite = _context.Planos.Where(p => p.id_plano == planoEmpresa).FirstOrDefault()?.qntd_usuarios ?? null;

            if (limite == null || limite <= qntdUsuarios)
                return BadRequest(new { message = "Limite de usuarios atingido" });

            // Verifica se já existe um usuário com esse email
            Usuario? usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == usuario.email);
            if (usuarioExistente != null)
            {
                return Conflict(new { message = "Já existe um usuário cadastrado com esse e-mail." });
            }
            usuario.senha = PasswordHasher.HashPassword(usuario.senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostUsuario), new { id = usuario.id_usuario }, usuario);
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
            Usuario? usuario = await _context.Usuarios.FindAsync(id);
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
            Usuario? usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == login);
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

        [HttpPost("loginJWT")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.email == request.Email);

            if (usuario == null || !PasswordHasher.VerifyPassword(request.Password, usuario.senha))
                return Unauthorized(new { message = "Email ou senha inválidos." });

            var token = JwtHelper.GenerateToken(usuario, _config);
            return Ok(new
            {
                token,
                usuario.id_usuario,
                usuario.email,
                usuario.permissoes_usuario,
                usuario.cnpj
            });
        }


    }
}

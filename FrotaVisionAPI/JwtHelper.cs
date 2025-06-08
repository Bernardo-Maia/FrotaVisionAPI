using FrotaVisionAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public static class JwtHelper
{
    public static string GenerateToken(Usuario usuario, IConfiguration config)
    {
        var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.id_usuario.ToString()),
            new Claim(ClaimTypes.Email, usuario.email),
            new Claim("Permissao", usuario.permissoes_usuario.ToString()),
            new Claim("Cnpj", usuario.cnpj ?? "")
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

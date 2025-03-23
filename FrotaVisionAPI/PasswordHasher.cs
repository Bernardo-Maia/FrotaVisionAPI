using BCrypt.Net;

namespace FrotaVisionAPI
{
    public static class PasswordHasher

    {

        // Transforma senha em Hash

        public static string HashPassword(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        // Verifica se a senha informada corresponde ao hash armazenado
        public static bool VerifyPassword(string senhaDigitada, string hashArmazenado)
        {
            return BCrypt.Net.BCrypt.Verify(senhaDigitada, hashArmazenado);
        }
    }
}

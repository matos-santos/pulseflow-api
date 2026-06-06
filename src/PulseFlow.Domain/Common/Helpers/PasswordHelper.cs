namespace PulseFlow.Domain.Common.Helpers;


/// <summary>
/// Helper para operações relacionadas a senhas
/// </summary>
public static class PasswordHelper
{
    /// <summary>
    /// Gera um hash da senha usando BCrypt
    /// </summary>
    /// <param name="password">Senha em texto plano</param>
    /// <returns>Hash da senha</returns>
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verifica se uma senha corresponde ao hash
    /// </summary>
    /// <param name="password">Senha em texto plano</param>
    /// <param name="hashedPassword">Hash da senha armazenado</param>
    /// <returns>True se a senha estiver correta</returns>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    /// <summary>
    /// Valida a força da senha
    /// </summary>
    /// <param name="password">Senha a ser validada</param>
    /// <returns>True se a senha atender aos requisitos mínimos</returns>
    public static bool IsStrongPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            return false;

        // Requisitos: mínimo 6 caracteres
        // Você pode adicionar mais regras aqui conforme necessário:
        // - Letra maiúscula
        // - Letra minúscula
        // - Número
        // - Caractere especial

        return true;
    }
}


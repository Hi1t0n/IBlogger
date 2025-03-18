namespace UserService.Infrastructure.Services;

/// <summary>
/// Криптографический сервис.
/// </summary>
public static class CryptoService
{
    /// <summary>
    /// Шифрование пароля
    /// </summary>
    /// <param name="password">Пароль</param>
    /// <returns>Хэш пароля</returns>
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Проверка схожести пароля
    /// </summary>
    /// <param name="hashPassword">Хэш пароля</param>
    /// <param name="password">Пароль</param>
    /// <returns>True, если пароли совпадают, иначе false</returns>
    public static bool Verify(string hashPassword, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashPassword);
    }
}
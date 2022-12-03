namespace Application.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPasswordBCrypt(this string plain)
        {
            return BCrypt.Net.BCrypt.HashPassword(plain);
        }

        public static bool VerifyHashedPasswordBCrypt(this string hashed, string plain)
        {
            return BCrypt.Net.BCrypt.Verify(plain, hashed);
        }
    }
}

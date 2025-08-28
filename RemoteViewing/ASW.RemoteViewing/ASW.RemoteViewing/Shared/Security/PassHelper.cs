using Microsoft.AspNetCore.Identity;

namespace ASW.RemoteViewing.Shared.Security
{
    public static class PassHelper<TUser> where TUser : class
    {
        private readonly static PasswordHasher<TUser> _passwordHasher = new();
        private static Func<TUser, string, string, bool>? _testVerify;

        public static string GetHash(TUser user, string pass)
        {
            return _passwordHasher.HashPassword(user, pass);
        }

        public static bool Verify(TUser user, string hash, string pass)
        {
            if (_testVerify != null)
                return _testVerify.Invoke(user, hash, pass);

            var result = _passwordHasher.VerifyHashedPassword(user, hash, pass);
            return result == PasswordVerificationResult.Success;
        }

        public static void SetTestVerifier(Func<TUser, string, string, bool> customVerify)
        {
            _testVerify = customVerify;
        }

        public static void ResetTestVerifier() => _testVerify = null;
    }
}

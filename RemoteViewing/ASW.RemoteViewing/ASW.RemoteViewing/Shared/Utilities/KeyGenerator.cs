using System.Security.Cryptography;

namespace ASW.RemoteViewing.Shared.Utilities;

public static class KeyGenerator
{
    public static string GenerateKey()
    { 
        return $"dev_{Convert.ToHexString(RandomNumberGenerator.GetBytes(32))}";
    }
}

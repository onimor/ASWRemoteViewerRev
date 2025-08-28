using System.Security.Cryptography;

namespace ASW.RemoteViewing.Infrastructure.Security.Key;

public class KeyService
{
    public virtual RSA RsaKey1 { get; protected set; }
    public virtual RSA RsaKey2 { get; protected set; }
    public virtual RSA RsaKey3 { get; protected set; }

    public KeyService()
    {
        RsaKey1 = LoadOrCreateKey("key1");
        RsaKey2 = LoadOrCreateKey("key2");
        RsaKey3 = LoadOrCreateKey("key3"); 
    }
    private RSA LoadOrCreateKey(string path)
    {
        var rsa = RSA.Create();
        if (File.Exists(path))
            rsa.ImportRSAPrivateKey(File.ReadAllBytes(path), out _);
        else
            File.WriteAllBytes(path, rsa.ExportRSAPrivateKey());
        return rsa;
    }
}

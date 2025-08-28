using System.Security.Cryptography;

namespace ASW.RemoteViewing.Infrastructure.Security.Key
{
    public class TestKeyService : KeyService
    {
        public TestKeyService()
        {
            RsaKey1 = RSA.Create();
            RsaKey2 = RSA.Create();
            RsaKey3 = RSA.Create();
        }

        public override RSA RsaKey1 { get; protected set; }
        public override RSA RsaKey2 { get; protected set; }
        public override RSA RsaKey3 { get; protected set; }
    }

}

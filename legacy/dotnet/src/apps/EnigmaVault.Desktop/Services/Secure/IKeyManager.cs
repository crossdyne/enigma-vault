
using Crossdyne.Toolkit.Primitives;

namespace EnigmaVault.Desktop.Services.Secure
{
    public interface IKeyManager
    {
        void SaveKey(byte[] key);
        Maybe<byte[]> GetKey();
        void ClearKey();
    }
}
using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Desktop.Models;

namespace EnigmaVault.Desktop.Services.Managers
{
    internal interface ITokenManager
    {
        void SaveTokens(AccessData tokens);
        Maybe<AccessData> GetTokens();
        void ClearTokens();
    }
}
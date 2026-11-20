using Crossdyne.Security.Abstractions;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.Secure;
using EnigmaVault.Desktop.Services.WindowNavigation;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Features.Authentication;
using Shared.Contracts.AuthenticationService.Clients;
using Shared.Contracts.UserManagementService.Clients;

namespace EnigmaVault.Desktop.ViewModels.Windows
{
    internal sealed partial class AuthenticationWindowViewModel(
        IWindowNavigation windowNavigation,
        IPageNavigation pageNavigation,
        IUserManagementService userManagementService,
        IUserContext userContext,
        IAuthService authService,
        ITokenManager tokenManager,
        IKeyManager keyManager,
        ISrpClient srpClient,
        IKeyDerivationService keyDerivationService,
        ISrpKeyDerivationService srpKeyDerivationService,
        ICryptoService cryptoServices) : BaseWindowViewModel(windowNavigation, pageNavigation)
    {
        public LoginViewModel Login { get; } = new(windowNavigation, pageNavigation, authService, userManagementService, userContext, tokenManager, keyManager, srpClient, srpKeyDerivationService, keyDerivationService, cryptoServices);
    }
}
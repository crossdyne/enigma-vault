using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Models;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.Secure;
using EnigmaVault.Desktop.Services.WindowNavigation;
using Shared.Contracts.AuthenticationService.Clients;
using Shared.Contracts.AuthenticationService.Requests;
using Shared.Contracts.UserManagementService.Clients;

namespace EnigmaVault.Desktop.Services.Initializers
{
    internal class ApplicationInitializer(
        ITokenManager tokenManager,
        IKeyManager keyManager,
        IAuthService authService,
        IUserManagementService userManagementService,
        IUserContext userContext,
        IWindowNavigation windowNavigation,
        IPageNavigation pageNavigation) : IApplicationInitializer
    {
        private readonly ITokenManager _tokenManager = tokenManager;
        private readonly IKeyManager _keyManager = keyManager;
        private readonly IAuthService _authService = authService;
        private readonly IUserManagementService _userManagementService = userManagementService;
        private readonly IUserContext _userContext = userContext;
        private readonly IWindowNavigation _windowNavigation = windowNavigation;
        private readonly IPageNavigation _pageNavigation = pageNavigation;

        public void InitializeAsync()
        {
            var tokenMaybe = _tokenManager!.GetTokens();

            tokenMaybe.Match(
                onSome: async token =>
                {
                    var authResult = await _authService!.RefreshTokens(new LoginByTokenRequest(token.RefreshToken));

                    authResult.Switch(
                        onSuccess: async () =>
                        {
                            Maybe<byte[]> dek = _keyManager.GetKey();

                            dek.Match(
                                onSome : key => _userContext.UpdateDek(key),
                                onNone: () => _windowNavigation!.Open(WindowsName.AuthenticationWindow));

                            _tokenManager.SaveTokens(new AccessData(authResult.Value!.AccessToken, authResult.Value!.RefreshToken));
                            var userInfo = await _userManagementService.Me(authResult.Value.AccessToken);

                            _userContext.UpdateUserInfo(new UserInfo(userInfo.Value!.Id, userInfo.Value.Login));
                            _userContext.UpdateTokens(new AccessData(authResult.Value.AccessToken, authResult.Value.RefreshToken));

                            _windowNavigation.Open(WindowsName.MainWindow);
                            _pageNavigation.Navigate(PagesName.Password, FramesName.MainFrame);
                        },
                        onFailure: errors => _windowNavigation!.Open(WindowsName.AuthenticationWindow));
                },
                onNone: () => _windowNavigation!.Open(WindowsName.AuthenticationWindow));
        }
    }
}
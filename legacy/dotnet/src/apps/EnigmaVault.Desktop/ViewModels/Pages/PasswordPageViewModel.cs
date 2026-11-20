using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crossdyne.Security.Abstractions;
using Crossdyne.Security.Configuration;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Helpers;
using EnigmaVault.Desktop.Models;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Common.Assets;
using EnigmaVault.Desktop.ViewModels.Common.Controls;
using EnigmaVault.Desktop.ViewModels.Common.Organization;
using EnigmaVault.Desktop.ViewModels.Features.Credentials.Items;
using EnigmaVault.Desktop.ViewModels.Features.Credentials.Vault;
using Microsoft.Extensions.Options;
using Shared.Contracts.AssetsService.Clients;
using Shared.Contracts.AssetsService.Responses;
using Shared.Contracts.FileService.Clients;
using Shared.Contracts.FileService.Requests;
using Shared.Contracts.FileService.Responses;
using Shared.Contracts.SecretService.Requests;
using Shared.Contracts.SecretService.Responses;
using Shared.Contracts.SecretService.Clients;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace EnigmaVault.Desktop.ViewModels.Pages
{
    internal sealed partial class PasswordPageViewModel : BasePageViewModel, IAsyncInitializable, IUpdatable, ISidebarController
    {
        private readonly IVaultService _vaultService;
        private readonly ITagService _tagService;
        private readonly IAssetService _assetClient;
        private readonly IAssetCategoryService _iconCategoryService;
        private readonly IFileService _fileService;
        private readonly IUserContext _userContext;
        private readonly ICryptoService _cryptoServices;

        // ====================================================================================
        //                                      ИНИЦИАЛИЗАЦИЯ                                        
        // ====================================================================================

        public PasswordPageViewModel(
            IOptions<Urls> urlsOptions,
            IVaultService vaultService,
            ITagService tagService,
            IAssetService assetClient,
            IAssetCategoryService iconCategoryService,
            IFileService fileService,
            IUserContext userContext,
            ICryptoService cryptoServices)
        {
            WebSiteUrls = urlsOptions.Value;

            _vaultService = vaultService;
            _tagService = tagService;
            _assetClient = assetClient;
            _iconCategoryService = iconCategoryService;
            _fileService = fileService;
            _userContext = userContext;
            _cryptoServices = cryptoServices;

            SelectedPasswordType = PasswordTypes.FirstOrDefault();
            SelectedSorting = SortingView.Ascending;
            CurrentDisplayUserControlLeftSideMenu = UserControlsName.Tags;
            CurrentActionRightSideMenu = ActionOnData.Create;

            PasswordsView = CollectionViewSource.GetDefaultView(Passwords);
            IconView = CollectionViewSource.GetDefaultView(Icons);
            IconCategoryView = CollectionViewSource.GetDefaultView(IconCategories);

            UpdateIconsView();
            UpdateIconCategoriesView();

            ArchivesPopup = new(PopupPlacementMode.CustomRightUp, PlacementMode.Custom, () => ArchivedPasswords.Count > 0);

            ArchivedPasswords.CollectionChanged += (s, e) =>
            {
                ArchivesPopup?.UpdateCanExecute();
            };

            TrashPopup = new(PopupPlacementMode.CustomRightUp, PlacementMode.Custom, () => TrashPasswords.Count > 0);

            TrashPasswords.CollectionChanged += (s, e) =>
            {
                TrashPopup?.UpdateCanExecute();
                RestoreAllTrashCommand.NotifyCanExecuteChanged();
                EmptyTrashCommand.NotifyCanExecuteChanged();
            };

            AttachTagsPopup = new(PopupPlacementMode.CustomCenter, PlacementMode.Custom);

            CurrentTemplateTypePasswords = TemplateType.Detailed;
            SelectedGrouping = GroupingView.None;
        }

        public async Task InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            try
            {
                await Task.Delay(1111);

                await GetTags();
                await GetIconCategories();
                await GetIcons();

                await GetEncryptedOverview();

                UpdateGroupingPassword();

                IsInitialize = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка инициализации: {ex}");
            }
        }

        public void Update<TData>(TData value, TransmittingParameter parameter)
        {

        }

        // ====================================================================================
        //                                      КОЛЛЕКЦИИ                                        
        // ====================================================================================

        public ObservableCollection<CredentialsVaultViewModel> Passwords { get; init; } = [];
        public ICollectionView PasswordsView { get; private init; } = null!;

        public ObservableCollection<CredentialsVaultViewModel> ArchivedPasswords { get; init; } = [];
        public ObservableCollection<CredentialsVaultViewModel> TrashPasswords { get; init; } = [];
        public ObservableCollection<TagViewModel> Tags { get; init; } = [];

        public ObservableCollection<IconViewModel> Icons { get; init; } = [];
        public ICollectionView IconView { get; private set; } = null!;

        public ObservableCollection<IconCategoryResponse> IconCategories { get; init; } = [];
        public ICollectionView IconCategoryView { get; private init; } = null!;

        public ObservableCollection<KeyValuePair<VaultType, string>> PasswordTypes { get; private set; } =
        [
            new KeyValuePair<VaultType, string>(VaultType.Password, "Пароль"),
            new KeyValuePair<VaultType, string>(VaultType.Server, "Данные сервера"),
            new KeyValuePair<VaultType, string>(VaultType.CreditCard, "Банковские карты"),
            new KeyValuePair<VaultType, string>(VaultType.ApiKey, "Апи Ключ"),
            new KeyValuePair<VaultType, string>(VaultType.ConnectionString, "Строка подключения"),
            new KeyValuePair<VaultType, string>(VaultType.AsymmetricKey, "Ассеметричные ключи"),
        ];

        // ====================================================================================
        //                                      СВОЙСТВА                                        
        // ====================================================================================

        public ToolTipController RightToolTipController { get; } = new(ToolTipPlacement.CenterRight);
        public ToolTipController LeftToolTipController { get; } = new(ToolTipPlacement.CenterLeft);
        public ToolTipController TopToolTipController { get; } = new(ToolTipPlacement.CenterTop);
        public ToolTipController BottomToolTipController { get; } = new(ToolTipPlacement.CenterBottom);

        public PopupController PasswordMenuPopup { get; } = new(); 
        public PopupController AttachTagsPopup { get; } 
        public PopupController ArchivesPopup { get; }
        public PopupController TrashPopup { get; }

        // ================Vault=====================

        #region Свойство: [SelectedEncryptedOverview] - Выбор зашифрованного элемента

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UpdateVaultCommand))]
        private CredentialsVaultViewModel? _selectedEncryptedOverview;

        partial void OnSelectedEncryptedOverviewChanged(CredentialsVaultViewModel? value)
        {
            if (value is null)
                return;

            CreateViewModelForType(value.Type, value);
            SelectedPasswordType = PasswordTypes.FirstOrDefault(pt => pt.Key == value.Type);
            CurrentActionRightSideMenu = ActionOnData.View;
            SetReadOnly(CurrentActionRightSideMenu);
            SelectedCredentialItemBaseViewModel?.Decrypt(value.EncryptedOverview, value.EncryptedDetails, _cryptoServices, _userContext);
            SelectedCredentialItemBaseViewModel?.SetIcon(Icons.FirstOrDefault(i => i.Id == value.IconId)?.Icon);

            foreach (var tag in Tags)
            {
                if (tag is null) continue;

                if (value.Tags.Contains(tag))
                    tag.AttachedTag();
                else
                    tag.DetachedTag();
            }
        }

        #endregion

        #region Свойств: [SelectedArchivedEncryptedOverview] - Выбор зашифрованного элемента в архиве

        [ObservableProperty]
        private CredentialsVaultViewModel? _selectedArchivedEncryptedOverview;

        #endregion

        #region Свойство: [SelectedTrashEncryptedOverview] - Выбор зашифрованного элемента в корзине

        [ObservableProperty]
        private CredentialsVaultViewModel? _selectedTrashEncryptedOverview;

        #endregion

        #region Свойство: [SelectedPasswordType], Метод [OnSelectedPasswordTypeChanged]

        [ObservableProperty]
        private KeyValuePair<VaultType, string> _selectedPasswordType;

        partial void OnSelectedPasswordTypeChanged(KeyValuePair<VaultType, string> value)
        {
            CreateViewModelForType(value.Key, SelectedEncryptedOverview!);
            SetReadOnly(CurrentActionRightSideMenu);
        }

        #endregion

        #region Свойство: [SelectedPasswordViewModel]

        [ObservableProperty]
        private CredentialItemBaseViewModel? _selectedCredentialItemBaseViewModel;

        partial void OnSelectedCredentialItemBaseViewModelChanged(CredentialItemBaseViewModel? value)
        {
            value?.SetIsReadOnly(CurrentActionRightSideMenu != ActionOnData.View);
        }

        #endregion

        #region Свойство: [SelectedGrouping] - Выбор группировки списка паролей

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetGroupingPasswordsCommand))]
        private GroupingView _selectedGrouping;

        partial void OnSelectedGroupingChanged(GroupingView value)
        {
            UpdateGroupingPassword();
        }

        #endregion

        #region Свойство: [SelectedSorting] - Выбор сортировки списка паролей

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetSortPasswordsCommand))]
        private SortingView _selectedSorting;

        partial void OnSelectedSortingChanged(SortingView value)
        {
            UpdateGroupingPassword();
        }

        #endregion

        #region Свойство: [Urls] - список URL по которым можно перейти

        [ObservableProperty]
        public Urls _webSiteUrls;

        #endregion

        // =================Tag======================

        #region Свойства: Tags, Метод: [OnSelectedTagChanged]

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateTagCommand))]
        private string? _nameTag;

        [ObservableProperty]
        private TagViewModel? _selectedTag;

        partial void OnSelectedTagChanged(TagViewModel? value)
        {
            if (value is not null)
            {
                UpdateRed = value!.RgbColor.R.ToString();
                UpdateGreen = value!.RgbColor.G.ToString();
                UpdateBlue = value!.RgbColor.B.ToString();

                value.SetColor(Color.FromRgb(byte.Parse(UpdateRed), byte.Parse(UpdateGreen), byte.Parse(UpdateBlue)));
            }
        }

        [ObservableProperty]
        private TagViewModel? _selectedAttachedTag;

        partial void OnSelectedAttachedTagChanged(TagViewModel? value)
        {
            if (value is null)
                return;

            if (value.IsAttached)
                DetachTagToSelectedVaultCommand.Execute(value);
            else
                AttachTagToSelectedVaultCommand.Execute(value);
        }

        [ObservableProperty]
        private string _red = "255";

        [ObservableProperty]
        private string _green = "255";

        [ObservableProperty]
        private string _blue = "255";

        [ObservableProperty]
        private string _updateRed = null!;

        [ObservableProperty]
        private string _updateGreen = null!;

        [ObservableProperty]
        private string _updateBlue = null!;

        partial void OnUpdateRedChanged(string value) => UpdateSelectedTagColor();

        partial void OnUpdateGreenChanged(string value) => UpdateSelectedTagColor();

        partial void OnUpdateBlueChanged(string value) => UpdateSelectedTagColor();

        private void UpdateSelectedTagColor()
        {
            if (SelectedTag == null)
                return;

            bool redParsed = byte.TryParse(UpdateRed, out byte r);
            bool greenParsed = byte.TryParse(UpdateGreen, out byte g);
            bool blueParsed = byte.TryParse(UpdateBlue, out byte b);

            if (redParsed && greenParsed && blueParsed)
                SelectedTag?.SetColor(System.Windows.Media.Color.FromRgb(r, g, b));
        }


        #endregion

        // ================Icon======================

        #region Свойство: [SelectedIcon]

        [ObservableProperty]
        private IconViewModel? _selectedIcon;

        partial void OnSelectedIconChanged(IconViewModel? value)
        {
            if (value is null)
                return;

            if (CurrentActionRightSideMenu is ActionOnData.Create || CurrentActionRightSideMenu is ActionOnData.Update)
            {
                if (SelectedCredentialItemBaseViewModel is null)
                    return;

                SelectedCredentialItemBaseViewModel.SvgCode = value.Key;
                SelectedCredentialItemBaseViewModel.IconId = value.Id;
                SelectedCredentialItemBaseViewModel.SetIcon(value.Icon);
            }
        }

        #endregion

        #region Свойства: SVG

        [ObservableProperty]
        private IconCategoryResponse? _selectedIconCategory;

        #endregion

        // ==============SideMenu====================

        #region Свойства: [CurrentDisplayUserControlLeftSideMenu, CurrentDisplayUserControlRightSideMenu] - Текущий отображаемый элемент в боковых меню

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetLeftSideMenuControlCommand))]
        private UserControlsName _currentDisplayUserControlLeftSideMenu = UserControlsName.Folders;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetRightSideMenuActionCommand))]
        private ActionOnData _currentActionRightSideMenu = ActionOnData.View;

        partial void OnCurrentActionRightSideMenuChanged(ActionOnData value)
        {
            SetReadOnly(value);

            if (value == ActionOnData.Create || value == ActionOnData.Update)
                SelectedIcon = Icons.FirstOrDefault(i => i.Id == SelectedEncryptedOverview?.IconId);
            else
                SelectedIcon = null;
        }

        #endregion

        #region Свойство: [CurrentTemplateTypePasswords] - Текущий отображаемый темплейт у списка с паролями.

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetTemplatePasswordsCommand))]
        private TemplateType _currentTemplateTypePasswords;

        #endregion

        // ====================================================================================
        //                                      КОМАНДЫ                                        
        // ====================================================================================

        // ================Vault=====================

        /*--CRUD--*/

        #region Команда [CreateVault]: Создание зашифрованного элемента

        [RelayCommand]
        public async Task CreateVault()
        {
            (string EncryptedOverView, string EncryptedDetails, CryptoVersion CryptoVersion) = SelectedCredentialItemBaseViewModel!.Encrypt(_cryptoServices, _userContext);

            var result = await _vaultService.CreateAsync(new CreateVaultItemRequest((int)SelectedPasswordType.Key, SelectedIcon!.Id!, EncryptedOverView, EncryptedDetails, (int)CryptoVersion));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            var encryptedVm = new CredentialsVaultViewModel(
                    new EncryptedVaultResponse(
                        result.Value,
                        SelectedPasswordType.Key.ToString(),
                        DateTime.Now,
                        DateUpdate: null,
                        DeletedAt: null,
                        IsFavorite: false,
                        IsArchive: false,
                        IsInTrash: false,
                        EncryptedOverView,
                        EncryptedDetails,
                        [],
                        SelectedIcon!.Id!),
                    _cryptoServices, 
                    _userContext.Dek,
                    Tags);

            encryptedVm.Icon = Icons.FirstOrDefault(i => i.Id == encryptedVm.IconId)?.Icon;

            Passwords.Add(encryptedVm);
        }

        #endregion

        #region Команда [UpdateVault]: Обновление записи

        [RelayCommand(CanExecute = nameof(CanUpdateVault))]
        private async Task UpdateVault()
        {
            (string EncryptedOverView, string EncryptedDetails, CryptoVersion CryptoVersion) = SelectedCredentialItemBaseViewModel!.Encrypt(_cryptoServices, _userContext);

            var result = await _vaultService.UpdateAsync(new UpdateVaultItemRequest(SelectedEncryptedOverview!.Id, SelectedIcon!.Id!, EncryptedOverView, EncryptedDetails, (int)CryptoVersion));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            SelectedEncryptedOverview!.UpdateEncrypted(EncryptedOverView, EncryptedDetails);
            SelectedEncryptedOverview!.UpdateDate(DateTime.Parse(result.Value).ToLocalTime());
            SelectedEncryptedOverview.SetIcon(SelectedIcon?.Icon);
        }

        private bool CanUpdateVault() => SelectedEncryptedOverview is not null;

        #endregion

        /*--Action--*/

        #region Команда [SetFavorite]: Изменение статуса избранного

        [RelayCommand(CanExecute = nameof(CanSetFavorite))]
        private async Task SetFavorite(CredentialsVaultViewModel model)
        {
            void SetValue(Result<Unit> result, bool condition)
            {
                if (result.IsFailure)
                {
                    MessageBox.Show($"{result.StringMessage}");
                    return;
                }

                model!.IsFavorite = condition;
            }

            if (model.IsFavorite)
            {
                var result = await _vaultService.RemoveFromFavoritesAsync(model.Id);
                SetValue(result, false);
            }
            else
            {
                var result = await _vaultService.AddToFavoritesAsync(model.Id);
                SetValue(result, true);
            }
        }

        private bool CanSetFavorite(CredentialsVaultViewModel model) => model is not null;

        #endregion

        #region Команда [SetArchive]: Изменение статуса архивации

        [RelayCommand(CanExecute = nameof(CanSetArchive))]
        private async Task SetArchive(CredentialsVaultViewModel model)
        {
            void SetValue(Result<Unit> result, bool condition)
            {
                if (result.IsFailure)
                {
                    MessageBox.Show($"{result.StringMessage}");
                    return;
                }

                model!.IsArchive = condition;

                if (condition)
                {
                    Passwords.Remove(model);
                    ArchivedPasswords.Add(model);
                }
                else
                {
                    ArchivedPasswords.Remove(model);
                    Passwords.Add(model);
                }

                PasswordMenuPopup.HideCommand.Execute(null);
            }

            if (model.IsArchive)
            {
                var result = await _vaultService.UnArchiveAsync(model.Id);
                SetValue(result, false);

                if (ArchivedPasswords.Count <= 0)
                    ArchivesPopup.HideCommand.Execute(null);
            }
            else
            {
                var result = await _vaultService.ArchiveAsync(model.Id);
                SetValue(result, true);
            }
        }

        private bool CanSetArchive(CredentialsVaultViewModel model) => model is not null;

        #endregion

        #region Команда [AttachTagToSelectedVaultCommand]: Присоединяет тэг к выбранному зашифрованному элементу

        [RelayCommand]
        private async Task AttachTagToSelectedVault(TagViewModel tag)
        {
            if (SelectedEncryptedOverview is null)
                return;

            var result = await _vaultService.AddTagAsync(SelectedEncryptedOverview.Id, tag.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            SelectedEncryptedOverview.AddTag(tag.Id);

            if (tag.IsAttached)
                tag.DetachedTag();
            else
                tag.AttachedTag();
        }

        [RelayCommand]
        private async Task DetachTagToSelectedVault(TagViewModel tag)
        {
            if (SelectedEncryptedOverview is null)
                return;

            var result = await _vaultService.RemoveTagAsync(SelectedEncryptedOverview.Id, tag.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            SelectedEncryptedOverview.RemoveTag(tag.Id);
            tag.DetachedTag();
        }

        #endregion

        /*--Trash--*/

        #region Команда [MoveToTrashCommand]: Переносит запись в корзину (Мягкое удаление)

        [RelayCommand(CanExecute = nameof(CanMoveToTrash))]
        private async Task MoveToTrash(CredentialsVaultViewModel model)
        {
            var result = await _vaultService.MoveToTrashAsync(model.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            model.DeletedAt = result.Value.ToLocalTime();

            Passwords.Remove(model);
            TrashPasswords.Add(model);
            PasswordMenuPopup.HideCommand.Execute(null);
        }

        private bool CanMoveToTrash(CredentialsVaultViewModel model) => model is not null;

        #endregion

        #region Команда [RestoreTrashCommand]: Восстанавливает запись из корзины

        [RelayCommand(CanExecute = nameof(CanRestoreTrash))]
        private async Task RestoreTrash(CredentialsVaultViewModel model)
        {
            var result = await _vaultService.RestoreFromTrashAsync(model.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            model.DeletedAt = null;

            TrashPasswords.Remove(model);
            Passwords.Add(model);

            if (TrashPasswords.Count <= 0)
                TrashPopup.HideCommand.Execute(null);
        }

        private bool CanRestoreTrash(CredentialsVaultViewModel model) => model is not null;

        #endregion

        #region Команда [RestoreAllTrashCommand] : Восстановить все записи из корзины

        [RelayCommand(CanExecute = nameof(CanRestoreAllTrash))]
        private async Task RestoreAllTrash()
        {
            if (MessageBox.Show($"Вы точно хотите восстановить все записи в кол-ве {TrashPasswords.Count}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            var result = await _vaultService.RestoreAllFromTrashAsync();

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            foreach (var vault in TrashPasswords.ToList())
            {
                TrashPasswords.Remove(vault);
                vault.IsInTrash = false;
                Passwords.Add(vault);
            }

            TrashPopup.HideCommand.Execute(null);
        }

        private bool CanRestoreAllTrash() => TrashPasswords.Count > 0;

        #endregion

        #region Команда [EmptyTrashCommand] : Очистка корзины

        [RelayCommand(CanExecute = nameof(CanEmptyTrash))]
        private async Task EmptyTrash()
        {
            if (MessageBox.Show($"Вы точно хотите удалить все записи в кол-ве {TrashPasswords.Count}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            var result = await _vaultService.EmptyTrashAsync();

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            TrashPasswords.Clear();

            TrashPopup.HideCommand.Execute(null);
        }

        private bool CanEmptyTrash() => TrashPasswords.Count > 0;

        #endregion

        /*--Template--*/

        #region Команда [SetTemplatePasswordsCommand]: Выбор текущего темплейта у списка с паролями

        [RelayCommand(CanExecute = nameof(CanSetTemplatePasswords))]
        private void SetTemplatePasswords(TemplateType type) => CurrentTemplateTypePasswords = type;

        private bool CanSetTemplatePasswords(TemplateType type) => type != CurrentTemplateTypePasswords;

        #endregion

        #region Команда [SetGroupingPasswordsCommand]: Выбор текущей группировки у списка с паролями

        [RelayCommand(CanExecute = nameof(CanSetGroupingPasswords))]
        private void SetGroupingPasswords(GroupingView type) => SelectedGrouping = type;

        private bool CanSetGroupingPasswords(GroupingView type) => type != SelectedGrouping;

        #endregion

        #region Команда [SetGroupingPasswordsCommand]: Выбор текущей группировки у списка с паролями

        [RelayCommand(CanExecute = nameof(CanSetSortPasswords))]
        private void SetSortPasswords(SortingView type) => SelectedSorting = type;

        private bool CanSetSortPasswords(SortingView type) => type != SelectedSorting;

        #endregion

        /*--PopupManagement--*/

        #region Команда [SelectAndShowPasswordMenuPopup]: Отвечает за выбор элемента списка паролей при открытие контекстного меню 

        [RelayCommand]
        private void SelectAndShowPasswordMenuPopup(CredentialsVaultViewModel password)
        {
            if (password is null) return;

            SelectedEncryptedOverview = password;

            PasswordMenuPopup.ShowAtMouse();
        }

        #endregion

        #region Команда [OpenAttachTagPopupCommand]

        [RelayCommand]
        private void OpenAttachTagPopupCommand(UIElement? target)
        {
            if (PasswordMenuPopup.IsOpen)
                PasswordMenuPopup.HideCommand.Execute(null);

            AttachTagsPopup.ShowCommand.Execute(target);
        }

        #endregion

        /*--Ссылки--*/

        #region Команда [NavigateToAssetsWeb]: Производит навигацию в браузер, на сайт с настройками ассетов

        [RelayCommand()]
        private void NavigateToAssetsWeb()
        {
            if (string.IsNullOrWhiteSpace(WebSiteUrls.Assets))
                return;

            var psi = new ProcessStartInfo
            {
                FileName = WebSiteUrls.Assets,
                UseShellExecute = true,
            };

            Process.Start(psi);
        }

        #endregion

        #region Команда [CopyFieldCommand]: Копирует выбранное свойство

        [RelayCommand]
        private void CopyField(FieldToCopy field)
        {
            if (SelectedEncryptedOverview == null)
                return;

            Action action = SelectedEncryptedOverview.Type switch
            {
                VaultType.Password => () =>
                {
                    CreateViewModelForType(SelectedEncryptedOverview.Type, SelectedEncryptedOverview);
                    SelectedCredentialItemBaseViewModel?.Decrypt(SelectedEncryptedOverview.EncryptedOverview, SelectedEncryptedOverview.EncryptedDetails, _cryptoServices, _userContext);
                    var standardPassword = SelectedCredentialItemBaseViewModel as StandardPasswordViewModel;

                    if (standardPassword is null)
                        return;

                    switch (field)
                    {
                        case FieldToCopy.StandardPasswordLogin:
                            Clipboard.SetText(standardPassword.Login!);
                            break;
                        case FieldToCopy.StandardPassword:
                            Clipboard.SetText(standardPassword.Password!);
                            break;
                        case FieldToCopy.StandardPasswordEmail:
                            Clipboard.SetText(standardPassword.Email!);
                            break;
                        case FieldToCopy.StandardPasswordPhoneNumber:
                            Clipboard.SetText(standardPassword.Phone!);
                            break;
                        default:
                            break;
                    }
                }
                ,
                VaultType.Server => () =>
                {
                    CreateViewModelForType(SelectedEncryptedOverview.Type, SelectedEncryptedOverview);
                    SelectedCredentialItemBaseViewModel?.Decrypt(SelectedEncryptedOverview.EncryptedOverview, SelectedEncryptedOverview.EncryptedDetails, _cryptoServices, _userContext);
                    var server = SelectedCredentialItemBaseViewModel as ServerPasswordViewModel;

                    if (server is null)
                        return;

                    switch (field)
                    {
                        case FieldToCopy.ServerAddress:
                            Clipboard.SetText(server.IpAddress!);
                            break;
                        case FieldToCopy.ServerPort:
                            Clipboard.SetText(server.Port?.ToString()!);
                            break;
                        case FieldToCopy.ServerLogin:
                            Clipboard.SetText(server.Login!);
                            break;
                        case FieldToCopy.ServerPassword:
                            Clipboard.SetText(server.RootPassword!);
                            break;
                        default:
                            break;
                    }
                }
                ,
                VaultType.CreditCard => () =>
                {
                    CreateViewModelForType(SelectedEncryptedOverview.Type, SelectedEncryptedOverview);
                    SelectedCredentialItemBaseViewModel?.Decrypt(SelectedEncryptedOverview.EncryptedOverview, SelectedEncryptedOverview.EncryptedDetails, _cryptoServices, _userContext);
                    var creditCard = SelectedCredentialItemBaseViewModel as CreditCardViewModel;

                    if (creditCard is null)
                        return;

                    switch (field)
                    {
                        case FieldToCopy.CreditCardNumber:
                            Clipboard.SetText(creditCard.CardNumber!);
                            break;
                        case FieldToCopy.CreditCardOwner:
                            Clipboard.SetText(creditCard.CardHolder!);
                            break;
                        case FieldToCopy.CreditCardCVV:
                            Clipboard.SetText(creditCard.CvvCode!);
                            break;
                        default:
                            break;
                    }
                }
                ,
                VaultType.ApiKey => () =>
                {
                    CreateViewModelForType(SelectedEncryptedOverview.Type, SelectedEncryptedOverview);
                    SelectedCredentialItemBaseViewModel?.Decrypt(SelectedEncryptedOverview.EncryptedOverview, SelectedEncryptedOverview.EncryptedDetails, _cryptoServices, _userContext);
                    var apiKey = SelectedCredentialItemBaseViewModel as ApiKeyViewModel;

                    if (apiKey is null)
                        return;

                    Clipboard.SetText(apiKey.ApiKey!);
                }
                ,
                VaultType.ConnectionString => () =>
                {
                    CreateViewModelForType(SelectedEncryptedOverview.Type, SelectedEncryptedOverview);
                    SelectedCredentialItemBaseViewModel?.Decrypt(SelectedEncryptedOverview.EncryptedOverview, SelectedEncryptedOverview.EncryptedDetails, _cryptoServices, _userContext);
                    var connectionString = SelectedCredentialItemBaseViewModel as ConnectionStringViewModel;

                    if (connectionString is null)
                        return;

                    Clipboard.SetText(connectionString.Value!);
                }
                ,
                VaultType.AsymmetricKey => () =>
                {
                    CreateViewModelForType(SelectedEncryptedOverview.Type, SelectedEncryptedOverview);
                    SelectedCredentialItemBaseViewModel?.Decrypt(SelectedEncryptedOverview.EncryptedOverview, SelectedEncryptedOverview.EncryptedDetails, _cryptoServices, _userContext);
                    var asymmetricKey = SelectedCredentialItemBaseViewModel as AsymmetricKeyViewModel;

                    if (asymmetricKey is null)
                        return;

                    switch (field)
                    {
                        case FieldToCopy.AsymmetricKeyPublicKey:
                            Clipboard.SetText(asymmetricKey.PublicKey!);
                            break;
                        case FieldToCopy.AsymmetricKeyPrivateKey:
                            Clipboard.SetText(asymmetricKey.PrivateKey!);
                            break;
                        default:
                            break;
                    }
                }
                ,
                _ => () => throw new Exception("Выбранный формат не поддерживается")
            };

            action?.Invoke();

            PasswordMenuPopup.HideCommand.Execute(null);
        }

        #endregion

        // =================Tag======================

        #region Команда [CreateTagCommand]: Создает тэг

        [RelayCommand(CanExecute = nameof(CanCreateTag))]
        private async Task CreateTag()
        {
            var result = await _tagService.CreateAsync(new CreateTagRequest(NameTag!, Helpers.ColorConverter.RgbToHex(int.Parse(Red), int.Parse(Green), int.Parse(Blue))));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            Tags.Add(new TagViewModel(new TagResponse(result.Value.TagId, _userContext.Id, NameTag!, Helpers.ColorConverter.RgbToHex(int.Parse(Red), int.Parse(Green), int.Parse(Blue)))));

            NameTag = string.Empty;
        }

        private bool CanCreateTag() => !string.IsNullOrWhiteSpace(NameTag);

        #endregion

        #region Команда [DeleteTagCommand]: Удаляет тэг

        [RelayCommand]
        private async Task DeleteTag()
        {
            var result = await _tagService.DeleteAsync(SelectedTag!.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            Tags.Remove(SelectedTag);
        }

        #endregion

        #region Команда [UpdateTagCommand]: Обновляет изменение в тэге

        [RelayCommand]
        private async Task UpdateTag()
        {
            var result = await _tagService.UpdateAsync(new UpdateTagRequest(SelectedTag!.Id, SelectedTag.TagName!, SelectedTag.HexColor));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                SelectedTag.RevertChanges();
                return;
            }

            SelectedTag.CommitChanges(new TagResponse(SelectedTag.Id, _userContext.Id, SelectedTag.TagName!, SelectedTag.HexColor));
        }

        #endregion

        // ==============SideMenu====================

        #region Команда [SetLeftSideMenuControlCommand]: Отвечает за выбор текущего отображаемого контрола на левой боковой панели

        [RelayCommand(CanExecute = nameof(CanSetLeftSideMenuControl))]
        private void SetLeftSideMenuControl(UserControlsName controlName) => CurrentDisplayUserControlLeftSideMenu = controlName;

        private bool CanSetLeftSideMenuControl(UserControlsName controlsName) => CurrentDisplayUserControlLeftSideMenu != controlsName;

        #endregion

        #region Команда [SetRightSideMenuActionCommand]: Отвечает за выбор текущего действия на правой боковой панели

        [RelayCommand(CanExecute = nameof(CanSetRightSideMenuAction))]
        private void SetRightSideMenuAction(ActionOnData action) => CurrentActionRightSideMenu = action;

        private bool CanSetRightSideMenuAction(ActionOnData action) => CurrentActionRightSideMenu != action;

        #endregion

        // ==============TopMenu====================

        #region Команда [ReloadVaultsCommand]: Отвечает за повторную загрузку списка с паролями

        [RelayCommand]
        private async Task ReloadVaults()
        {
            Passwords.Clear();
            await GetEncryptedOverview();
            SelectedEncryptedOverview = null;
            SetRightSideMenuAction(ActionOnData.Create);
            SelectedPasswordType = PasswordTypes.FirstOrDefault(pt => pt.Key == VaultType.Password);
            CreateViewModelForType(VaultType.Password, null!);
        } 

        #endregion

        // ====================================================================================
        //                                      МЕТОДЫ                                        
        // ====================================================================================

        #region Получение данных (API)

        public async Task GetEncryptedOverview()
        {
            var result = await _vaultService.GetAllAsync();

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            foreach (var encrypted in result.Value)
            {
                var encryptedVm = new CredentialsVaultViewModel(encrypted, _cryptoServices, _userContext.Dek, Tags)
                {
                    Icon = Icons.FirstOrDefault(i => i.Id == encrypted.IconId)?.Icon
                };

                if (encrypted.IsArchive)
                {
                    ArchivedPasswords.Add(encryptedVm);
                    continue;
                }

                if (encrypted.IsInTrash)
                {
                    TrashPasswords.Add(encryptedVm);
                    continue;
                }

                Passwords.Add(encryptedVm);
            }
        }

        public async Task GetTags()
        {
            var result = await _tagService.GetAll(); 

            if (result.IsFailure)
                return;

            foreach (var item in result.Value)
            {
                Tags.Add(new TagViewModel(item));
            }
        }

        public async Task GetIcons()
        {
            Result<List<IconMetadataResponse>> iconMetaDataResult = await _assetClient.GetFilesMetadata();

            if (iconMetaDataResult.IsFailure)
            {
                MessageBox.Show(iconMetaDataResult.StringMessage);
                return;
            }

            List<IconMetadataResponse> iconMetadatas = iconMetaDataResult.Value;
            List<FileRequest> fileRequests = [];

            foreach (var metadata in iconMetadatas)
                fileRequests.Add(new FileRequest("crossdyne-assets", metadata.S3Key.FolderPath, metadata.S3Key.Name));

            Result<BatchUrlResponse> urlsResult = await _fileService.GetUrls(new BatchUrlRequest(fileRequests, null));

            foreach (var url in urlsResult.Value.Urls)
            {
                var metaData = iconMetadatas.FirstOrDefault(im => im.S3Key.Name == url.Key);

                if (metaData == null)
                    continue;

                var iconCategory = IconCategories.FirstOrDefault(ic => ic.CategoryId == metaData.CategoryId);

                var iconVm = new IconViewModel(await ImageHelper.LoadSvgFromUrlAsync(url.Url), metaData.S3Key.Key, metaData.AssetId, metaData.AssetName, iconCategory);
                Icons.Add(iconVm);
            }

            UpdateIconsView();
        }

        public async Task GetIconCategories()
        {
            var result = await _iconCategoryService.GetIconCategories();
            
            if (result.IsFailure)
                return;

            foreach (var item in result.Value)
                IconCategories.Add(item);
        }

        #endregion

        #region ViewModels

        private void CreateViewModelForType(VaultType type, CredentialsVaultViewModel encryptedVm)
        {
            var encrypted = encryptedVm;

            encrypted ??= new(new EncryptedVaultResponse(string.Empty, SelectedPasswordType.Key.ToString(), DateTime.UtcNow, null, null, false, false, false, string.Empty, string.Empty, [], ""), _cryptoServices, _userContext.Dek, Tags);

            SelectedCredentialItemBaseViewModel = type switch
            {
                VaultType.Password => new StandardPasswordViewModel(encrypted),
                VaultType.Server => new ServerPasswordViewModel(encrypted),
                VaultType.ApiKey => new ApiKeyViewModel(encrypted),
                VaultType.CreditCard => new CreditCardViewModel(encrypted),
                VaultType.ConnectionString => new ConnectionStringViewModel(encrypted),
                VaultType.AsymmetricKey => new AsymmetricKeyViewModel(encrypted),
                _ => null,
            };
        }

        private void SetReadOnly(ActionOnData action)
        {
            if (action == ActionOnData.View)
                SelectedCredentialItemBaseViewModel?.SetIsReadOnly(true);
            else
                SelectedCredentialItemBaseViewModel?.SetIsReadOnly(false);
        }

        #endregion

        #region Управление правым боковым меню

        [ObservableProperty]
        private double _rightSidebarWidth = 250;

        [ObservableProperty]
        private bool _isSidebarOpen;

        public void ToggleSidebar()
        {
            IsSidebarOpen = !IsSidebarOpen;

            if (IsSidebarOpen)
                RightSidebarWidth = 250;
            else
                RightSidebarWidth = 0;
        }

        #endregion

        #region ICollectionView 

        private void UpdateIconsView()
        {
            IconView.SortDescriptions.Clear();
            IconView.GroupDescriptions.Clear();

            IconView.SortDescriptions.Add(new SortDescription(nameof(IconViewModel.IconCategoryName), ListSortDirection.Ascending));
            IconView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(IconViewModel.IconCategoryName)));
        }

        private void UpdateIconCategoriesView()
        {
            IconCategoryView.SortDescriptions.Clear();

            IconCategoryView.SortDescriptions.Add(new SortDescription(nameof(IconCategoryResponse.Name), ListSortDirection.Ascending));
        }

        #endregion

        #region Взаимодействие с ICollectionView

        private void UpdateGroupingPassword()
        {
            if (PasswordsView == null)
                return;

            PasswordsView.GroupDescriptions.Clear();
            PasswordsView.SortDescriptions.Clear();

            var sorting = SelectedSorting == SortingView.None
                ? (ListSortDirection?)null
                : SelectedSorting == SortingView.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending;

            Action action = SelectedGrouping switch
            {
                GroupingView.Name => () =>
                {
                    PasswordsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CredentialsVaultViewModel.ServiceNameFirstLetter)));
                    if (sorting.HasValue)
                        PasswordsView.SortDescriptions.Add(new SortDescription(nameof(CredentialsVaultViewModel.ServiceNameFirstLetter), sorting.Value));
                }
                ,
                GroupingView.Add => () =>
                {
                    PasswordsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CredentialsVaultViewModel.DateOnlyAdd)));
                    if (sorting.HasValue)
                        PasswordsView.SortDescriptions.Add(new SortDescription(nameof(CredentialsVaultViewModel.DateOnlyAdd), sorting.Value));
                }
                ,
                GroupingView.Update => () =>
                {
                    PasswordsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CredentialsVaultViewModel.DateOnlyUpdate)));
                    if (sorting.HasValue)
                        PasswordsView.SortDescriptions.Add(new SortDescription(nameof(CredentialsVaultViewModel.DateOnlyUpdate), sorting.Value));
                }
                ,
                GroupingView.VaultType => () =>
                {
                    PasswordsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CredentialsVaultViewModel.VaultTypeString)));
                    if (sorting.HasValue)
                        PasswordsView.SortDescriptions.Add(new SortDescription(nameof(CredentialsVaultViewModel.VaultTypeString), sorting.Value));
                }
                ,
                GroupingView.Tag => () =>
                {
                    PasswordsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CredentialsVaultViewModel.FirstTagName)));
                    if (sorting.HasValue)
                        PasswordsView.SortDescriptions.Add(new SortDescription(nameof(CredentialsVaultViewModel.FirstTagName), sorting.Value));
                }
                ,
                GroupingView.None or _ => () =>
                {
                    if (sorting.HasValue)
                        PasswordsView.SortDescriptions.Add(new SortDescription(nameof(CredentialsVaultViewModel.DateAdded), sorting.Value));
                }
                ,
            };

            action?.Invoke();

            PasswordsView.Refresh();

            if (PasswordsView.CurrentItem != null)
                PasswordsView.MoveCurrentToFirst();
        }

        #endregion

    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using EnigmaVault.Desktop.ViewModels.Base;
using Shared.Contracts.SecretService.Responses;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace EnigmaVault.Desktop.ViewModels.Common.Organization
{
    // , Action<FolderViewModel> onSelected
    public sealed partial class FolderViewModel(FolderResponse model) : BaseViewModel
    {
        private FolderResponse _model = model;

        public string Id => _model.Id;
        public string UserId => _model.UserId;
        public string? ParentFolderId => _model.ParentFolderId;

        public ObservableCollection<FolderViewModel> SubFolders { get; } = new();

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private string _folderName = model.FolderName;

        [ObservableProperty]
        private SolidColorBrush? _color = new(Helpers.ColorConverter.HexToRgb(model.Color));

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasChanges))]
        private string _hexColor = model.Color;

        [ObservableProperty]
        public Color _rgbColor = Helpers.ColorConverter.HexToRgb(model.Color);

        public bool HasChanges => _model.Color != HexColor;

    }
}
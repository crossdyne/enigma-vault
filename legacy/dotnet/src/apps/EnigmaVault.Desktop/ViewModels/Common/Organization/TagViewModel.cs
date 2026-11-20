using CommunityToolkit.Mvvm.ComponentModel;
using EnigmaVault.Desktop.ViewModels.Base;
using Shared.Contracts.SecretService.Responses;
using System.Windows.Media;
using ColorConverter = EnigmaVault.Desktop.Helpers.ColorConverter;

namespace EnigmaVault.Desktop.ViewModels.Common.Organization
{
    public sealed partial class TagViewModel(TagResponse model) : BaseViewModel
    {
        private TagResponse _model = model;

        public string Id => _model.Id;
        public string UserId => _model.UserId;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasChanges))]
        private string? _tagName = model.Name;

        [ObservableProperty]
        private SolidColorBrush? _color = new(ColorConverter.HexToRgb(model.Color));

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasChanges))]
        private string _hexColor = model.Color;

        [ObservableProperty]
        public Color _rgbColor = ColorConverter.HexToRgb(model.Color);

        [ObservableProperty]
        private bool _isAttached;

        public bool HasChanges => _model.Name != TagName || _model.Color != HexColor;

        public void RevertChanges()
        {
            TagName = _model.Name;

            HexColor = _model.Color;
            RgbColor = ColorConverter.HexToRgb(_model.Color);
            Color = new SolidColorBrush(ColorConverter.HexToRgb(_model.Color));
        }

        public void CommitChanges(TagResponse newModel)
        {
            _model = newModel ?? throw new ArgumentNullException(nameof(newModel));
            OnPropertyChanged(string.Empty);
        }

        public TagResponse ToModel()
            => _model with
            {
                Name = TagName!,
                Color = HexColor,
            };

        public void SetColor(Color color)
        {
            RgbColor = color;
            HexColor = ColorConverter.RgbToHex(color.R, color.G, color.B);
            Color = new SolidColorBrush(color);
        }

        public void AttachedTag()
        {
            IsAttached = true;
        }

        public void DetachedTag()
        {
            IsAttached = false;
        }
    }
}
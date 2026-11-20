using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.ViewModels.Base;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace EnigmaVault.Desktop.ViewModels.Common.Controls
{
    public sealed partial class PopupController : BaseViewModel
    {
        private readonly Func<bool>? _condition;

        public PopupController(PopupPlacementMode placementMode = PopupPlacementMode.Default, PlacementMode placement = PlacementMode.Left, Func<bool>? condition = null)
        {
            CurrentPlacement = placement;

            if (placementMode == PopupPlacementMode.CustomRightUp)
                CustomPlacementCallback = PlacePopupRightUp;

            if (placementMode == PopupPlacementMode.CustomCenter)
                CustomPlacementCallback = PlacePopupCenter;

            if (condition != null)
                _condition = condition;
        }

        /*--Свойства--------------------------------------------------------------------------------------*/

        [ObservableProperty]
        private bool _isOpen;

        [ObservableProperty]
        private bool _staysOpen = true;

        [ObservableProperty]
        private UIElement? _placementTarget;

        [ObservableProperty]
        private PlacementMode _currentPlacement;

        /*--Команды---------------------------------------------------------------------------------------*/

        [RelayCommand(CanExecute = nameof(CanShow))]
        private void Show(UIElement target)
        {
            PlacementTarget = target;
            IsOpen = true;
        }

        [RelayCommand]
        public void ShowAtMouse()
        {
            CurrentPlacement = PlacementMode.MousePoint;
            PlacementTarget = null;
            IsOpen = true;
        }

        private bool CanShow(UIElement? target = null)
        {
            if (_condition is null) return true;
            return _condition();
        }

        [RelayCommand]
        private void Hide()
        {
            IsOpen = false;
            PlacementTarget = null;
        }

        /*--Дополнительная логика-------------------------------------------------------------------------*/

        public void UpdateCanExecute() => ShowCommand.NotifyCanExecuteChanged();

        [ObservableProperty]
        private double _height = double.NaN;

        [ObservableProperty]
        private double _maxHeight = double.PositiveInfinity;

        public CustomPopupPlacementCallback? CustomPlacementCallback { get; private set; }

        public void UpdatePopupSize(double containerActualHeight, double contentActualHeight = 0)
        {
            if (containerActualHeight > 100)
                MaxHeight = containerActualHeight - 100;

            if (contentActualHeight > 0)
                Height = contentActualHeight + 100;
        }

        private CustomPopupPlacement[] PlacePopupRightUp(Size popupSize, Size targetSize, Point offset)
        {
            double xOffset = targetSize.Width;
            double pHeight = popupSize.Height > 0 ? popupSize.Height : 200;

            double x = xOffset + 10;
            double y = (0 - pHeight) + 25;

            return [new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Vertical)];
        }

        private CustomPopupPlacement[] PlacePopupCenter(Size popupSize, Size targetSize, Point offset)
        {
            double x = (targetSize.Width - popupSize.Width) / 2;
            double y = (targetSize.Height - popupSize.Height) / 2;

            return [new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.None)];
        }
    }
}
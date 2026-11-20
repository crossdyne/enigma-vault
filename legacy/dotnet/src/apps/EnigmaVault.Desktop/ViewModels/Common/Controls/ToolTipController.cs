using CommunityToolkit.Mvvm.ComponentModel;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.ViewModels.Base;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace EnigmaVault.Desktop.ViewModels.Common.Controls
{
    public sealed partial class ToolTipController : BaseViewModel
    {
        //private const double PlacementOffset = 8.0;

        public ToolTipController(ToolTipPlacement placement, double horizontalOffset = 0, double verticalOffset = 0)
        {
            HorizontalOffset = horizontalOffset;
            VerticalOffset = verticalOffset;

            CustomPlacementCallback = placement switch
            {
                ToolTipPlacement.CenterLeft => PlaceCenterLeft,
                ToolTipPlacement.CenterRight => PlaceCenterRight,
                ToolTipPlacement.CenterTop => PlaceCenterTop,
                ToolTipPlacement.CenterBottom => PlaceCenterBottom,
                _ => throw new ArgumentOutOfRangeException(nameof(placement), "Неизвестный режим размещения ToolTip.")
            };
        }

        [ObservableProperty]
        private bool _isOpen;

        [ObservableProperty]
        private object? _content;

        [ObservableProperty]
        private double _horizontalOffset;

        [ObservableProperty]
        private double _verticalOffset;

        public CustomPopupPlacementCallback CustomPlacementCallback { get; }

        private CustomPopupPlacement[] PlaceCenterRight(Size toolTipSize, Size targetSize, Point offset)
        {
            var position = new Point(
                targetSize.Width  + HorizontalOffset,
                (targetSize.Height - toolTipSize.Height) / 2 + (VerticalOffset)
            );
            return [new CustomPopupPlacement(position, PopupPrimaryAxis.Horizontal)];
        }

        private CustomPopupPlacement[] PlaceCenterLeft(Size toolTipSize, Size targetSize, Point offset)
        {
            var position = new Point(-toolTipSize.Width + HorizontalOffset, (targetSize.Height - toolTipSize.Height) / 2 + (VerticalOffset));
            return [new CustomPopupPlacement(position, PopupPrimaryAxis.Horizontal)];
        }

        private CustomPopupPlacement[] PlaceCenterTop(Size toolTipSize, Size targetSize, Point offset)
        {
            var position = new Point((targetSize.Width - toolTipSize.Width) / 2 + (HorizontalOffset), -toolTipSize.Height + VerticalOffset);
            return [new CustomPopupPlacement(position, PopupPrimaryAxis.Vertical)];
        }

        private CustomPopupPlacement[] PlaceCenterBottom(Size toolTipSize, Size targetSize, Point offset)
        {
            var position = new Point((targetSize.Width - toolTipSize.Width) / 2 + (HorizontalOffset), targetSize.Height + VerticalOffset);
            return [new CustomPopupPlacement(position, PopupPrimaryAxis.Vertical)];
        }
    }
}
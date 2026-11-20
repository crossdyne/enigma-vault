using System.Windows;
using System.Windows.Input;

namespace EnigmaVault.Desktop.Resources.Attachable
{
    public static class ControlHelper
    {
        #region Клик на ПКМ

        public static readonly DependencyProperty RightClickCommandProperty =
            DependencyProperty.RegisterAttached(
                "RightClickCommand",
                typeof(ICommand),
                typeof(ControlHelper), 
                new PropertyMetadata(null));

        public static void SetRightClickCommand(DependencyObject element, ICommand value) => element.SetValue(RightClickCommandProperty, value);

        public static ICommand GetRightClickCommand(DependencyObject element) => (ICommand)element.GetValue(RightClickCommandProperty);

        #endregion

        #region Скругление

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached(
                "CornerRadius", 
                typeof(CornerRadius), 
                typeof(ControlHelper), 
                new PropertyMetadata(new CornerRadius(0)));

        public static void SetCornerRadius(DependencyObject element, CornerRadius value) => element.SetValue(CornerRadiusProperty, value);

        public static CornerRadius GetCornerRadius(DependencyObject element) => (CornerRadius)element.GetValue(CornerRadiusProperty);

        #endregion
    }
}
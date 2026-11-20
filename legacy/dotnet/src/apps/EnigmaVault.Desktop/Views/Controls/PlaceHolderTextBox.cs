using System.Windows;
using System.Windows.Controls;

namespace EnigmaVault.Desktop.Views.Controls
{
    public class PlaceHolderTextBox : TextBox
    {
        public string PlaceHolder
        {
            get => (string)GetValue(PlaceHolderProperty);
            set => SetValue(PlaceHolderProperty, value);
        }

        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register("PlaceHolder", typeof(string), typeof(PlaceHolderTextBox), new PropertyMetadata(default));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(PlaceHolderTextBox), new PropertyMetadata(default));

        static PlaceHolderTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceHolderTextBox), new FrameworkPropertyMetadata(typeof(PlaceHolderTextBox)));
        }
    }
}
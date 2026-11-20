using System.Windows;
using System.Windows.Controls;

namespace EnigmaVault.Desktop.Resources.Attachable
{
    internal static class ButtonHelper
    {
        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.RegisterAttached(
                "IconTemplate",
                typeof(ControlTemplate),
                typeof(ButtonHelper),
                new PropertyMetadata(null));

        public static void SetIconTemplate(DependencyObject element, ControlTemplate value)
        {
            element.SetValue(IconTemplateProperty, value);
        }

        public static ControlTemplate GetIconTemplate(DependencyObject element)
        {
            return (ControlTemplate)element.GetValue(IconTemplateProperty);
        }
    }
}
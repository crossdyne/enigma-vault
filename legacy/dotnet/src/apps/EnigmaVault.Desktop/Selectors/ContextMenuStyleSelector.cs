using System.Windows;
using System.Windows.Controls;

namespace EnigmaVault.Desktop.Selectors
{
    public class ContextMenuStyleSelector : StyleSelector
    {
        public Style? MenuItemStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is MenuItem)
                return MenuItemStyle!;

            return base.SelectStyle(item, container);
        }
    }
}
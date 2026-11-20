using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EnigmaVault.Desktop.Resources.Behaviors
{
    public class TabControlWheelScrollBehavior : Behavior<TabControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewMouseWheel += OnTabControlPreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (this.AssociatedObject != null)
                this.AssociatedObject.PreviewMouseWheel -= OnTabControlPreviewMouseWheel;
        }

        private void OnTabControlPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is not TabControl tabControl)
                return;

            var scrollViewer = FindVisualChild<ScrollViewer>(tabControl);
            if (scrollViewer == null)
                return;

            var sourceElement = e.OriginalSource as DependencyObject;
            if (!IsDescendantOf(sourceElement!, scrollViewer))
                return;

            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
            e.Handled = true;
        }

        private static bool IsDescendantOf(DependencyObject child, DependencyObject parent)
        {
            if (child == null || parent == null)
                return false;

            DependencyObject current = child;
            while (current != null)
            {
                if (current == parent)
                    return true;

                current = VisualTreeHelper.GetParent(current);
            }
            return false;
        }

        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                return null!;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild)
                    return typedChild;

                var result = FindVisualChild<T>(child);

                if (result != null)
                    return result;
            }

            return null!;
        }
    }
}
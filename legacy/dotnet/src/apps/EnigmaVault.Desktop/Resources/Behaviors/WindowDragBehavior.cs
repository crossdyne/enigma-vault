using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace EnigmaVault.Desktop.Resources.Behaviors
{
    internal class WindowDragBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
                AssociatedObject.MouseDown += AssociatedObject_MouseDown;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
                AssociatedObject.MouseDown -= AssociatedObject_MouseDown;

            base.OnDetaching();
        }

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                if (AssociatedObject is Window window)
                    window.DragMove();
        }
    }
}
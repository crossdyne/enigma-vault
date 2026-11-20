using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace EnigmaVault.Desktop.Resources.Behaviors
{
    public class InvokeCommandWithHandled : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(InvokeCommandWithHandled), new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter", typeof(object), typeof(InvokeCommandWithHandled), new PropertyMetadata(null));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (Command != null && Command.CanExecute(CommandParameter))
                Command.Execute(CommandParameter);

            if (parameter is RequestNavigateEventArgs e)
                e.Handled = true;
        }
    }
}
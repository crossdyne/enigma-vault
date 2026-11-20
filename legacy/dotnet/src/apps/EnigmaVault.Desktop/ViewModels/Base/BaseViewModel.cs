using CommunityToolkit.Mvvm.ComponentModel;

namespace EnigmaVault.Desktop.ViewModels.Base
{
    public abstract partial class BaseViewModel : ObservableObject
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private bool _isInitialize = false;
        public bool IsInitialize
        {
            get => _isInitialize;
            set => SetProperty(ref _isInitialize, value);
        }
    }
}
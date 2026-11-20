using EnigmaVault.Desktop.Enums;

namespace EnigmaVault.Desktop.ViewModels.Base
{
    public interface IUpdatable
    {
        void Update<TData>(TData value, TransmittingParameter parameter);
    }
}
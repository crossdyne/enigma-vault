using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.ViewModels.Pages;
using EnigmaVault.Desktop.Views.Pages;
using System.Windows.Controls;

namespace EnigmaVault.Desktop.Factories.Pages
{
    internal sealed class NotePageFactory(Func<NotePageViewModel> vmFactory) : IPageFactory
    {
        private readonly Func<NotePageViewModel> _vmFactory = vmFactory;

        public PagesName PageName => PagesName.Note;

        public Page CreatePage() => new NotePage() { DataContext = _vmFactory() };
    }
}
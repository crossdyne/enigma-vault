using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Desktop.ViewModels.Common.Controls;
using EnigmaVault.Desktop.ViewModels.Common.Organization;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnigmaVault.Desktop.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для FoldersControl.xaml
    /// </summary>
    public partial class FoldersControl : UserControl
    {
        public FoldersControl()
        {
            InitializeComponent();
            SelectAndShowContextMenuPopupCommand = new RelayCommand<FolderViewModel>(SelectAndShowСontextMenuPopup);
        }

        public PopupController ContextMenuPopupController { get; } = new();

        #region Команда [SelectAndShowPasswordMenuPopup]: Отвечает за выбор элемента списка при открытие контекстного меню 

        public RelayCommand<FolderViewModel> SelectAndShowContextMenuPopupCommand { get; private set; }

        private void SelectAndShowСontextMenuPopup(FolderViewModel? password)
        {
            if (password is null) return;

            SelectedFolder = password;

            ContextMenuPopupController.ShowAtMouse();
        }

        #endregion

        #region FoldersProperty

        public static readonly DependencyProperty FoldersProperty =
            DependencyProperty.Register(
                nameof(Folders),
                typeof(ObservableCollection<FolderViewModel>),
                typeof(FoldersControl),
                new FrameworkPropertyMetadata(new ObservableCollection<FolderViewModel>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObservableCollection<FolderViewModel> Folders
        {
            get => (ObservableCollection<FolderViewModel>)GetValue(FoldersProperty);
            set => SetValue(FoldersProperty, value);
        }

        #endregion

        #region SelectedFolderProperty

        public static readonly DependencyProperty SelectedFolderProperty =
            DependencyProperty.Register(
                nameof(SelectedFolder),
                typeof(FolderViewModel),
                typeof(FoldersControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public FolderViewModel SelectedFolder
        {
            get => (FolderViewModel)GetValue(SelectedFolderProperty);
            set => SetValue(SelectedFolderProperty, value);
        }

        #endregion

        #region CreateFolderCommand, OnTagNameChanged

        public static readonly DependencyProperty CreateFolderCommandProperty =
            DependencyProperty.Register(
                nameof(CreateFolderCommand),
                typeof(ICommand),
                typeof(FoldersControl),
                new PropertyMetadata(null));

        public ICommand CreateFolderCommand
        {
            get => (ICommand)GetValue(CreateFolderCommandProperty);
            set => SetValue(CreateFolderCommandProperty, value);
        }

        #endregion

        #region DeleteFolderCommand

        public static readonly DependencyProperty DeleteFolderCommandProperty =
            DependencyProperty.Register(
                nameof(DeleteFolderCommand),
                typeof(ICommand),
                typeof(FoldersControl),
                new PropertyMetadata(null));

        public ICommand DeleteFolderCommand
        {
            get => (ICommand)GetValue(DeleteFolderCommandProperty);
            set => SetValue(DeleteFolderCommandProperty, value);
        }

        #endregion

        #region SaveFolderCommand

        public static readonly DependencyProperty SaveFolderCommandProperty =
            DependencyProperty.Register(
                nameof(SaveFolderCommand),
                typeof(ICommand),
                typeof(FoldersControl),
                new PropertyMetadata(null));

        public ICommand SaveFolderCommand
        {
            get => (ICommand)GetValue(SaveFolderCommandProperty);
            set => SetValue(SaveFolderCommandProperty, value);
        }

        #endregion
    }
}
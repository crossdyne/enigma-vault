using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.ViewModels.Common.Controls;
using EnigmaVault.Desktop.ViewModels.Common.Organization;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace EnigmaVault.Desktop.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для TagsControl.xaml
    /// </summary>
    public partial class TagsControl : UserControl
    {
        public TagsControl() => InitializeComponent();

        // public event Action? OnIsPopupOpen;

        public ToolTipController TopToolTipController { get; } = new(ToolTipPlacement.CenterTop);
        public ToolTipController RightToolTipListViewItemController { get; } = new(ToolTipPlacement.CenterRight, horizontalOffset: 10);

        public PopupController PalettePopupController { get; } = new(PopupPlacementMode.CustomBottom);
        public PopupController EditPopupController { get; } = new(PopupPlacementMode.CustomRightUp, PlacementMode.Bottom);

        #region TagsProperty

        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register(
                nameof(Tags),
                typeof(ObservableCollection<TagViewModel>),
                typeof(TagsControl),
                new FrameworkPropertyMetadata(new ObservableCollection<TagViewModel>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObservableCollection<TagViewModel> Tags
        {
            get => (ObservableCollection<TagViewModel>)GetValue(TagsProperty);
            set => SetValue(TagsProperty, value);
        }

        #endregion

        #region SelectedTagProperty

        public static readonly DependencyProperty SelectedTagProperty =
            DependencyProperty.Register(
                nameof(SelectedTag),
                typeof(TagViewModel),
                typeof(TagsControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public TagViewModel SelectedTag
        {
            get => (TagViewModel)GetValue(SelectedTagProperty);
            set => SetValue(SelectedTagProperty, value);
        }

        #endregion

        #region TagNameProperty

        public static readonly DependencyProperty TagNameProperty =
            DependencyProperty.Register(
                nameof(TagName),
                typeof(string),
                typeof(TagsControl),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnTagNameChanged));

        public string TagName
        {
            get => (string)GetValue(TagNameProperty);
            set => SetValue(TagNameProperty, value);
        }

        #endregion

        #region CreateTagCommand, OnTagNameChanged

        public static readonly DependencyProperty CreateTagCommandProperty =
            DependencyProperty.Register(
                nameof(CreateTagCommand),
                typeof(ICommand),
                typeof(TagsControl),
                new PropertyMetadata(null));

        public ICommand CreateTagCommand
        {
            get => (ICommand)GetValue(CreateTagCommandProperty);
            set => SetValue(CreateTagCommandProperty, value);
        }

        private static void OnTagNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TagsControl control)
                (control.CreateTagCommand as IRelayCommand)?.NotifyCanExecuteChanged();
        }

        #endregion

        #region DeleteTagCommand

        public static readonly DependencyProperty DeleteTagCommandProperty =
            DependencyProperty.Register(
                nameof(DeleteTagCommand),
                typeof(ICommand),
                typeof(TagsControl),
                new PropertyMetadata(null));

        public ICommand DeleteTagCommand
        {
            get => (ICommand)GetValue(DeleteTagCommandProperty);
            set => SetValue(DeleteTagCommandProperty, value);
        }

        #endregion

        #region SaveTagCommand

        public static readonly DependencyProperty SaveTagCommandProperty =
            DependencyProperty.Register(
                nameof(SaveTagCommand),
                typeof(ICommand),
                typeof(TagsControl),
                new PropertyMetadata(null));

        public ICommand SaveTagCommand
        {
            get => (ICommand)GetValue(SaveTagCommandProperty);
            set => SetValue(SaveTagCommandProperty, value);
        }

        #endregion

        #region R G B

        public static readonly DependencyProperty RedProperty =
            DependencyProperty.Register(
                nameof(Red),
                typeof(string),
                typeof(TagsControl),
                new FrameworkPropertyMetadata(
                    255.ToString(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Red
        {
            get => (string)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }

        public static readonly DependencyProperty GreenProperty =
            DependencyProperty.Register(
                nameof(Green),
                typeof(string),
                typeof(TagsControl),
                new FrameworkPropertyMetadata(
                    255.ToString(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Green
        {
            get => (string)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        public static readonly DependencyProperty BlueProperty =
            DependencyProperty.Register(
                nameof(Blue),
                typeof(string),
                typeof(TagsControl),
                new FrameworkPropertyMetadata(
                    255.ToString(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Blue
        {
            get => (string)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }

        #endregion

        #region Update R G B

        public static readonly DependencyProperty UpdateRedProperty =
            DependencyProperty.Register(
                nameof(UpdateRed),
                typeof(string),
                typeof(TagsControl),
                new FrameworkPropertyMetadata(
                    255.ToString(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string UpdateRed
        {
            get => (string)GetValue(UpdateRedProperty);
            set => SetValue(UpdateRedProperty, value);
        }

        public static readonly DependencyProperty UpdateGreenProperty =
            DependencyProperty.Register(
                nameof(UpdateGreen),
                typeof(string),
                typeof(TagsControl),
                new FrameworkPropertyMetadata(
                    255.ToString(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string UpdateGreen
        {
            get => (string)GetValue(UpdateGreenProperty);
            set => SetValue(UpdateGreenProperty, value);
        }

        public static readonly DependencyProperty UpdateBlueProperty =
            DependencyProperty.Register(
                nameof(UpdateBlue),
                typeof(string),
                typeof(TagsControl),
                new FrameworkPropertyMetadata(
                    255.ToString(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string UpdateBlue
        {
            get => (string)GetValue(UpdateBlueProperty);
            set => SetValue(UpdateBlueProperty, value);
        }

        #endregion
    }
}
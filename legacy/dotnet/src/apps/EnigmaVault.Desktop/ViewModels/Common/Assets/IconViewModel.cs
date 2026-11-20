using CommunityToolkit.Mvvm.ComponentModel;
using EnigmaVault.Desktop.ViewModels.Base;
using Shared.Contracts.AssetsService.Responses;
using System.Windows.Media;

namespace EnigmaVault.Desktop.ViewModels.Common.Assets
{
    internal sealed partial class IconViewModel : BaseViewModel
    {
        public IconViewModel(DrawingImage? icon, string s3Key, string? id, string iconName, IconCategoryResponse? category)
        {
            Id = id;
            Key = s3Key;
            Category = category;
            Name = iconName;
            Icon = icon;

            IconCategoryId = category?.CategoryId;
            IconCategoryName = Category != null ? Category.Name : "Без категории";  
        }

        [ObservableProperty]
        private string? _id;

         [ObservableProperty]
        private string? _key;

         [ObservableProperty]
        private string? _iconCategoryId;

        [ObservableProperty]
        private string? _iconCategoryName;

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private DrawingImage? _icon;

        [ObservableProperty]
        private IconCategoryResponse? _category;

        //[ObservableProperty]
        //private string? _iconCategoryName;

        public void SetIcon(DrawingImage? icon) => Icon = icon;

        public void SetCategory(IconCategoryResponse? category) => Category = category;
    }
}
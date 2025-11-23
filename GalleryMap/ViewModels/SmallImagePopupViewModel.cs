using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Extensions;
using GalleryMap.Services;
using GalleryMap.Views;

namespace GalleryMap.ViewModels
{
    public class SmallImagePopupViewModel
    {
        private readonly IImageService _imageService;

        public ImageSource Image => _imageService.SelectedImage.ImageSource;
        public Command Tap { get; set; }

        public SmallImagePopupViewModel(IImageService imageService)
        {
            _imageService = imageService;
            Tap = new Command(OnTap);
        }
        private async void OnTap()
        {
            await Shell.Current.ClosePopupAsync();
            await Shell.Current.GoToAsync(nameof(ViewImagePage), true);
        }
    }
}

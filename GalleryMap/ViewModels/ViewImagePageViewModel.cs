using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalleryMap.Models;
using GalleryMap.Services;

namespace GalleryMap.ViewModels
{
    public class ViewImagePageViewModel : BaseViewModel
    {
        private readonly IImageService _imageService;
        private readonly IEventService _eventService;
        public ImageLocation SelectedImage => _imageService.SelectedImage;

        public Command GoToImageLocation { get; set; }
        public Command DeleteImage { get; set; }

        public ViewImagePageViewModel(IImageService imageService, IEventService eventService)
        {
            _imageService = imageService;

            GoToImageLocation = new Command(onGoToImageLocation);
            DeleteImage = new Command(onDeleteImage);
            _eventService = eventService;
        }

        async void onGoToImageLocation()
        {
            await Shell.Current.GoToAsync("///MapsPage");

            _eventService.RequestImageLocationNavigation(_imageService.SelectedImage);
        }

        async void onDeleteImage()
        {
            await _imageService.DeleteImageAsync(SelectedImage.Id);

            await Shell.Current.Navigation.PopAsync();
        }
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GalleryMap.Database.Repositories;
using GalleryMap.Models;
using GalleryMap.Services;
using Microsoft.Maui.Controls;

namespace GalleryMap.ViewModels
{
    public partial class AddImagePageViewModel : BaseViewModel
    {
        private readonly IImageService _imageService;
        private readonly ILocationService _locationService;
        private readonly IImageLocationRepository _repository;

        private ImageSource selectedImageSource;
        public ImageSource SelectedImageSource
        {
            get
            {
                return ImageSource.FromStream(() =>
                new MemoryStream(_imageService.CurrentImage));
            }
            set => selectedImageSource = value;
        }
        private Location location;
        public Location Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        public Command GetLocation { get; set; }
        public Command CreateImage { get; set; }

        public AddImagePageViewModel(IImageService imageService, ILocationService locationService, IImageLocationRepository repository)
        {
            _imageService = imageService;
            _locationService = locationService;
            _repository = repository;

            GetLocation = new Command(OnImageLocation);
            CreateImage = new Command(OnCreateImage);
        }

        async void OnImageLocation()
        {
            var location = _locationService.GetLocationAsync();

            if (location != null)
            {
                var loc = await location;
                Location = loc;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Unable to get location.", "OK");
                Location = new Location { Latitude = 0, Longitude = 0 };
            }
        }

        async void OnCreateImage()
        {
            var location = await _locationService.GetLocationAsync();

            ImageLocation imageLocation = new ImageLocation
            {
                ImageUrl = Convert.ToBase64String(_imageService.CurrentImage),
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };

            await _repository.CreateAsync(imageLocation);
        }
    }
}

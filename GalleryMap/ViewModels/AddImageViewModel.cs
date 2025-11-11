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
        private readonly IImageLocationRepository _repository;

        private ImageSource selectedImageSource;
        public ImageSource SelectedImageSource
        {
            get
            {
                return ImageSource.FromStream(() =>
                new MemoryStream(_imageService.AddedImage));
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
                OnPropertyChanged(nameof(AddImageEnabled));
            }
        }

        public bool AddImageEnabled => _imageService.AddedImage != null && Location != null;

        public Command GetLocation { get; set; }
        public Command CreateImage { get; set; }

        public AddImagePageViewModel(IImageService imageService, IImageLocationRepository repository)
        {
            _imageService = imageService;
            _repository = repository;

            GetLocation = new Command(OnImageLocation);
            CreateImage = new Command(OnCreateImage);
        }

        async void OnImageLocation()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                try
                {
                    var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(10)
                    });
                    Location = location;
                }
                catch (FeatureNotEnabledException)
                {
                    var result = await Shell.Current.DisplayAlert(
                        "Location Services Disabled",
                        "Please enable location services in your device settings.",
                        "Open Settings",
                        "Cancel");

                    if (result)
                    {
                        AppInfo.ShowSettingsUI();
                    }
                    Location = new Location { Latitude = 0, Longitude = 0 };
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Unable to get location.", "OK");
                Location = new Location { Latitude = 0, Longitude = 0 };
            }
        }

        async void OnCreateImage()
        {
            ImageLocation imageLocation = new ImageLocation
            {
                ImageUrl = Convert.ToBase64String(_imageService.AddedImage),
                Latitude = Location.Latitude,
                Longitude = Location.Longitude
            };

            await _repository.CreateAsync(imageLocation);
            await Shell.Current.GoToAsync("..");
        }
    }
}

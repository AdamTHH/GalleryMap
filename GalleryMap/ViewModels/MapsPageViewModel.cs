using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using GalleryMap.Database.Repositories;
using GalleryMap.Models;
using GalleryMap.Services;
using GalleryMap.Views;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace GalleryMap.ViewModels
{
    public class MapsPageViewModel : BaseViewModel
    {
        private readonly IEventService _eventService;
        private readonly IImageService _imageService;

        
        private Map map;
        public Map Map
        {
            get { return map; }
            set
            {
                map = value;
                OnPropertyChanged(nameof(Map));
            }
        }

        public MapsPageViewModel(IImageService imageService, IEventService eventService)
        {
            _eventService = eventService;
            _imageService = imageService;
            Map = new Map
            {
                IsShowingUser = true,
            };

            _ = InitializeMapAsync();

            Shell.Current.Navigated += OnNavigated;
            _eventService.ImageLocationRequested += ImageLocationRequested;
        }

        private void ImageLocationRequested(object? sender, ImageLocation e)
        {
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Location(e.Latitude, e.Longitude),
                    Distance.FromMiles(1)));
        }

        private async Task InitializeMapAsync()
        {
            var images = await _imageService.LoadImagesAsync();

            foreach (var pin in Map.Pins)
            {
                pin.MarkerClicked -= Pin_MarkerClicked;
            }

            Map.Pins.Clear();

            foreach (var image in images)
            {
                var pin = new Pin
                {
                    Location = new Location { Latitude = image.Latitude, Longitude = image.Longitude },
                    Address = "asd",
                    
                    Label = image.Id.ToString(),
                    AutomationId = image.Id.ToString()
                };

                pin.MarkerClicked += Pin_MarkerClicked;
                Map.Pins.Add(pin);
            }

            if (Map.Pins.Count > 0)
            {
                OnPropertyChanged(nameof(Map));
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(Map.Pins[0].Location, Distance.FromKilometers(100)));
            }
        }

        private async void Pin_MarkerClicked(object? sender, PinClickedEventArgs e)
        {
            Pin pin = (Pin)sender;

            _imageService.SelectedImage = await _imageService.ReadAsync(int.Parse(pin.AutomationId));

            var vm = new SmallImagePopupViewModel(_imageService);
            await Shell.Current.ShowPopupAsync(new SmallImagePopup(vm));
        }

        private async void OnNavigated(object? sender, ShellNavigatedEventArgs e)
        {
            if (e.Current.Location.OriginalString.Contains(nameof(MapsPage)))
            {
                await InitializeMapAsync();
            }
        }
    }
}

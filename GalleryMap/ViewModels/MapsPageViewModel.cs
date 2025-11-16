using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GalleryMap.Database.Repositories;
using GalleryMap.Models;
using GalleryMap.Services;
using Kotlin.Properties;
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

            Shell.Current.Navigated += OnNavigated;
            _eventService.ImageLocationRequested += ImageLocationRequested;
        }

        private void ImageLocationRequested(object? sender, ImageLocation e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Location(e.Latitude, e.Longitude),
                    Distance.FromMiles(1)));
            });
        }

        private async void OnNavigated(object? sender, ShellNavigatedEventArgs e)
        {
            if (e.Current.Location.OriginalString.Contains(nameof(MainPage)))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var pins = new List<Pin>();

                    var images = await _imageService.LoadImagesAsync();

                    Map.Pins.Clear();

                    foreach (var image in images)
                    {
                        Map.Pins.Add(new Pin
                        {
                            Location = new Location { Latitude = image.Latitude, Longitude = image.Longitude },
                            Address = image.ImageUrl,
                            Label = image.Id.ToString()
                        });
                    }
                    OnPropertyChanged(nameof(Map));
                    Map.MoveToRegion(MapSpan.FromCenterAndRadius(Map.Pins[0].Location, Distance.FromKilometers(100)));
                });
                
            }
            
        }
    }
}

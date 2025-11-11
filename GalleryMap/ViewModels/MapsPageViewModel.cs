using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Threading.Tasks;
using GalleryMap.Database.Repositories;
using GalleryMap.Models;
using System.Linq;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace GalleryMap.ViewModels
{
    public class MapsPageViewModel : BaseViewModel
    {
        private readonly IImageLocationRepository _imageLocationRepository;
        private Map map;

        public Map Map
        {
            get { return map; }
            set
            {
                map = value;
                OnPropertyChanged("map");
            }
        }


        public MapsPageViewModel(IImageLocationRepository imageLocationRepository)
        {
            _imageLocationRepository = imageLocationRepository;
        }

        private async void LoadAllImageLocationsAsync()
        {
            try
            {
                var imageLocations = await _imageLocationRepository.GetAllAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Map.Pins.Clear();

                    foreach (var imageLocation in imageLocations)
                    {
                        var pin = new Pin
                        {
                            Label = $"Image {imageLocation.Id}",
                            Address = $"Created: {imageLocation.CreatedAt:MMM dd, yyyy}",
                            Location = new Location(imageLocation.Latitude, imageLocation.Longitude),
                            Type = PinType.Place
                        };
                        Map.Pins.Add(pin);
                    }

                    if (imageLocations.Any())
                    {
                        var firstLocation = imageLocations.First();
                        Map.MoveToRegion(MapSpan.FromCenterAndRadius(
                            new Location(firstLocation.Latitude, firstLocation.Longitude),
                            Distance.FromKilometers(10)));
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading image locations: {ex.Message}");
            }
        }

        public async Task RefreshImageLocationsAsync()
        {
            await Task.Run(LoadAllImageLocationsAsync);
        }
    }
}

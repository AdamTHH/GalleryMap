using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Threading.Tasks;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace GalleryMap.ViewModels
{
    public class MapsPageViewModel : BaseViewModel
    {
        public Map Map { get; }

        public MapsPageViewModel()
        {
            Map = new Map(MapSpan.FromCenterAndRadius(new Location(0, 0), Distance.FromKilometers(1000)))
            {
                MapType = MapType.Satellite,
                IsShowingUser = true,
                Margin = new Thickness(0)
            };


        }

        public void MoveTo(double latitude, double longitude, double radiusKm = 1)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(latitude, longitude), Distance.FromKilometers(radiusKm)));
            });
        }

        public void AddPin(double latitude, double longitude, string label = null, string address = null)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var pin = new Pin
                {
                    Label = label,
                    Address = address,
                    Location = new Location(latitude, longitude),
                    Type = PinType.Place
                };
                Map.Pins.Add(pin);
            });
        }
    }
}

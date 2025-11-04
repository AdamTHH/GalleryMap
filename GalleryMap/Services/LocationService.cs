using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryMap.Services
{
    public class LocationService : ILocationService
    {
        public async Task<Location> GetLocationAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (status != PermissionStatus.Granted)
                {
                    return new Location(0, 0);
                    throw new PermissionException("Location permission not granted");
                }

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location != null)
                {
                    return location;
                }

                throw new Exception("Unable to get location");
            }
            catch (FeatureNotSupportedException)
            {
                throw new Exception("Location services not supported on this device");
            }
            catch (PermissionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting location: {ex.Message}", ex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryMap.Services
{
    public interface ILocationService
    {
        Task<Location> GetLocationAsync();
    }
}

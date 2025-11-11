using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Threading.Tasks;
using GalleryMap.Database.Repositories;
using GalleryMap.Models;
using System.Linq;
using System.Collections.Generic;
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
                OnPropertyChanged(nameof(Map));
            }
        }

        public MapsPageViewModel(IImageLocationRepository imageLocationRepository)
        {
            _imageLocationRepository = imageLocationRepository;
        }
    }
}

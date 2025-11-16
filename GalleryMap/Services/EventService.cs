using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalleryMap.Models;

namespace GalleryMap.Services
{
    internal class EventService : IEventService
    {
        public event EventHandler<ImageLocation> ImageLocationRequested;

        public void RequestImageLocationNavigation(ImageLocation imageLocation)
        {
            ImageLocationRequested?.Invoke(this, imageLocation);
        }
    }
}

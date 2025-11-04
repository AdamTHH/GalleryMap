using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryMap.Services
{
    public class ImageService : IImageService
    {
        byte[]? _currentImage;
        public byte[]? CurrentImage
        {
            get => _currentImage;
            set => _currentImage = value;
        }

    }
}

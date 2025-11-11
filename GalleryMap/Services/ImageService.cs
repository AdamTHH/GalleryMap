using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalleryMap.Models;

namespace GalleryMap.Services
{
    public class ImageService : IImageService
    {
        byte[]? addedImage;
        public byte[]? AddedImage
        {
            get => addedImage;
            set => addedImage = value;
        }

        private ImageLocation? selectedImageLocation;
        public ImageLocation? SelectedImageLocation
        {
            get { return selectedImageLocation; }
            set { selectedImageLocation = value; }
        }

    }
}

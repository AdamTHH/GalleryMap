using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalleryMap.Models;

namespace GalleryMap.Services
{
    public interface IImageService
    {
        byte[]? AddedImage { get; set; }
        ImageLocation? SelectedImageLocation { get; set; }
    }
}

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
        ImageLocation? SelectedImage { get; set; }
        List<ImageLocation> Images { get; set; }
        Task<List<ImageLocation>> LoadImagesAsync();
        Task<bool> DeleteAsync(int id);
        Task<ImageLocation> CreateAsync(ImageLocation image);
        Task<ImageLocation> ReadAsync(int id);
        Task<ImageLocation?> UpdateAsync(ImageLocation image);
    }
}

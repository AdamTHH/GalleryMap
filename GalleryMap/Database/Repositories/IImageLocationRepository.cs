using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalleryMap.Models;

namespace GalleryMap.Database.Repositories
{
    public interface IImageLocationRepository
    {
        Task<ImageLocation> CreateAsync(ImageLocation image);
        Task<ImageLocation?> GetByIdAsync(int id);
        Task<List<ImageLocation>> GetAllAsync();
        Task<ImageLocation?> UpdateAsync(ImageLocation image);
        Task<bool> DeleteAsync(int id);
    }
}

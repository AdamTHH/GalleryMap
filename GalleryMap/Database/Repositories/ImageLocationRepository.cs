using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalleryMap.Models;
using Microsoft.EntityFrameworkCore;

namespace GalleryMap.Database.Repositories
{
    public class ImageLocationRepository : IImageLocationRepository
    {
        private readonly GalleryMapDbContext _context;

        public ImageLocationRepository(GalleryMapDbContext context)
        {
            _context = context;
        }

        public async Task<ImageLocation> CreateAsync(ImageLocation image)
        {
            image.CreatedAt = DateTime.UtcNow;
            _context.ImageLocations.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<ImageLocation?> ReadAsync(int id)
        {
            return await _context.ImageLocations.FindAsync(id);
        }
        public async Task<List<ImageLocation>> GetAllAsync()
        {
            return await _context.ImageLocations
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<ImageLocation?> UpdateAsync(ImageLocation image)
        {
            var existing = await _context.ImageLocations.FindAsync(image.Id);
            if (existing == null)
                return null;

            existing.ImageData = image.ImageData;
            existing.Latitude = image.Latitude;
            existing.Longitude = image.Longitude;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var image = await _context.ImageLocations.FindAsync(id);
            if (image == null)
                return false;

            _context.ImageLocations.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

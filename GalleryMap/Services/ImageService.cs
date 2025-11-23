using System.ComponentModel;
using GalleryMap.Database.Repositories;
using GalleryMap.Models;

namespace GalleryMap.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageLocationRepository _imageLocationRepository;
        byte[]? addedImage;
        public byte[]? AddedImage
        {
            get => addedImage;
            set => addedImage = value;
        }
        private ImageLocation? selectedImageLocation;
        public ImageLocation? SelectedImage
        {
            get { return selectedImageLocation; }
            set { selectedImageLocation = value; }
        }
        private List<ImageLocation> images;
        public List<ImageLocation> Images
        {
            get => images;
            set
            {
                images = value;
            }
        }

        public ImageService(IImageLocationRepository imageLocationRepository)
        {
            _imageLocationRepository = imageLocationRepository;

            LoadImagesAsync();
        }

        public async Task<List<ImageLocation>> LoadImagesAsync()
        {
            var images = await _imageLocationRepository.GetAllAsync();
            Images = images;
            return images;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var image = images.First(i => i.Id == id);
            images.Remove(image);
            return await _imageLocationRepository.DeleteAsync(id);
        }

        public async Task<ImageLocation> CreateAsync(ImageLocation image)
        {
            images.Add(image);
            return await _imageLocationRepository.CreateAsync(image);
        }

        public async Task<ImageLocation> ReadAsync(int id)
        {
            return await _imageLocationRepository.ReadAsync(id);
        }

        public async Task<ImageLocation?> UpdateAsync(ImageLocation image)
        {
            return await _imageLocationRepository.UpdateAsync(image);
        }
    }
}

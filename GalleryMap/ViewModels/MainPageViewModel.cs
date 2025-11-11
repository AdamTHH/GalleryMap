using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using GalleryMap.Database.Repositories;
using GalleryMap.Models;
using GalleryMap.Services;
using GalleryMap.Views;

namespace GalleryMap.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly IImageService _imageService;
        private readonly IImageLocationRepository _imageLocationRepository;

        private ObservableCollection<ImageLocation> imageLocations = new();
        public ObservableCollection<ImageLocation> ImageLocations
        {
            get { return imageLocations; }
            set
            {
                imageLocations = value;
                OnPropertyChanged(nameof(ImageLocations));
                OnPropertyChanged(nameof(IsEmpty));
            }
        }
        public bool IsEmpty => ImageLocations == null || ImageLocations.Count == 0;

        public async void LoadImagesAsync()
        {
            var images = await _imageLocationRepository.GetAllAsync();
            ImageLocations = new ObservableCollection<ImageLocation>(images);
        }

        public Command UploadPicture { get; set; }
        public Command<ImageLocation> ImageTapped { get; set; }
        
        public MainPageViewModel(IImageService imageService, IImageLocationRepository imageLocationRepository)
        {
            _imageService = imageService;
            _imageLocationRepository = imageLocationRepository;

            UploadPicture = new Command(OnUploadPicture);
            ImageTapped = new Command<ImageLocation>(OnImageTapped);

            LoadImagesAsync();
        }

        async void OnImageTapped(ImageLocation imageLocation)
        {
            _imageService.SelectedImageLocation = imageLocation;
            
            var popup = new ImagePopup();
            popup.BindingContext = _imageService;
            await Shell.Current.CurrentPage.ShowPopupAsync(popup);
        }

        async void OnUploadPicture()
        {
            var imageData = await PickImageAsync();

            if (imageData != null)
            {
                _imageService.AddedImage = imageData;
                await Shell.Current.GoToAsync(nameof(AddImagePage));
            }
        }

        public async Task<byte[]?> PickImageAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Select an image"
                });

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error picking image: {ex.Message}");
                return null;
            }
        }
    }
}

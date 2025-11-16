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
        private ObservableCollection<ImageLocation> images;
        public ObservableCollection<ImageLocation> Images
        {
            get { return images; }
            set
            {
                images = value;
                OnPropertyChanged(nameof(Images));
                OnPropertyChanged(nameof(IsEmpty));
            }
        }
        public bool IsEmpty => Images == null || Images.Count == 0;
        public Command UploadPicture { get; set; }
        public Command<ImageLocation> ImageTapped { get; set; }
        public MainPageViewModel(IImageService imageService)
        {
            _imageService = imageService;
            Images = new ObservableCollection<ImageLocation>();

            UploadPicture = new Command(OnUploadPicture);
            ImageTapped = new Command<ImageLocation>(OnImageTapped);

            Shell.Current.Navigated += OnNavigated;
        }

        private void OnNavigated(object? sender, ShellNavigatedEventArgs e)
        {
            if (e.Current.Location.OriginalString.Contains(nameof(MainPage)))
            {
                RefreshImages();
            }
        }

        public async void RefreshImages()
        {
            var imageList = _imageService.Images;
            Images = new ObservableCollection<ImageLocation>(imageList.OrderByDescending(i => i.CreatedAt));
        }
        async void OnImageTapped(ImageLocation imageLocation)
        {
            _imageService.SelectedImage = imageLocation;

            await Shell.Current.GoToAsync(nameof(ViewImagePage));
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

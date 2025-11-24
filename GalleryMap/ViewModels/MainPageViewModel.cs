using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using GalleryMap.Database.Repositories;
using GalleryMap.Models;
using GalleryMap.Services;
using GalleryMap.Views;

namespace GalleryMap.ViewModels
{
    public class ImageGroup : ObservableCollection<ImageLocation>
    {
        public string MonthYear { get; set; }

        public ImageGroup(string monthYear, IEnumerable<ImageLocation> images) : base(images)
        {
            MonthYear = monthYear;
        }
    }

    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly IImageService _imageService;
        private ObservableCollection<ImageGroup> groupedImages;

        public ObservableCollection<ImageGroup> GroupedImages
        {
            get { return groupedImages; }
            set
            {
                groupedImages = value;
                OnPropertyChanged(nameof(GroupedImages));
                OnPropertyChanged(nameof(IsEmpty));
            }
        }

        public bool IsEmpty => GroupedImages == null || GroupedImages.Count == 0;

        public Command UploadPictureGallery { get; set; }
        public Command UploadPictureCamera { get; set; }
        public Command<ImageLocation> ImageTapped { get; set; }

        public MainPageViewModel(IImageService imageService)
        {
            _imageService = imageService;
            GroupedImages = new ObservableCollection<ImageGroup>();
            UploadPictureGallery = new Command(OnUploadPictureGallery);
            UploadPictureCamera = new Command(OnUploadPictureCamera);
            ImageTapped = new Command<ImageLocation>(OnImageTapped);
            Shell.Current.Navigated += OnNavigated;
        }

        private void OnNavigated(object? sender, ShellNavigatedEventArgs e)
        {
            if (e.Current.Location.OriginalString.Contains(nameof(MainPage)))
            {
                LoadGroupedImages();
            }
        }

        private async void LoadGroupedImages()
        {
            var images = await _imageService.LoadImagesAsync();
            var imageList = images.OrderByDescending(i => i.CreatedAt);

            var grouped = imageList
                .GroupBy(img => new {
                    Year = img.CreatedAt.Year,
                    Month = img.CreatedAt.Month
                })
                .Select(g => new ImageGroup(
                    g.First().CreatedAt.ToString("yyyy MMMM", new CultureInfo("en-US")),
                    g.ToList()
                ))
                .ToList();

            GroupedImages = new ObservableCollection<ImageGroup>(grouped);
        }

        async void OnImageTapped(ImageLocation imageLocation)
        {
            _imageService.SelectedImage = imageLocation;
            await Shell.Current.GoToAsync(nameof(ViewImagePage), true);
        }

        async void OnUploadPictureGallery()
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

        async void OnUploadPictureCamera()
        {
            var imageData = await TakePhotoAsync();
            if (imageData != null)
            {
                _imageService.AddedImage = imageData;
                await Shell.Current.GoToAsync(nameof(AddImagePage));
            }
        }

        public async Task<byte[]?> TakePhotoAsync()
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var result = await MediaPicker.Default.CapturePhotoAsync();
                    if (result != null)
                    {
                        using var stream = await result.OpenReadAsync();
                        using var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error taking photo: {ex.Message}");
                return null;
            }
        }
    }
}

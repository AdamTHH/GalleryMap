using GalleryMap.Models;
using GalleryMap.Services;
using GalleryMap.Views;

namespace GalleryMap.ViewModels
{
    public class ViewImagePageViewModel : BaseViewModel
    {
        private readonly IImageService _imageService;
        private readonly IEventService _eventService;

        private ImageLocation selectedImage;
        public ImageLocation SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
                if (selectedImage != null)
                {
                    NewTitle = selectedImage.Title;
                }
            }
        }

        private bool wereThereChanges;
        public bool WereThereChanges
        {
            get { return wereThereChanges; }
            set
            {
                wereThereChanges = value;
                OnPropertyChanged(nameof(WereThereChanges));
            }
        }

        private string newTitle;
        public string NewTitle
        {
            get { return newTitle; }
            set
            {
                newTitle = value;
                WereThereChanges = SelectedImage?.Title != value;
                OnPropertyChanged(nameof(NewTitle));
            }
        }

        public Command GoToImageLocation { get; set; }
        public Command DeleteImage { get; set; }
        public Command SaveChanges { get; set; }

        public ViewImagePageViewModel(IImageService imageService, IEventService eventService)
        {
            _imageService = imageService;
            _eventService = eventService;

            GoToImageLocation = new Command(onGoToImageLocation);
            DeleteImage = new Command(onDeleteImage);
            SaveChanges = new Command(onSaveChanges);

            SelectedImage = _imageService.SelectedImage;

            Shell.Current.Navigated += OnNavigated;
        }

        private void OnNavigated(object? sender, ShellNavigatedEventArgs e)
        {
            if (e.Current.Location.OriginalString.Contains(nameof(ViewImagePage)))
            {
                SelectedImage = _imageService.SelectedImage;
            }
        }

        async void onSaveChanges()
        {
            SelectedImage.Title = NewTitle;
            var updatedImage = await _imageService.UpdateAsync(SelectedImage);
            SelectedImage = updatedImage;
            WereThereChanges = false;
        }

        async void onGoToImageLocation()
        {
            await Shell.Current.GoToAsync("///MapsPage");
            _eventService.RequestImageLocationNavigation(SelectedImage);
        }

        async void onDeleteImage()
        {
            await _imageService.DeleteAsync(SelectedImage.Id);
            await Shell.Current.Navigation.PopAsync();
        }
    }
}

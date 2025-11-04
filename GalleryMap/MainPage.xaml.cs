using GalleryMap.ViewModels;

namespace GalleryMap
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel;
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadImagesAsync();
        }
    }
}

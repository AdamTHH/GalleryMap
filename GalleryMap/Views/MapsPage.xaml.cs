using GalleryMap.Database.Repositories;
using GalleryMap.ViewModels;

namespace GalleryMap.Views;

public partial class MapsPage : ContentPage
{
    private readonly MapsPageViewModel _viewModel;

    public MapsPage(MapsPageViewModel mapsPageViewModel)
    {
        InitializeComponent();
        BindingContext = mapsPageViewModel;
    }

}

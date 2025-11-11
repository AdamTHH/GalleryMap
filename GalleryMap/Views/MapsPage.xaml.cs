using GalleryMap.ViewModels;

namespace GalleryMap.Views;

public partial class MapsPage : ContentPage
{
	public MapsPage(MapsPageViewModel mapsPageViewModel)
	{
		InitializeComponent();

        BindingContext = mapsPageViewModel;
    }
}

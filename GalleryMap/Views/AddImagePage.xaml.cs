using GalleryMap.ViewModels;

namespace GalleryMap.Views;

public partial class AddImagePage : ContentPage
{
    public AddImagePage(AddImagePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

using GalleryMap.ViewModels;

namespace GalleryMap.Views;

public partial class ViewImagePage : ContentPage
{
	public ViewImagePage(ViewImagePageViewModel viewImagePageViewModel)
	{
		InitializeComponent();

        BindingContext = viewImagePageViewModel;
    }

    private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Running)
        {
            var image = sender as Image;
            if (image != null)
            {
                image.Scale = Math.Max(1, Math.Min(image.Scale * e.Scale, 3));
            }
        }
    }
}

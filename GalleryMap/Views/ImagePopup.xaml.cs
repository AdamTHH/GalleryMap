using CommunityToolkit.Maui.Views;

namespace GalleryMap.Views;

public partial class ImagePopup : Popup
{
	public ImagePopup()
	{
		InitializeComponent();
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

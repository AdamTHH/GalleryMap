using CommunityToolkit.Maui.Views;
using GalleryMap.ViewModels;

namespace GalleryMap.Views;

public partial class SmallImagePopup : Popup
{
	public SmallImagePopup(SmallImagePopupViewModel smallImagePopupViewModel)
	{
		InitializeComponent();

        BindingContext = smallImagePopupViewModel;
    }
}

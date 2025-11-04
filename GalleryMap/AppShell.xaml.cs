using GalleryMap.Views;

namespace GalleryMap
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddImagePage), typeof(AddImagePage));
            Routing.RegisterRoute(nameof(MapsPage), typeof(MapsPage));
        }
    }
}

using CommunityToolkit.Maui;
using GalleryMap.Database;
using GalleryMap.Database.Repositories;
using GalleryMap.Services;
using GalleryMap.ViewModels;
using GalleryMap.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GalleryMap
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font Awesome 7 Free-Regular-400.otf", "FontAwesomeRegular");
                    fonts.AddFont("Font Awesome 7 Free-Solid-900.otf", "FontAwesomeSolid");
                })
                .UseMauiMaps();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddDbContext<GalleryMapDbContext>();


            builder.Services.AddScoped<MainPageViewModel>();

            builder.Services.AddTransient<AddImagePage>();
            builder.Services.AddTransient<AddImagePageViewModel>();

            builder.Services.AddTransient<MapsPage>();
            builder.Services.AddTransient<MapsPageViewModel>();

            builder.Services.AddSingleton<ViewImagePage>();
            builder.Services.AddSingleton<ViewImagePageViewModel>();

            builder.Services.AddTransient<SmallImagePopup>();
            builder.Services.AddTransient<SmallImagePopupViewModel>();


            builder.Services.AddScoped<IImageLocationRepository, ImageLocationRepository>();

            builder.Services.AddSingleton<IEventService, EventService>();
            builder.Services.AddSingleton<IImageService, ImageService>();


            var app = builder.Build();

            return app;
        }
    }
}

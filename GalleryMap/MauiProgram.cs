using CommunityToolkit.Maui;
using GalleryMap.Database;
using GalleryMap.Database.Repositories;
using GalleryMap.Services;
using GalleryMap.ViewModels;
using GalleryMap.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

#if ANDROID
using AndroidX.Core.View;
#endif

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
                .UseMauiMaps()
                .ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                    events.AddAndroid(android => android
                        .OnCreate((activity, bundle) => MakeStatusBarMatchSystemTheme()));
#endif
                });


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

#if ANDROID
        private static void MakeStatusBarMatchSystemTheme()
        {
            var currentActivity = Platform.CurrentActivity ?? Android.App.Application.Context as AndroidX.AppCompat.App.AppCompatActivity;
            if (currentActivity?.Window != null)
            {
                var window = currentActivity.Window;
                var configuration = currentActivity.Resources?.Configuration;
                
                if (configuration != null)
                {
                    var currentNightMode = configuration.UiMode & Android.Content.Res.UiMode.NightMask;
                    switch (currentNightMode)
                    {
                        case Android.Content.Res.UiMode.NightNo:
                            // Light mode - light status bar with dark content
                            window.SetStatusBarColor(Android.Graphics.Color.White);
                            WindowCompat.GetInsetsController(window, window.DecorView).AppearanceLightStatusBars = true;
                            break;
                        case Android.Content.Res.UiMode.NightYes:
                            // Dark mode - dark status bar with light content  
                            window.SetStatusBarColor(Android.Graphics.Color.Black);
                            WindowCompat.GetInsetsController(window, window.DecorView).AppearanceLightStatusBars = false;
                            break;
                    }
                }
            }
        }
#endif
    }
}

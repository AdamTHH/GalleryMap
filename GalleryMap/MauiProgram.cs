using CommunityToolkit.Maui;
using GalleryMap.Database;
using GalleryMap.Database.Repositories;
using GalleryMap.Services;
using GalleryMap.ViewModels;
using GalleryMap.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace GalleryMap
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font Awesome 7 Free-Regular-400.otf", "FontAwesomeRegular");
                    fonts.AddFont("Font Awesome 7 Free-Solid-900.otf", "FontAwesomeSolid");
                });
                

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddDbContext<GalleryMapDbContext>(options => options.UseSqlite("Data Source=maui.db"));


            builder.Services.AddScoped<MainPageViewModel>();

            builder.Services.AddTransient<AddImagePageViewModel>();
            builder.Services.AddTransient<AddImagePage>();

            builder.Services.AddTransient<MapsPage>();
            builder.Services.AddTransient<MapsPageViewModel>();


            builder.Services.AddScoped<IImageLocationRepository, ImageLocationRepository>();

            builder.Services.AddSingleton<IImageService, ImageService>();
            builder.Services.AddSingleton<ILocationService, LocationService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GalleryMapDbContext>();
                dbContext.Database.Migrate();
            }

            return app;
        }
    }
}

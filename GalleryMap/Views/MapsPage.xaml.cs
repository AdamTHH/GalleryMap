using GalleryMap.Database.Repositories;
using GalleryMap.ViewModels;
using GalleryMap.Models;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.Generic;

namespace GalleryMap.Views;

public partial class MapsPage : ContentPage
{
    private readonly IImageLocationRepository _imageLocationRepository;
    private Dictionary<Pin, ImageLocation> _pinToImageLocationMap = new Dictionary<Pin, ImageLocation>();
    
    public MapsPage(MapsPageViewModel mapsPageViewModel, IImageLocationRepository imageLocationRepository)
	{
		InitializeComponent();

        BindingContext = mapsPageViewModel;

        _imageLocationRepository = imageLocationRepository;

        LoadAllImageLocationsAsync();
    }
    private async void LoadAllImageLocationsAsync()
    {
        try
        {
            var imageLocations = await _imageLocationRepository.GetAllAsync();

            mainMap.Pins.Clear();
            _pinToImageLocationMap.Clear();

            foreach (var imageLocation in imageLocations)
            {
                var pin = new Pin
                {
                    Label = $"Image {imageLocation.Id}",
                    Address = $"Created: {imageLocation.CreatedAt:MMM dd, yyyy}",
                    Location = new Location(imageLocation.Latitude, imageLocation.Longitude),
                    Type = PinType.Place
                };
                
                pin.MarkerClicked += OnPinClicked;
                mainMap.Pins.Add(pin);
                _pinToImageLocationMap[pin] = imageLocation;
            }

            if (imageLocations.Any())
            {
                var firstLocation = imageLocations.First();
                mainMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Location(firstLocation.Latitude, firstLocation.Longitude),
                    Distance.FromKilometers(10)));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading image locations: {ex.Message}");
        }
    }

    private void OnPinClicked(object sender, PinClickedEventArgs e)
    {
        if (sender is Pin pin && _pinToImageLocationMap.TryGetValue(pin, out ImageLocation imageLocation))
        {
            ShowImageOverlay(imageLocation);
        }
        e.HideInfoWindow = true;
    }

    private void ShowImageOverlay(ImageLocation imageLocation)
    {
        try
        {
            overlayImage.Source = imageLocation.ImageSource;
            imageInfoLabel.Text = $"Image {imageLocation.Id} - {imageLocation.CreatedAt:MMM dd, yyyy 'at' HH:mm}";
            imageOverlay.IsVisible = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error showing image overlay: {ex.Message}");
        }
    }

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        imageOverlay.IsVisible = false;
    }

    private void OnOverlayTapped(object sender, EventArgs e)
    {
        imageOverlay.IsVisible = false;
    }
}

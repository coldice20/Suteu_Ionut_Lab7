using Plugin.LocalNotification;
using SuteuIonutLab7.Models;

namespace SuteuIonutLab7;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
    }
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }
    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;
        var locations = await Geocoding.GetLocationsAsync(address);

        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat"
        };
        var location = locations?.FirstOrDefault();
        // var myLocation = await Geolocation.GetLocationAsync();
        var myLocation = new Location(46.7731796289, 23.6213886738);

        var distance = myLocation.CalculateDistance(location, DistanceUnits.Kilometers);
        if (distance < 4)
        {
            var request = new NotificationRequest
            {
                Title = "Ai de facut cumparaturi in apropiere!",
                Description = address,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
            LocalNotificationCenter.Current.Show(request);
        }
        await Map.OpenAsync(location, options);
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Confirmare", "Sunteti sigur ca doriti sa stergeti acest magazin?", "Da", "Nu");

        if (answer)
        {
            bool deleteSuccess = await DeleteShopAsync();

            if (deleteSuccess)
            {
                await DisplayAlert("Succes", "Magazinul a fost sters cu succes!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Eroare", "A aparut o problema la stergerea magazinului.", "OK");
            }
        }
    }

    private async Task<bool> DeleteShopAsync()
    {
        try
        {
            var shop = (Shop)BindingContext;

            int result = await App.Database.DeleteShopAsync(shop);

            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la stergerea magazinului: {ex.Message}");
            return false;
        }
    }
}
using SuteuIonutLab7.Models;

namespace SuteuIonutLab7
{
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();
        }
        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var slist = (ShopList)BindingContext;
            slist.Date = DateTime.UtcNow;
            await App.Database.SaveShopListAsync(slist);
            await Navigation.PopAsync();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var slist = (ShopList)BindingContext;
            await App.Database.DeleteShopListAsync(slist);
            await Navigation.PopAsync();
        }

        async void OnChooseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
            {
                BindingContext = new Product()
            });

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var shopl = (ShopList)BindingContext;

            listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
        }

        async void OnDeleteItemButtonClicked(object sender, EventArgs e)
        {
            if (listView.SelectedItem is Product selectedProduct)
            {
                await App.Database.DeleteProductAsync(selectedProduct);
                var shopl = (ShopList)BindingContext;
                listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
                listView.SelectedItem = null;
            }
            else
            {
                await DisplayAlert("Error", "Please select an item to delete.", "OK");
            }
        }

    }
}

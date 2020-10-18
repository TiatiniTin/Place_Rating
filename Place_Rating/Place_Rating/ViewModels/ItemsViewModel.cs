using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;
using BoxProtocol;
using BoxProtocol.Models;
using Place_Rating.Views;
using Xamarin.Essentials;
using MagicOnion.Client;
using Grpc.Core;
using BoxProtocol.Interfaces;
using MessagePack;
using System.IO;

namespace Place_Rating.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;
        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Item> ItemTapped { get; }

        public ItemsViewModel()
        {
            Title = "Places";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Item>(OnItemSelected);
            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            //var channel = new Channel("10.0.2.2", 12345, ChannelCredentials.Insecure);
            var channel = new Channel("192.168.1.69", 12345, ChannelCredentials.Insecure);
            var DataStore = MagicOnionClient.Create<IServerDB>(channel);

            IsBusy = true;
            //var location = Geolocation.GetLastKnownLocationAsync().Result;
            /*Item item_1 = new Item
            {
                Id = Guid.NewGuid().ToString(),
                Name = "First person",
                Place_image_path = "isaak.jpg",
                Place_name = "Исаакиевский собор",
                Place_rating = "10/10",
                //Place_location = location,
                Place_description = "This is an item description.",
                Time_created = DateTime.Now
            };
            Item item_2 = new Item
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Second person",
                Place_image_path = "Hermitage.jpg",
                Place_name = "Эрмитаж",
                Place_rating = "10/10",
                //Place_location = location,
                Place_description = "This is an item description.",
                Time_created = DateTime.Now
            };
            await DataStore.Add(item_1);
            await DataStore.Add(item_2);*/

            try
            {
                Items.Clear();
                var items = await DataStore.GetAll();
                foreach (var item in items)
                {
                    if (item.Place_image_path == null) 
                    {
                        var path = $"/Place_Rating/photo/{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg";
                        File.WriteAllBytes(path, item.Image_arr);
                        item.Place_image_path = path;
                        await DataStore.Update(item);
                        Items.Add(item);
                    }
                    else
                    {
                        Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}
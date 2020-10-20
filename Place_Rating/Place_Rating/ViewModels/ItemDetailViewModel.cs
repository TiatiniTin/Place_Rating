using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BoxProtocol.Models;
using Xamarin.Forms;
using Xamarin.Essentials;
using BoxProtocol;
using Grpc.Core;
using BoxProtocol.Interfaces;
using MagicOnion.Client;

namespace Place_Rating.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string place_name;
        private string place_description;
        private string place_rating;
        private double place_location_Longitude;
        private double place_location_Latitude;
        private string place_image_path_server;
        private string place_image_path_client;
        private DateTime time_created;
        private string name;

        public string Id { get; set; }

        public string Place_name
        {
            get => place_name;
            set => SetProperty(ref place_name, value);
        }

        public string Place_description
        {
            get => place_description;
            set => SetProperty(ref place_description, value);
        }
        public string Place_rating
        {
            get => place_rating;
            set => SetProperty(ref place_rating, value);
        }        
        public DateTime Time_created
        {
            get => time_created;
            set => SetProperty(ref time_created, value);
        }
        public double Place_location_Latitude
        {
            get => place_location_Latitude;
            set => SetProperty(ref place_location_Latitude, value);
        }

        /*public double get_location_Latitude()
        {
            try
            {
                var location = Geolocation.GetLastKnownLocationAsync().Result;
                //var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                //Location location = Geolocation.GetLocationAsync(request).Result;

                if (location != null)
                {
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    return location.Latitude;
                }
            }
            catch (FeatureNotSupportedException )
            {
                // Handle not supported on device exception

            }
            catch (FeatureNotEnabledException )
            {
                // Handle not enabled on device exception

            }
            catch (PermissionException )
            {
                // Handle permission exception

            }
            catch (Exception )
            {
                // Unable to get location

            }
            return -1;
        }*/

        public double Place_location_Longitude
        {
            get => place_location_Longitude;
            set => SetProperty(ref place_location_Longitude, value);
        }

        /*public double get_location_Longitude()
        {
            try
            {
                var location = Geolocation.GetLastKnownLocationAsync().Result;
                //var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                //Location location = Geolocation.GetLocationAsync(request).Result;

                if (location != null)
                {
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    return location.Longitude;
                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception

            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception

            }
            catch (PermissionException)
            {
                // Handle permission exception

            }
            catch (Exception)
            {
                // Unable to get location

            }
            return -1;
        }*/

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string Place_image_path_server
        {
            get => place_image_path_server;
            set => SetProperty(ref place_image_path_server, value);
        }        
        public string Place_image_path_client
        {
            get => place_image_path_client;
            set => SetProperty(ref place_image_path_client, value);
        }        

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                //new Command(async () => await LoadItemId(value));
                LoadItemId(value);
            }
        }

        async Task LoadItemId(string itemId)
        {
            try
            {
                //var options = new[] { new ChannelOption("grpc.max_receive_message_length", 1024 * 1024 * 1024), new ChannelOption("grpc.max_send_message_length", 1024 * 1024 * 1024) };
                var options = new[] { new ChannelOption("grpc.max_receive_message_length", 1024 * 1024 * 1024)};
                //var channel = new Channel("10.0.2.2", 12345, ChannelCredentials.Insecure);
                var channel = new Channel("192.168.1.69", 12345, ChannelCredentials.Insecure, options);
                var DataStore = MagicOnionClient.Create<IServerDB>(channel);

                Item item = await DataStore.Get(itemId);
                Id = item.Id;
                Name = item.Name;
                Place_image_path_client = item.Place_image_path_client;
                Place_image_path_server = item.Place_image_path_server;
                Place_name = item.Place_name;
                Place_rating = item.Place_rating;
                Place_location_Latitude = item.Place_location_Latitude;
                Place_location_Longitude = item.Place_location_Longitude;
                Place_description = item.Place_description;
                Time_created = item.Time_created;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}

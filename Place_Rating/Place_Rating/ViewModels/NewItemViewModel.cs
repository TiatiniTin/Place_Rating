using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using BoxProtocol.Models;
using BoxProtocol.Interfaces;
using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Nest;
using System.IO;
using MagicOnion.Client;
using Grpc.Core;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Location = Xamarin.Essentials.Location;

namespace Place_Rating.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string place_name;
        private string place_description;
        private string place_rating;
        private double place_location_Longitude;
        private double place_location_Latitude;
        private string place_image_path;
        private DateTime time_created;
        private string name;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            TakePhotoCommand = new Command(OnTakePhoto);
            //PickPhotoCommand = new Command(OnPickPhoto);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(place_name)
                && !String.IsNullOrWhiteSpace(place_description)
                && !String.IsNullOrWhiteSpace(place_rating)
                && !String.IsNullOrWhiteSpace(name);
        }

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
            set => SetProperty(ref time_created, DateTime.Now);
        }
        /*public double Place_location_Latitude
        {
            get => place_location_Latitude;
            set => SetProperty(ref place_location_Latitude, get_location_Latitude());
        }

        public double get_location_Latitude()
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
        }

        public double Place_location_Longitude
        {
            get => place_location_Longitude;
            set => SetProperty(ref place_location_Latitude, get_location_Longitude());
        }

        public double get_location_Longitude()
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
        /*public string Place_image_path
        {
            get => place_image_path;
            set => SetProperty(ref place_image_path, value);
        }*/

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            //var channel = new Channel("10.0.2.2", 12345, ChannelCredentials.Insecure);
            var channel = new Channel("192.168.1.69", 12345, ChannelCredentials.Insecure);
            var DataStore = MagicOnionClient.Create<IServerDB>(channel);

            Item newItem = new Item()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                Place_image_path = img.Source.ToString().Split(' ')[1],
                Place_name = Place_name,
                Place_rating = Place_rating,
                //Place_location_ = Geolocation.GetLastKnownLocationAsync().Result,
                Place_location_Latitude = Geolocation.GetLastKnownLocationAsync().Result.Latitude,
                Place_location_Longitude = Geolocation.GetLastKnownLocationAsync().Result.Longitude,
                Place_description = Place_description,             
                Time_created = Time_created
            };

            await DataStore.Add(newItem);
   
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        public Command TakePhotoCommand { get; }
        //public Command PickPhotoCommand { get; }

        Image img = new Image();

        private async void OnTakePhoto()
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "Sample",
                    Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
                });

                if (file == null)
                    return;

                img.Source = ImageSource.FromFile(file.Path);
            }
        }

        /*private async void OnPickPhoto()
        {
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                img.Source = ImageSource.FromFile(photo.Path);

            }
        }*/
    }
}

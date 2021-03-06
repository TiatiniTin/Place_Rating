﻿using System;
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
using MessagePack;

namespace Place_Rating.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string place_name;
        private string place_description;
        private string place_rating;
        private double place_location_Longitude;
        private double place_location_Latitude;
        private string place_image_path_server;
        private string place_image_path_client;
        private DateTime time_created;
        private string name;
        private byte[] image_arr;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            TakePhotoCommand = new Command(OnTakePhoto);
            PickPhotoCommand = new Command(OnPickPhoto);
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
            set => SetProperty(ref time_created, value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }        
        public byte[] Image_arr
        {
            get => image_arr;
            set => SetProperty(ref image_arr, value);
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
        public double Place_location_Latitude
        {
            get => place_location_Latitude;
            set => SetProperty(ref place_location_Latitude, value);
        }
        public double Place_location_Longitude
        {
            get => place_location_Longitude;
            set => SetProperty(ref place_location_Longitude, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var options = new[] { new ChannelOption("grpc.max_receive_message_length", 1024 * 1024 * 1024)};
            //var options = new[] { new ChannelOption("grpc.max_receive_message_length", 1024 * 1024 * 1024), new ChannelOption("grpc.max_send_message_length", 1024 * 1024 * 1024) };
            //var channel = new Channel("10.0.2.2", 12345, ChannelCredentials.Insecure);
            var channel = new Channel("192.168.1.69", 12345, ChannelCredentials.Insecure, options);
            var DataStore = MagicOnionClient.Create<IServerDB>(channel);

            using (FileStream fstream = File.OpenRead(Place_image_path_client))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                Image_arr = array;
            }

            Item newItem = new Item()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                Place_image_path_server = "null",
                Place_image_path_client = Place_image_path_client,
                Place_name = Place_name,
                Place_rating = Place_rating,
                //Place_location_ = Geolocation.GetLastKnownLocationAsync().Result,
                Place_location_Latitude = Geolocation.GetLastKnownLocationAsync().Result.Latitude,
                Place_location_Longitude = Geolocation.GetLastKnownLocationAsync().Result.Longitude,
                Place_description = Place_description,             
                Time_created = DateTime.Now,
                Image_arr = Image_arr
            };

            await DataStore.Add(newItem);
   
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        public Command TakePhotoCommand { get; }
        public Command PickPhotoCommand { get; }

        private async void OnTakePhoto()
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "Sample",
                    Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
                });

                if (photo == null)
                    return;
                Place_image_path_client = ImageSource.FromFile(photo.Path).ToString().Split(' ')[1];
            }

        }

        private async void OnPickPhoto()
        {
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                Place_image_path_client = ImageSource.FromFile(photo.Path).ToString().Split(' ')[1];
                //img.Source = ImageSource.FromFile(photo.Path);
                //return;
            }
        }
    }
}

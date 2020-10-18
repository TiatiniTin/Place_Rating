using BoxProtocol.Interfaces;
using System;
using Xamarin.Essentials;
using MessagePack;
using Xamarin.Forms;
using Elasticsearch.Net;

namespace BoxProtocol.Models
{
    [MessagePackObject]
    public class Item : IHaveID
    {
        [Key(0)]
        public string Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public string Place_image_path { get; set; }
        [Key(3)]
        public string Place_name { get; set; }
        [Key(4)]
        public string Place_rating { get; set; }
        [Key(5)]
        public double Place_location_Latitude { get; set; }
        [Key(6)]
        public double Place_location_Longitude { get; set; }
        [Key(7)]
        public string Place_description { get; set; }
        [Key(8)]
        public DateTime Time_created { get; set; }
        [Key(9)]
        public byte[] Image_arr { get; set; }

    }
}
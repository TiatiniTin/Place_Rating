using System;
using System.Linq;
using System.Threading.Tasks;
using BoxProtocol.Models;
using BoxProtocol.Interfaces;
using Xamarin.Essentials;
using Nest;
using MagicOnion.Server;
using MagicOnion;
using System.Collections.Generic;
using Plugin.Media.Abstractions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using static System.Net.Mime.MediaTypeNames;

namespace BoxProtocol
{
    public class ServerDB : ServiceBase<IServerDB>, IServerDB
    {
        private static bool Initialized;
        private static ElasticClient _client;
        public static ElasticClient Client()
        {
            if (!Initialized)
            {
                var node = new Uri("http://127.0.0.1:9200");
                var settings = new ConnectionSettings(node);
                _client = new ElasticClient(settings);
                Initialized = true;
                _client.Indices.Create("data_base");
            }
            return _client;
        }

        public UnaryResult<bool> Add(Item item)
        {
            var client = Client();
            client.Index<Item>(item, idx => idx.Index("data_base"));
            return new UnaryResult<bool>(true);
        }

        public UnaryResult<bool> Update(Item updated)
        {
            Client().Update<Item>(updated.Id, descriptor => descriptor.Doc(updated).Index("data_base"));
            return new UnaryResult<bool>(true);
        }

        /*public UnaryResult<bool> Delete(string id) 
        {
            Client().Delete<Item>(id, descriptor => descriptor.Index("data_base"));
            return new UnaryResult<bool>(true);
        }*/

        public UnaryResult<Item> Get(string id) 
        {
            var doc = Client().Get<Item>(id, idx => idx.Index("data_base"));
            return new UnaryResult<Item>(doc.Source);
        }
        public UnaryResult<List<Item>> GetAll() 
        {
            var docs = Client().Search<Item>(s => s
                .Index("data_base")
                .Query(q => q
                    .MatchAll())
            );
            return new UnaryResult<List<Item>>(docs.Documents.ToList());
        }

        /*public UnaryResult<string> SaveImage(MediaFile photo)
        {
            var path = $"/Place_Rating/photo/{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg";

            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, photo); 
                File.WriteAllBytes(path, ms.ToArray());
            }
            return new UnaryResult <string>(path);
        }*/

        /*public UnaryResult<List<Item>> GetOnLocation(GeoLocation point) 
        {
            var docs = Client().Search<Item>(s => s
                .Query(query => query.GeoDistance(
                        n => n.Distance("1km").Location(point)
                    )
                )
            );
            return new UnaryResult<List<Item>>(docs.Documents.ToList());
        }*/
        
        // Convert a byte array to an Object
        /*public UnaryResult<MediaFile> ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return new UnaryResult<MediaFile>((MediaFile)obj);
            }
        }*/

    }
}


  
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoxProtocol.Models;
using Xamarin.Essentials;
using MagicOnion;

namespace BoxProtocol.Interfaces
{
    public interface IHaveCoordinates
    {
        double Place_location_Latitude { get; set; }
        double Place_location_Longitude { get; set; }
    }
}

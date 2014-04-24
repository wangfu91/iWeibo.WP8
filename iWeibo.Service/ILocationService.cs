using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWeibo.WP8.Services
{
    public interface ILocationService
    {
        GeoCoordinate TryGetCurrentLocation();

        void StartWatcher();

        void StopWatcher();
    }
}

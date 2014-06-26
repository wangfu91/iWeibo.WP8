using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using iWeibo.WP8.Adapters;

namespace iWeibo.WP8.Services
{
    public class LocationService:ILocationService
    {
        private readonly TimeSpan maximumAge=TimeSpan.FromMinutes(15);

        private readonly ISettingStore settingsStore;

        private GeoCoordinate lastCoordinate = GeoCoordinate.Unknown;

        private DateTime lastCoordinateTime;

        private IGeoCoordinateWatcher geoCoordinateWatcher;


        public LocationService(ISettingStore settingsStore,IGeoCoordinateWatcher geoCoordinateWatcher)
        {
            this.settingsStore = settingsStore;
            this.geoCoordinateWatcher = geoCoordinateWatcher;
        }
        public GeoCoordinate TryGetCurrentLocation()
        {
            throw new NotImplementedException();
        }

        public void StartWatcher()
        {
            throw new NotImplementedException();
        }

        public void StopWatcher()
        {
            throw new NotImplementedException();
        }
    }
}

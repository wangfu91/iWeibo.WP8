using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWeibo.WP8.Adapters
{
    public interface IGeoCoordinateWatcher : IDisposable
    {
        void Start();
        void Start(bool suppressPermissionPrompt);
        bool TryStart(bool suppressPermissionPrompt, TimeSpan timeout);
        void Stop();
        GeoPositionAccuracy DesiredAccurary { get; }
        double MovementThreshold { get; set; }
        GeoPosition<GeoCoordinate> Position { get; }
        GeoPositionStatus Status { get; }
        GeoPositionPermission Permission { get; }
        event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionChanged;
        event EventHandler<GeoPositionStatusChangedEventArgs> StatusChanged;

    }
}

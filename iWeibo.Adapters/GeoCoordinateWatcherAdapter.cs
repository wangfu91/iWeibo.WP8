using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWeibo.WP8.Adapters
{
    public class GeoCoordinateWatcherAdapter:IGeoCoordinateWatcher
    {

        private GeoCoordinateWatcher WrappedSubject { get; set; }

        public GeoCoordinateWatcherAdapter()
        {
            WrappedSubject = new GeoCoordinateWatcher();
            AttachToEvents();
        }

        public GeoCoordinateWatcherAdapter(GeoPositionAccuracy desiredAccuracy)
        {
            WrappedSubject=new GeoCoordinateWatcher(desiredAccuracy);
            AttachToEvents();
        }

        public void Start()
        {
            WrappedSubject.Start();
        }

        public void Start(bool suppressPermissionPrompt)
        {
            WrappedSubject.Start(suppressPermissionPrompt);
        }

        public bool TryStart(bool suppressPermissionPrompt, TimeSpan timeout)
        {
            return WrappedSubject.TryStart(suppressPermissionPrompt,timeout);
        }

        public void Stop()
        {
            WrappedSubject.Stop();
        }

        public System.Device.Location.GeoPositionAccuracy DesiredAccurary
        {
            get { return WrappedSubject.DesiredAccuracy;}
        }

        public double MovementThreshold
        {
            get
            {
                return WrappedSubject.MovementThreshold;
            }
            set
            {
                WrappedSubject.MovementThreshold=value;
            }
        }

        public System.Device.Location.GeoPosition<System.Device.Location.GeoCoordinate> Position
        {
            get { return WrappedSubject.Position;}
        }

        public System.Device.Location.GeoPositionStatus Status
        {
            get { return WrappedSubject.Status;}
        }

        public System.Device.Location.GeoPositionPermission Permission
        {
            get { return WrappedSubject.Permission;}
        }

        public event EventHandler<System.Device.Location.GeoPositionChangedEventArgs<System.Device.Location.GeoCoordinate>> PositionChanged;

        public event EventHandler<System.Device.Location.GeoPositionStatusChangedEventArgs> StatusChanged;


        private void AttachToEvents()
        {
            WrappedSubject.PositionChanged += WrappedSubject_PositionChanged;
            WrappedSubject.StatusChanged += WrappedSubject_StatusChanged;
        }

        void WrappedSubject_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            StatusChangedRelay(sender,e);
        }

        private void StatusChangedRelay(object sender,GeoPositionStatusChangedEventArgs e)
        {
            var handler = StatusChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        void WrappedSubject_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            PositionChangedRelay(sender, e);
        }

        private void PositionChangedRelay(object sender,GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
         	var hanler=PositionChanged;
            if(hanler!=null)
            {
                hanler(sender,e);
            }
        }

        public void Dispose()
        {
            WrappedSubject.Dispose();
        }
    }
}

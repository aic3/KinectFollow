using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace KinectMonitor.Services
{
    public class BodyHub: Hub
    {
        
        public void SendHeadTracking(HeadTrackingNotification notification)
        {
            this.Clients.All.headTracked(notification);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}

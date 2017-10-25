using Microsoft.Owin.Hosting;
using System;

namespace KinectMonitor.Services
{
    // ref: https://code.msdn.microsoft.com/Using-SignalR-in-WinForms-f1ec847b/view/SourceCode#content
    public class SignalRServer
    {
        private string _hostURI = "";
        private IDisposable _server = null;

        public SignalRServer(String uri)
        {
            this._hostURI = uri;
        }

        public void Start()
        {
            try
            {
                this._server = WebApp.Start(this._hostURI);

                //this.Log(String.Format("Server Started: {0}", this._hostURI));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Stop()
        {
            if (this._server != null)
            {
                this._server.Dispose();
                this._server = null;

                //this.Log(String.Format("Server Stopped: {0}", this._hostURI));
            }
        }

        
    }
}

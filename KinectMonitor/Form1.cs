using KinectMonitor.Services;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Windows.Forms;

namespace KinectMonitor
{
    public partial class Form1 : Form
    {

        private Monitor _monitor = null;
        private SignalRServer _server = null;
        private string _hostUri = "http://localhost:8080";
        private IHubProxy _hubProxy = null;
        private HubConnection _hubConnection = null;
        private string _clienName = Environment.MachineName + "_0";

        public Form1()
        {
            InitializeComponent();
        }

        public void StartMonitor()
        {
            if(this._monitor == null)
            {
                this._monitor = new Monitor();
                
                this._monitor.HeadTracked += _monitor_HeadTracked;
            }

            this._monitor.Start(0);
        }

        

        public void StopMonitor()
        {
            if (this._monitor != null)
            {
                this._monitor.Stop();
            }
        }

        public void StartServer()
        {
            if (this._server == null)
            {
                this._server = new SignalRServer(this._hostUri);
            }

            this._server.Start();
        }

        public void StopServer()
        {
            if(this._server != null)
            {
                this._server.Stop();
                this._server = null;
            }
        }

        private async void StartHubConnection()
        {
            if(this._hubConnection == null)
            {
                this._hubConnection = new HubConnection(this._hostUri);
                this._hubProxy = this._hubConnection.CreateHubProxy("BodyHub");

                // register the hub proxy
                this._hubProxy.On<string, string>("addMessage", (name, message) => {
                    this.Log(String.Format("Message received: {0}", message));
                });

                this._hubProxy.On<HeadTrackingNotification>("headTracked", (notification) => {
                    this.Log(String.Format("Head tracking received: {0}, {1}, {2}, {3}", notification.Person, notification.X, notification.Y, notification.Z));
                });

                try
                {
                    await this._hubConnection.Start();
                    this.Log("Connected to SignalR host");
                }
                catch (Exception ex)
                {
                    this.Log(String.Format( "Error attmpting to connect to SignalR host [{0}]. The message is: {1}", this._hostUri, ex.ToString()));
                }
            }
        }

        private void StopHubConnection()
        {
            if(this._hubConnection != null)
            {
                this._hubConnection.Stop();
                this._hubConnection.Dispose();
                this._hubConnection = null;
            }
        }

        public void Log(string message)
        {
            this.Log(message, DateTime.Now);
        }

        public void Log(string message, DateTime time)
        {
            String text = "";
            text = String.Format("\r\n{0} - {1}", time.ToString("yyyy.MM.dd HH:mm:ss.fff"), message);

            // ref: https://stackoverflow.com/questions/142003/cross-thread-operation-not-valid-control-accessed-from-a-thread-other-than-the
            if (this._debug.InvokeRequired)
            {
                this._debug.Invoke(new MethodInvoker(delegate { this._debug.AppendText(text); }));
            }
            else
            {
                this._debug.AppendText(text);
            }
        }

        private void _monitor_HeadTracked(object sender, HeadTrackingNotification e)
        {          
            // send the notifcation
            if(this._hubProxy != null)
            {
                this._hubProxy.Invoke("SendHeadTracking", e);
            }
            else
            {
                this.Log("No proxy found!");
            }
        }
        
        private void _run_Click(object sender, EventArgs e)
        {
            this._hubProxy.Invoke("SendHeadTracking", new HeadTrackingNotification() { Person = 1, X = 0.1f, Y = 0.2f, Z = 0.3f  });
        }

        private void _start_Click(object sender, EventArgs e)
        {
            this.StartServer();
            this.StartHubConnection();
            this.StartMonitor();
        }

        private void _stop_Click(object sender, EventArgs e)
        {
            this.StopMonitor();
            this.StopHubConnection();
            this.StopServer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

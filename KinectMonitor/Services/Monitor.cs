using Microsoft.Kinect;
using System;
using System.Linq;


namespace KinectMonitor.Services
{
    public class Monitor
    {
        private KinectSensor _sensor = null;
        private BodyFrameReader _bodyFrame = null;
        
        public event EventHandler<HeadTrackingNotification> HeadTracked = null;

        public Monitor()
        {
        }

        public void Start(double poll = 0)
        {
            this.Init();
            this._bodyFrame = this._sensor.BodyFrameSource.OpenReader();
            this._bodyFrame.FrameArrived += _bodyFrame_FrameArrived;            
        }

        private void _bodyFrame_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            this.ProcessBodyFrame(e.FrameReference.AcquireFrame());
        }

        private void ProcessBodyFrame(BodyFrame frame)
        {
            try
            {
                if (frame != null)
                {
                    int bodyCount = 0;
                    Body[] bodies = null;

                    bodyCount = frame.BodyCount;
                    bodies = new Body[bodyCount];
                    frame.GetAndRefreshBodyData(bodies);

                    if (this.HeadTracked != null)
                    {
                        bodies.ToList().ForEach(b =>
                        {
                            if (b != null && b.IsTracked)
                            {
                                b.Joints.ToList().ForEach(j =>
                                {
                                    if (j.Key == JointType.Head)
                                    {
                                        this.HeadTracked(this, new HeadTrackingNotification() { Person = b.TrackingId, X = j.Value.Position.X, Y = j.Value.Position.Y, Z = j.Value.Position.Z });
                                    }
                                });
                            }
                        });
                    }
                    
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.SafeDispose(frame);
            }
        }


        public void Stop()
        {
            if (this._sensor != null)
            {
                this._sensor.Close();
                this._sensor = null;
            }        
        }


        private void Init()
        {
            if(this._sensor == null)
            {
                this._sensor = KinectSensor.GetDefault();
                this._sensor.Open();

            }
        }

        private void SafeDispose(IDisposable obj)
        {
            if(obj != null)
            {
                obj.Dispose();
                obj = null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectMonitor.Services
{
    public class Logger
    {
        public event EventHandler<LoggingNotification> LogEvent = null;

        protected void Log(String msg, LogLevel level = LogLevel.Verbose)
        {
            try
            {
                if (this.LogEvent != null)
                {
                    LoggingNotification notification = null;
                    notification = new LoggingNotification()
                    {
                        Level = level,
                        Message = msg,
                        Time = DateTime.Now
                    };
                    this.LogEvent(this, notification);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectMonitor.Services
{
    public class LoggingNotification
    {
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public LogLevel Level {get; set;}
    }
}


using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    /// <summary>
    /// LoggingService class implements  ILoggingService
    /// </summary>
    public class LoggingService : ILoggingService
    {
        /// <summary>
        /// An event for writing logs whan a Message have been Recieved
        /// </summary>
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// writes massage to the log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs msg = new MessageRecievedEventArgs();
            msg.Message = message;
            msg.Status = type;
            MessageRecieved?.Invoke(this, msg);
        }
    }
}

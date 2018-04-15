using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    /// <summary>
    /// ILoggingService inteface which responsibole for log writing 
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// An event for writing logs whan a Message have been Recieved
        /// </summary>
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// Logging the Message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        void Log(string message, MessageTypeEnum type);           // Logging the Message
    }
}

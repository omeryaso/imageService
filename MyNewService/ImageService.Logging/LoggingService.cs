
using ImageService.Infrastructure.Enums;
using ImageService.Logging.Modal;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Log> LogsCollection { get; set; }
        public event LogsUpdate LogsUpdate;

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
            LogsCollection.Add(new Log { Type = (Convert.ToString((int)type)), Message = message });
            msg.Message = message;
            msg.Status = type;
            MessageRecieved?.Invoke(this, msg);
        }

        public LoggingService()
        {
            LogsCollection = new ObservableCollection<Log>();
        }

        public void InvokeUpdateEvent(string message, Modal.MessageTypeEnum type)
        {
            Log log = new Log { Type = Enum.GetName(typeof(MessageTypeEnum), type), Message = message };
            string[] args = new string[2];

            // args[0] = EntryType, args[1] = Message
            args[0] = log.Type;
            args[1] = log.Message;
            CommandRecievedEventArgs updateObj = new CommandRecievedEventArgs((int)CommandEnum.NewLog, args, null);
            if (this.LogsUpdate != null)
            {
                LogsUpdate?.Invoke(updateObj);
            }
        }
    }
}

using System;
using ImageService.Logging.Modal;

namespace ImageService.Logging
{
    public class Log
    {
        private MessageTypeEnum type;
        private string message;

        public string Type
        {
            get { return Enum.GetName(typeof(MessageTypeEnum), type); }
            set { type = (MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), value); }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
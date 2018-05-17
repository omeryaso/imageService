using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Log
    {
      
        public MessageTypeEnum Type { get; set; }
        public string Message { get; set; }
        public Log(MessageTypeEnum Status, string Message)
        {
            this.Type = Status;
            this.Message = Message;
        }
    }
}

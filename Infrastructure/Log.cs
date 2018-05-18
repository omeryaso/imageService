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
        public Log(MessageTypeEnum Type, string Message)
        {
            this.Type = Type;
            this.Message = Message;
        }
    }
}

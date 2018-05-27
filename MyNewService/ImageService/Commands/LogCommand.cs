using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {

        private ILoggingService logging;

        public LogCommand(ILoggingService loggingService)
        {
            this.logging = loggingService;
        }

        public string Execute(string[] args, out bool result)
        {
            try
            {
                ObservableCollection<Log> logMessages = logging.LogsCollection;
                string[] arr = { JsonConvert.SerializeObject(logMessages)};
                CommandRecievedEventArgs commandSendArgs = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, arr, "");
                result = true;
                return JsonConvert.SerializeObject(commandSendArgs);
            }
            catch (Exception e)
            {
                result = false;
                return "LogCommand: " + e.Message;
            }
        }
    }
}

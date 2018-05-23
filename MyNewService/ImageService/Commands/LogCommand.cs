using ImageService.Logging;
using Infrastructure;
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

        private ILoggingService loggingService;

        public LogCommand(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }

        public string Execute(string[] args, out bool result)
        {
            // ObservableCollection<Log> logMessages = this.loggingService.Log;

            // return JsonConvert.SerializeObject();

            throw new NotImplementedException();
        }

    }
}

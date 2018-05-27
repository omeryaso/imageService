using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            try
            {
                string[] arr = {ConfigurationManager.AppSettings.Get("OutputDir"), ConfigurationManager.AppSettings.Get("SourceName"),
                ConfigurationManager.AppSettings.Get("LogName"), ConfigurationManager.AppSettings.Get("ThumbnailSize"),
                ConfigurationManager.AppSettings.Get("Handler")};

                result = true;
                return JsonConvert.SerializeObject( new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, arr, ""));
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}

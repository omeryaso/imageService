using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class HandlerShutDownCommand : ICommand
    {
        private ImageServer server;

        public HandlerShutDownCommand(ImageServer server)
        {
            this.server = server;
        }

        public string Execute(string[] args, out bool result)
        {
            try
            {
                result = true;
                if (args == null || args.Length == 0)
                {
                    throw new Exception("Error in shutting down the handler");
                }
                string handler = args[0];
                string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
                StringBuilder sbNewHandlers = new StringBuilder();
                for (int i = 0; i < directories.Length; i++)
                {
                    if (directories[i] != handler)
                    {
                        sbNewHandlers.Append(directories[i] + ";");
                    }
                }
                string newHandlers = (sbNewHandlers.ToString()).TrimEnd(';');
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                // Add an Application Setting.
                config.AppSettings.Settings.Remove("Handler");
                config.AppSettings.Settings.Add("Handler", newHandlers);
                // Save the configuration file.
                config.Save(ConfigurationSaveMode.Modified);
                // Force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");
                server.CloseHandler(handler);
                string[] array = new string[1];
                array[0] = handler;
                CommandRecievedEventArgs notifyParams = new CommandRecievedEventArgs((int)CommandEnum.HandlerShutDown, array, "");
                ImageServer.PerformSomeEvent(notifyParams);
                return string.Empty;
            }
            catch (Exception ex)
            {
                result = false;
                return ex.ToString();
            }
        }
    }
}

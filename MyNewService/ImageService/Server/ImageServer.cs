using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        public ImageServer(IImageController m_controller, ILoggingService m_logging)
        {
            this.m_controller = m_controller;
            this.m_logging = m_logging;
            string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
            // creates an handler for each directory
            foreach (string directory in directories)
            {
                CreateHandler(directory);
            }
        }

        private void CreateHandler(string directory)
        {
            IDirectoryHandler dHandler = new DirectoryHandler(m_controller, m_logging, directory);
            CommandRecieved += dHandler.OnCommandRecieved;
            dHandler.DirectoryClose += HandlerClose;
            dHandler.StartHandleDirectory(directory);
        }

        public void HandlerClose (object src, DirectoryCloseEventArgs e)
        {
            CommandRecieved -= ((IDirectoryHandler)src).OnCommandRecieved;
            m_logging.Log("The handler has been closed", Logging.Modal.MessageTypeEnum.INFO);
        }

        public void ServerClose()
        {
            CommandRecievedEventArgs commArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, null);
            m_logging.Log("The server has been closed", Logging.Modal.MessageTypeEnum.INFO);
            CommandRecieved?.Invoke(this, commArgs);
        }
    }


}

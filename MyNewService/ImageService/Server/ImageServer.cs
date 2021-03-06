﻿using ImageService.Controller;
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
    /// <summary>
    /// ImageServer class
    /// </summary>
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public delegate void NotifyClients(CommandRecievedEventArgs msg);
        public static event NotifyClients NotifyHandlersRemoved;
        public event EventHandler<DirectoryCloseEventArgs> CloseServer;
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public Dictionary<string, IDirectoryHandler> Handlers { get; set; }
        public string[] Directories { get; set; }
        #endregion

        /// <summary>
        /// constructor - createas an handler for each directory
        /// </summary>
        /// <param name="m_controller"></param>
        /// <param name="m_logging"></param>
        public ImageServer(IImageController m_controller, ILoggingService m_logging)
        {
            this.m_controller = m_controller;
            this.Handlers = new Dictionary<string, IDirectoryHandler>();
            this.m_logging = m_logging;
            Directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
            // creates an handler for each directory
            foreach (string directory in Directories)
            {
                CreateHandler(directory);
            }
        }

        /// <summary>
        /// CreateHandler creats a diractory handler
        /// </summary>
        /// <param name="directory"></param>
        private void CreateHandler(string directory)
        {
            IDirectoryHandler dHandler = new DirectoryHandler(m_controller, m_logging, directory);
            Handlers[directory] = dHandler;
            CommandRecieved += dHandler.OnCommandRecieved;
            dHandler.DirectoryClose += HandlerClose;
            dHandler.StartHandleDirectory(directory);
        }

        public static void PerformSomeEvent(CommandRecievedEventArgs commandRecievedEventArgs)
        {
            NotifyHandlersRemoved.Invoke(commandRecievedEventArgs);
        }

        internal void CloseHandler(string handler)
        {
            try
            {
                if (Handlers.ContainsKey(handler))
                {
                    m_logging.Log("The handler is " + handler, Logging.Modal.MessageTypeEnum.INFO);
                    IDirectoryHandler newHandler = Handlers[handler];
                    this.CloseServer -= newHandler.closeHandler;
                    newHandler.closeHandler(this, null);
                }
            }
            catch (Exception ex)
            {
                m_logging.Log("Error in " + ex, Logging.Modal.MessageTypeEnum.FAIL);            }
        }

        /// <summary>
        /// closes a handler and updates the log
        /// </summary>
        /// <param name="src"></param>
        /// <param name="e"></param>
        public void HandlerClose (object src, DirectoryCloseEventArgs e)
        {
            CommandRecieved -= ((IDirectoryHandler)src).OnCommandRecieved;
            m_logging.Log("The handler has been closed", Logging.Modal.MessageTypeEnum.INFO);
        }

        /// <summary>
        /// closes the server and updates the log
        /// </summary>
        public void ServerClose()
        {
            CommandRecievedEventArgs commArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, null);
            m_logging.Log("The server has been closed", Logging.Modal.MessageTypeEnum.INFO);
            CommandRecieved?.Invoke(this, commArgs);
        }
    }


}

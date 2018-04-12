using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;
using ImageService.Server;

namespace ImageService.Controller.Handlers
{
    public class DirectoryHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        private readonly string[] extentions = { ".jpg", ".png", ".gif", ".bmp" };      // the filters
        #endregion


        public DirectoryHandler(IImageController m_controller, ILoggingService m_logging, string m_path)
        {
            this.m_controller = m_controller;
            this.m_logging = m_logging;
            this.m_path = m_path;
            m_dirWatcher = new FileSystemWatcher(this.m_path);

        }

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            string message = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
            if (result == false)
            {
                m_logging.Log(message, MessageTypeEnum.FAIL);
            }
            else
            {
                m_logging.Log(message, MessageTypeEnum.INFO);
            }
        }

        public void StartHandleDirectory(string dirPath)
        {
            //adding events and watchers
            m_logging.Log("StartHandleDirectory" + " " + dirPath, MessageTypeEnum.INFO);
            this.m_path = dirPath;
            this.m_dirWatcher = new FileSystemWatcher(this.m_path);
            this.m_dirWatcher.Changed += new FileSystemEventHandler(dirWatcherCreated);
            this.m_dirWatcher.Created += new FileSystemEventHandler(dirWatcherCreated);
            this.m_dirWatcher.EnableRaisingEvents = true;

        }

        private void dirWatcherCreated(object src, FileSystemEventArgs e)
        {
            this.m_logging.Log("dirWatcherCreated: " + e.FullPath, MessageTypeEnum.INFO);
            string extension = Path.GetExtension(e.FullPath).ToLower();
            // matching file extention with the extentions list and sends command accordingly
            if (this.extentions.Contains(extension))
            {
                string[] args = { e.FullPath };
                CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, this.m_path);
                this.OnCommandRecieved(this, eventArgs);
            }

        }

        public void closeHandler(object sender, DirectoryCloseEventArgs e)
        {
            // stop directory listeninig and writing to the log the result .
            try
            {
                this.m_dirWatcher.EnableRaisingEvents = false;
                ((ImageServer)sender).CommandRecieved -= this.OnCommandRecieved;
                this.m_logging.Log("closeHandler succesfully  " + this.m_path, MessageTypeEnum.INFO);
            }
            catch (Exception ex)
            {
                this.m_logging.Log("closeHandler succesfully  " + this.m_path + " "
                    + ex.ToString(), MessageTypeEnum.FAIL);
            }
        }
    }
}
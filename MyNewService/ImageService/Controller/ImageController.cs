using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// ImageController implements IImageController
    /// </summary>
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;
        private ImageServer imageServer;

        /// <summary>
        /// the constructor. initialze m_modal and creates commands dictionary
        /// </summary>
        /// <param name="modal"></param>
        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>()
            {
                // For Now will contain NEW_FILE_COMMAND
                { (int)CommandEnum.NewFileCommand,new NewFileCommand(m_modal) }
            };
        }

        public ImageServer ImageServer
        {
            get { return imageServer; }
            set {
                this.imageServer = value;
                this.commands[((int)CommandEnum.HandlerShutDown)] = new HandlerShutDownCommand(imageServer);
            }
        }

        /// <summary>
        /// if the command exist it will execute it using tasks.
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="args"></param>
        /// <param name="resultSuccesful"></param>
        /// <returns></returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            if (commands.TryGetValue(commandID, out ICommand command))
            {
                Task<Tuple<string, bool>> task = new Task<Tuple<string, bool>>(() =>
                {
                    string message = command.Execute(args, out bool result);
                    return Tuple.Create(message, result);
                });
                task.Start();
                resultSuccesful = task.Result.Item2;
                return task.Result.Item1;
            }
            else
            {
                resultSuccesful = false;
                return "The command hasn't been found";
            }
        }
    }
}


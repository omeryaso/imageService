using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
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
        public ImageServer ImageServer { get { return imageServer; } set{
                imageServer = value;
                commands[((int)CommandEnum.HandlerShutDown)] = new HandlerShutDownCommand(imageServer,logging);
            } }

        private ILoggingService logging;

        /// <summary>
        /// the constructor. initialze m_modal and creates commands dictionary
        /// </summary>
        /// <param name="modal"></param>
        public ImageController(IImageServiceModal modal, ILoggingService logging,ImageServer server)
        {
            this.logging = logging;
            m_modal = modal;
            imageServer = server;// Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>()
            {
                { (int)CommandEnum.NewFileCommand,new NewFileCommand(m_modal)},
                { (int)CommandEnum.CloseCommand,new CloseCommand()},
                { (int)CommandEnum.GetConfigCommand,new GetConfigCommand()},
                { (int)CommandEnum.LogCommand,new LogCommand(logging)},
            };
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


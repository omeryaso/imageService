using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageServiceWeb.Communication.ImageServiceClient;

namespace ImageServiceWeb.Communication
{
    interface IImageServiceClient
    {

        /// <summary>
        /// SendCommand function.
        /// sends command to srv.
        /// </summary>
        /// <param name="commandRecievedEventArgs">info to be sented to server</param>
        void SendCommand(CommandRecievedEventArgs commandRecievedEventArgs);
        /// <summary>
        /// CloseClient function.
        /// closes the client.
        /// </summary>
        void CloseClient();
        /// <summary>
        /// RecieveCommand function.
        /// creates task and reads new messages.
        /// </summary>
        void RecieveCommand();
        event UpdateResponseArrived UpdateResponse;
        bool IsConnected { get; set; }
    }
}

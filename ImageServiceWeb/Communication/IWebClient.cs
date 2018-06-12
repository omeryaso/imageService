using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageServiceWeb.Communication.WebClient;

namespace ImageServiceWeb.Communication
{
    interface IWebClient
    {

        /// <summary>
        /// SendMessage function.
        /// sends message to the server
        /// </summary>
        /// <param name="commandRecievedEventArgs">info to be sent to server</param>
        void SendMessage(CommandRecievedEventArgs msg);
        /// <summary>
        /// CloseClient function.
        /// closes the client.
        /// </summary>
        void CloseClient();
        /// <summary>
        /// RecieveMessage function.
        /// using a task to recieve a message from the server
        /// </summary>
        void RecieveMessage();
        /// <summary>
        /// Start function
        /// starts the client
        /// </summary>
        bool Start();
        event UpdateDataIn UpdateData;
        bool IsConnected { get; set; }
    }
}

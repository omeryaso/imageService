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
        /// sends command to srv.
        /// </summary>
        /// <param name="commandRecievedEventArgs">info to be sented to server</param>
        void SendMessage(CommandRecievedEventArgs msg);
        /// <summary>
        /// CloseClient function.
        /// closes the client.
        /// </summary>
        void CloseClient();
        /// <summary>
        /// RecieveMessage function.
        /// creates task and reads new messages.
        /// </summary>
        void RecieveMessage();
        bool Start();
        event UpdateDataIn UpdateData;
        bool IsConnected { get; set; }
    }
}

using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Communication
{
    public delegate void UpdateDataIn(CommandRecievedEventArgs data);

    interface IGUIClient
    {
        bool IsConnected { get; set; }
        bool Start();
        void SendMessage(CommandRecievedEventArgs msg);
        void RecieveMessage();
        void CloseClient();
        event UpdateDataIn UpdateData;
    }
}

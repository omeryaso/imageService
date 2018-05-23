using ImageService.Modal;
using ImageServiceGUI.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI
{
    class GUIClient : IGUIClient
    {
        private static GUIClient instance;
        bool IsConnected { get; set; }
        private TcpClient client;
        private GUIClient()
        {
            IsConnected = Start();
        }

        public static GUIClient Instance
        {
            get { if (instance == null)
                {
                    instance = new GUIClient();
                }
                return instance;
            }
        }

        public event UpdateDataIn UpdateData;
        public bool Start()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                client = new TcpClient();
                client.Connect(ep);
                return true;
            }            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }        }

        public void RecieveMessage()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(CommandRecievedEventArgs msg)
        {
            throw new NotImplementedException();
        }

        public void CloseClient()
        {
            throw new NotImplementedException();
        }
    }
}

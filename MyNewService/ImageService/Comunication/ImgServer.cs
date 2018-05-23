using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class ImgServer
    {
        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        private ILoggingService m_logging;

        public ImgServer(int port, IClientHandler ch, ILoggingService m_logging)
        {
            this.ch = ch;
            this.port = port;
            this.m_logging = m_logging;
        }


        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);
            listener.Start();
            m_logging.Log("Waiting for client connections...", MessageTypeEnum.INFO);
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        m_logging.Log("Got new connection", MessageTypeEnum.INFO);
                        ch.HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                m_logging.Log("Server stopped", MessageTypeEnum.INFO);
            });
            task.Start();
        }
        public void Stop()
        {
            listener.Stop();
        }
    }

}

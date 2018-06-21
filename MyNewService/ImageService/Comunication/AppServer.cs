using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Comunication
{
    class AppServer : IAppServer
    {
        ILoggingService Logging { get; set; }
        int Port { get; set; }
        TcpListener Listener { get; set; }
        IAppClientHandler ClientHandler { get; set; }
        private List<TcpClient> Clients = new List<TcpClient>();
        private static Mutex Mutex = new Mutex();

        /// <summary>
        /// creates an instance of the AppServer
        /// </summary>
        /// <param name="port"></param> the port we are using
        /// <param name="logging"></param> the service logger
        /// <param name="ch"></param> the ClientHandler
        public AppServer(int port, ILoggingService logging, IAppClientHandler ch)
        {
            Port = port;
            Logging = logging;
            ClientHandler = ch;
        }
        /// <summary>
        /// starts the server
        /// </summary>
        public void Start()
        {
            try
            {
                IPEndPoint ep = new
                IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);
                Listener = new TcpListener(ep);

                Listener.Start();
                Logging.Log("Waiting for client connections of the android application...", MessageTypeEnum.INFO);
                Task task = new Task(() =>
                {
                    while (true)
                    {
                        try
                        {
                            TcpClient client = Listener.AcceptTcpClient();
                            Logging.Log("Got new connection - ", MessageTypeEnum.INFO);
                            Clients.Add(client);
                            ClientHandler.HandleClient(client, Clients);
                        }
                        catch (Exception ex)
                        {
                            Logging.Log(ex.ToString(), MessageTypeEnum.FAIL);
                        }
                    }
                });
                task.Start();
            }
            catch (Exception ex)
            {
                Logging.Log(ex.ToString(), MessageTypeEnum.FAIL);
            }
        
    }
    }
}

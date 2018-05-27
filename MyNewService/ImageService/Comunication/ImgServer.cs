using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService
{
    class ImgServer
    {
        private int port;
        private TcpListener listener;
        private IClientHandler clientHandler;
        private ILoggingService loggingService;
        private List<TcpClient> clients = new List<TcpClient>();
        private static Mutex writeMutex = new Mutex();

        public ImgServer(int port, IClientHandler clientHandler, ILoggingService loggingService)
        {
            this.clientHandler = clientHandler;
            this.port = port;
            this.loggingService = loggingService;
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);
            listener.Start();
            loggingService.Log("Waiting for client connections...", MessageTypeEnum.INFO);
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        loggingService.Log("Got new connection", MessageTypeEnum.INFO);
                        clients.Add(client);
                        clientHandler.HandleClient(client, clients);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                loggingService.Log("Server stopped", MessageTypeEnum.INFO);
            });
            task.Start();
        }

        public void NotifyClients(CommandRecievedEventArgs commandRecievedEventArgs)
        {
            try
            {
                List<TcpClient> copyClients = new List<TcpClient>(clients);
                foreach (TcpClient client in copyClients)
                {
                    new Task(() =>
                    {
                        try
                        {
                            NetworkStream stream = client.GetStream();
                            BinaryWriter writer = new BinaryWriter(stream);
                            string jsonCommand = JsonConvert.SerializeObject(commandRecievedEventArgs);
                            lock (writeMutex) {
                                writer.Write(jsonCommand);
                            }
                        }
                        catch (Exception)
                        {
                            this.clients.Remove(client);
                        }

                    }).Start();
                }
            }
            catch (Exception ex)
            {
                loggingService.Log(ex.ToString(), MessageTypeEnum.FAIL);
            }
        }

        private void NotifyAboutNewLogEntry(CommandRecievedEventArgs update)
        {
            NotifyClients(update);
        }



        public void Stop()
        {
            listener.Stop();
        }
    }

}

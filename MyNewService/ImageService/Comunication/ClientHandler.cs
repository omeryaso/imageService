using ImageService.Controller;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ImageService
{
    class ClientHandler : IClientHandler
    {
        private IImageController imageController;
        private ILoggingService logging;
        public static Mutex Mutex { get; set; }
        //private static Mutex writeMutex = new Mutex();

        public ClientHandler (IImageController imageController, ILoggingService loggingService)
        {
            this.imageController = imageController;
            this.logging = loggingService;

        }

        public void HandleClient(TcpClient client, List<TcpClient> clients)
        {
            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        BinaryReader reader = new BinaryReader(stream);
                        BinaryWriter writer = new BinaryWriter(stream);

                        {
                            string commandLine = reader.ReadString();
                            CommandRecievedEventArgs commandRecievedEventArgs = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                            if (commandRecievedEventArgs.CommandID == (int)CommandEnum.DisconnectClient)
                            {
                                clients.Remove(client);
                                client.Close();
                                break;
                            }

                            string command = imageController.ExecuteCommand((int)commandRecievedEventArgs.CommandID, commandRecievedEventArgs.Args, out bool result);
                            logging.Log("exe1111 command:" + command, MessageTypeEnum.INFO);
                            //Mutex.WaitOne();
                            writer.Write(command);
                            //Mutex.ReleaseMutex();
                        }
                    }
                    catch (Exception e) {
                        //add mutex
                        clients.Remove(client);
                        logging.Log(e.ToString(), MessageTypeEnum.FAIL);
                        client.Close();
                        break;
                    }
                }
            }).Start();
        }

    }
}

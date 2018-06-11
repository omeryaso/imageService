using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ImageServiceWeb.Communication
{
    public class ImageServiceClient : IImageServiceClient
    {
        private TcpClient client;
        private bool m_isStopped;
        public delegate void UpdateResponseArrived(CommandRecievedEventArgs responseObj);
        public event UpdateResponseArrived UpdateResponse;
        private static ImageServiceClient m_instance;
        private static Mutex m_mutex = new Mutex();
        private static Mutex m_readMutex = new Mutex();
        public bool IsConnected { get; set; }

        /// <summary>
        /// ImageServiceClient private constructor.
        /// </summary>
        private ImageServiceClient()
        {
            this.IsConnected = this.Start();
        }

        /// <summary>
        /// Instance - returns instance of the singleton class.
        /// </summary>
        public static ImageServiceClient Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new ImageServiceClient();
                }
                return m_instance;
            }
        }

        /// <summary>
        /// Start function.
        /// starts the tcp connection.
        /// </summary>
        /// <returns></returns>
        private bool Start()
        {
            try
            {
                bool result = true;
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                client = new TcpClient();
                client.Connect(ep);
                Console.WriteLine("You are connected");
                m_isStopped = false;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// SendCommand function.
        /// sends command to srv.
        /// </summary>
        /// <param name="commandRecievedEventArgs">info to be sented to server</param>
        public void SendCommand(CommandRecievedEventArgs commandRecievedEventArgs)
        {
            //new Task(() =>
            //{
                try
                {
                    string jsonCommand = JsonConvert.SerializeObject(commandRecievedEventArgs);
                    NetworkStream stream = client.GetStream();
                    BinaryWriter writer = new BinaryWriter(stream);
                        // Send data to server
                        Console.WriteLine($"Send {jsonCommand} to Server");
                    m_mutex.WaitOne();
                    writer.Write(jsonCommand);
                    m_mutex.ReleaseMutex();
                }
                catch (Exception ex)
                {

                }
            //}).Start();
        }

        /// <summary>
        /// RecieveCommand function.
        /// creates task and reads new messages.
        /// </summary>
        public void RecieveCommand()
        {
            new Task(() =>
            {
                try
                {
                    while (!m_isStopped)
                    {
                        NetworkStream stream = client.GetStream();
                        BinaryReader reader = new BinaryReader(stream);
                        m_readMutex.WaitOne();
                        string response = reader.ReadString();
                        m_readMutex.ReleaseMutex();
                        Console.WriteLine($"Recieve {response} from Server");
                        CommandRecievedEventArgs responseObj = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(response);
                        this.UpdateResponse?.Invoke(responseObj);
                    }
                }
                catch (Exception ex)
                {

                }
            }).Start();
        }

        /// <summary>
        /// CloseClient function.
        /// closes the client.
        /// </summary>
        public void CloseClient()
        {
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.HandlerShutDown, null, "");
            this.SendCommand(commandRecievedEventArgs);
            client.Close();
            this.m_isStopped = true;
        }
    }
}


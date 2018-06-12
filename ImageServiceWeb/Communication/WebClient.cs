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
    public class WebClient : IWebClient
    {
        private TcpClient client;
        public bool IsConnected { get; set; }
        private static WebClient instance;
        private bool isStopped;
        private static Mutex mutex = new Mutex();
        private static Mutex readMutex = new Mutex();
        public delegate void UpdateDataIn(CommandRecievedEventArgs data);
        public event UpdateDataIn UpdateData;

        /// <summary>
        /// WebClient private constructor.
        /// </summary>
        private WebClient()
        {
            IsConnected = Start();
        }

        /// <summary>
        /// Instance - returns instance of the singleton class.
        /// </summary>
        public static WebClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WebClient();
                }
                return instance;
            }
        }

        /// <summary>
        /// Start function.
        /// starts the tcp connection.
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                client = new TcpClient();
                client.Connect(ep);
                Console.WriteLine("The client is connected");
                isStopped = false;
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }
        }


        /// <summary>
        /// RecieveMessage function.
        /// creates task and reads new messages.
        /// </summary>
        public void RecieveMessage()
        {
            new Task(() =>
            {
                try
                {
                    while (!isStopped)
                    {
                        NetworkStream stream = client.GetStream();
                        BinaryReader reader = new BinaryReader(stream);
                        readMutex.WaitOne();
                        string answer = reader.ReadString();
                        readMutex.ReleaseMutex();
                        Console.WriteLine($"Recieve {answer} from Server");
                        CommandRecievedEventArgs answerObject = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(answer);
                        this.UpdateData?.Invoke(answerObject);
                    }
                }
                catch (Exception )
                {
                    Console.WriteLine("Error in trying to read from the server ");
                }
            }).Start();
        }

        /// <summary>
        /// SendMessage function.
        /// sends message to the server.
        /// </summary>
        /// <param name="commandRecievedEventArgs">message to be sent to the server</param>
        public void SendMessage(CommandRecievedEventArgs msg)
        {
                try
                {
                    string jsonCommand = JsonConvert.SerializeObject(msg);
                    NetworkStream stream = client.GetStream();
                    BinaryWriter writer = new BinaryWriter(stream);
                        // Send data to server
                        Console.WriteLine($"Send {jsonCommand} to Server");
                    mutex.WaitOne();
                    writer.Write(jsonCommand);
                    mutex.ReleaseMutex();
                }
                catch (Exception )
                {

                }
        }


        /// <summary>
        /// CloseClient function.
        /// closes the client.
        /// </summary>
        public void CloseClient()
        {
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.HandlerShutDown, null, "");
            SendMessage(commandRecievedEventArgs);
            client.Close();
            isStopped = true;
        }
    }
}


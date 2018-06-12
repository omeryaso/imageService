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
        /// WebClient private constructor (singelton).
        /// </summary>
        private WebClient()
        {
            IsConnected = Start();
        }

        /// <summary>
        /// Instance - returns the instance of the WebClient singleton .
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
        /// starts the client
        /// </summary>
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
        /// using a task to recieve a message from the server
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
                        Console.WriteLine($"Recieved {answer} from Server");
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
        /// <param name="commandRecievedEventArgs">info to be sent to server</param>
        public void SendMessage(CommandRecievedEventArgs msg)
        {
                try
                {
                    NetworkStream stream = client.GetStream();
                    BinaryWriter writer = new BinaryWriter(stream);
                    string jsonCommand = JsonConvert.SerializeObject(msg);
                    Console.WriteLine($"Sent {jsonCommand} to Server");
                    mutex.WaitOne();
                    writer.Write(jsonCommand);
                    mutex.ReleaseMutex();
                }
                catch (Exception)
                {
                Console.WriteLine("Error in trying to write to the server");
                }
        }


        /// <summary>
        /// CloseClient function.
        /// closes the client.
        /// </summary>
        public void CloseClient()
        {
            isStopped = true;
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.HandlerShutDown, null, "");
            SendMessage(commandRecievedEventArgs);
            client.Close();
        }
    }
}


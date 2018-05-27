using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageServiceGUI.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ImageServiceGUI
{
    class GUIClient : IGUIClient
    {
        public bool IsConnected { get; set; }
        private static GUIClient instance;
        private bool isStopped;
        private static Mutex Clientmutex = new Mutex();
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

       // bool IGUIClient.IsConnected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event UpdateDataIn UpdateData;
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
            }            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }        }

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
                        string answer = "humus";
                        Console.WriteLine(answer);
                        answer = reader.ReadString();
                        Console.WriteLine("message is: " + answer);
                        Console.WriteLine($"Recieve {answer} from Server");
                        CommandRecievedEventArgs answerObject = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(answer);
                        UpdateData?.Invoke(answerObject);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in trying to reading to the client ");
                }
            }).Start();
        }

        public void SendMessage(CommandRecievedEventArgs msg)
        {
            new Task(() =>
            {
                try
                {
                NetworkStream stream = client.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);
                string json = JsonConvert.SerializeObject(msg);
                 //   MessageBox.Show(json);
                Console.WriteLine($"Send1 {json} to Server");
                    Clientmutex.WaitOne();
                    try {
                        writer.Write(json);
                    }
                    catch (Exception)
                    {
                //        MessageBox.Show(json+"bla");
                    }
                    Clientmutex.ReleaseMutex();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error in trying to write to the client");
                }

            }).Start();
        }

        public void CloseClient()
        {
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.DisconnectClient, null, "");
            SendMessage(commandRecievedEventArgs);
            client.Close();
            isStopped = true;

        }
    }
}

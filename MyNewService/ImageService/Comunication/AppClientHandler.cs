using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Comunication
{
    class AppClientHandler : IAppClientHandler
    {

        IImageController ImageController { get; set; }
        ILoggingService Logging { get; set; }
        private bool IsStopped = false;
        public static Mutex Mutex { get; set; }

        /// <summary>
        /// creates an instance of the AppClientHandler
        /// </summary>
        /// <param name="imageController"></param> the controller of the imageService
        /// <param name="logging"></param> the logger of the servicse
        public AppClientHandler(IImageController imageController, ILoggingService logging)
        {
            ImageController = imageController;
            Logging = logging;
        }

        /// <summary>
        /// handle the client - transfer the photos from the phone to the output directory
        /// </summary>
        /// <param name="client"></param> the client of the service
        /// <param name="clients"></param> the list of the clients
        public void HandleClient(TcpClient client, List<TcpClient> clients)
        {
            try
            {
                new Task(() =>
                {
                    try
                    {
                        while (!IsStopped)
                        {
                            Logging.Log("transfering the photos from the phone...", MessageTypeEnum.INFO);
                            NetworkStream stream = client.GetStream();
                            string fileName = GetName(stream);
                            Byte[] tmp = { 1 };
                            stream.Write(tmp, 0, 1);
                            List<Byte> image = GetImage(stream);
                            File.WriteAllBytes(ImageController.ImageServer.Directories[0] + @"\" + fileName + ".jpg", image.ToArray());
                            Logging.Log("transfered "+ fileName+ " successfuly", MessageTypeEnum.INFO);
                        }
                    }
                    catch (Exception ex)
                    {
                        clients.Remove(client);
                        Logging.Log(ex.ToString(), MessageTypeEnum.FAIL);
                        client.Close();
                    }

                }).Start();
            }
            catch (Exception ex)
            {
                Logging.Log(ex.ToString(), MessageTypeEnum.FAIL);

            }

        }

        /// <summary>
        /// gets the current image name
        /// </summary>
        /// <param name="stream"></param> the stream to the app client
        /// <returns></returns>the image name
        private static string GetName(NetworkStream stream)
        {
            Byte[] temp = new Byte[1];
            List<Byte> fileName = new List<byte>();
            do
            {
                stream.Read(temp, 0, 1);
                fileName.Add(temp[0]);
            } while (stream.DataAvailable);
            return Path.GetFileNameWithoutExtension(System.Text.Encoding.UTF8.GetString(fileName.ToArray()));
        }

        /// <summary>
        /// gets the image in list of bytes
        /// </summary>
        /// <param name="stream"></param> the stream to the app client
        /// <returns></returns> the image casted to bytes
        private List<Byte> GetImage(NetworkStream stream)
        {
            List<Byte> image = new List<byte>();
            Byte[] data = new Byte[7000];
            int i;
            do
            {
                i = stream.Read(data, 0, data.Length);
                for (int j = 0; j < i; j++)
                    image.Add(data[j]);               
                Thread.Sleep(400);
            } while (stream.DataAvailable || i == data.Length);
            return image;
        }
    }
}

﻿using ImageService.Controller;
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

        public AppClientHandler(IImageController imageController, ILoggingService logging)
        {
            ImageController = imageController;
            Logging = logging;
        }

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
                            Logging.Log("Start transfer photos!", MessageTypeEnum.INFO);
                            NetworkStream stream = client.GetStream();
                            //get the image name
                            Byte[] temp = new Byte[1];
                            List<Byte> fileName = new List<byte>();
                            //read the file name
                            do
                            {
                                stream.Read(temp, 0, 1);
                                fileName.Add(temp[0]);
                            } while (stream.DataAvailable);
                            string finalNameString = Path.GetFileNameWithoutExtension(System.Text.Encoding.UTF8.GetString(fileName.ToArray()));
                            //tell the client we got the name 
                            Byte[] confirmation = new byte[1];
                            confirmation[0] = 1;
                            stream.Write(confirmation, 0, 1);
                            //read the image
                            List<Byte> finalbytes = GetImageBytes(stream);
                            //save the image
                            File.WriteAllBytes(ImageController.ImageServer.Directories[0] + @"\" + finalNameString + ".jpg", finalbytes.ToArray());
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
        private List<Byte> GetImageBytes(NetworkStream stream)
        {
            List<Byte> bytesArr = new List<byte>();
            Byte[] tempForReadBytes;
            Byte[] data = new Byte[6790];
            int i = 0;
            //start reading the bytes in parts to get the whole image
            do
            {
                i = stream.Read(data, 0, data.Length);
                tempForReadBytes = new byte[i];
                for (int n = 0; n < i; n++)
                {
                    tempForReadBytes[n] = data[n];
                    bytesArr.Add(tempForReadBytes[n]);

                }
                Thread.Sleep(300);
            } while (stream.DataAvailable || i == data.Length);
            return bytesArr;
        }
    }
}

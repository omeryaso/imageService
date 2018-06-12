using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {
        private static Communication.IWebClient WebClient { get; set; }
        public delegate void NotifyAboutChange();
        public event NotifyAboutChange Notify;


        /// <summary>
        /// constructor.
        /// initialize new config params.
        /// </summary>
        public ConfigModel()
        {
            try
            {
                ThumbnailSize = 1;
                string[] str = { "", "", "" };
                initCRE(str);
                Handlers = new ObservableCollection<string>();
                Enabled = false;

                WebClient = Communication.WebClient.Instance;
                WebClient.RecieveMessage();
                WebClient.UpdateData += (CommandRecievedEventArgs res) =>
                {
                    try
                    {
                        if (res == null)
                            return;

                        if (res.CommandID == (int)CommandEnum.GetConfigCommand)
                            WriteConfig(res);
                        else if (res.CommandID == (int)CommandEnum.HandlerShutDown)
                            HandlerClose(res);
                        Notify?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ConfigModel: " + e);
                    }
                };

                WebClient.SendMessage(new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, new string[5], ""));
            }
            catch (Exception e)
            {
                Console.WriteLine("ConfigModel: " + e);
            }
        }
        /// <summary>
        /// this method will delete the given handler.
        /// </summary>
        /// <param name="handler"></param>
        public void DeleteHandler(string handler)
        {
            string[] arr = { handler };
            try
            {
                WebClient.SendMessage(new CommandRecievedEventArgs((int)CommandEnum.HandlerShutDown, arr, ""));
            }
            catch (Exception e)
            {
                Console.WriteLine("ConfigModel - DeleteHandler: " + e);
            }
        }

        /// <summary>
        /// this method will close the given handler.
        /// </summary>
        /// <param name="responseObj">the info came from srv</param>
        private void HandlerClose(CommandRecievedEventArgs responseObj)
        {
            if (Handlers != null && Handlers.Count > 0 && responseObj != null && responseObj.Args != null
                                 && Handlers.Contains(responseObj.Args[0]))
            {
                this.Handlers.Remove(responseObj.Args[0]);
            }
        }


        /// <summary>
        /// UpdateConfigurations function.
        /// updates app config params.
        /// </summary>
        /// <param name="msg">the info came from srv</param>
        private void WriteConfig(CommandRecievedEventArgs msg)
        {
            try
            {
                string[] str = { msg.Args[0], msg.Args[1], msg.Args[2]};
                initCRE(str);
                int.TryParse(msg.Args[3], out int num);
                ThumbnailSize = num;
                string[] handlers = msg.Args[4].Split(';');
                handlerAdder(handlers);
            }
            catch (Exception e)
            {
                Console.WriteLine("ConfigModel - DeleteHandler: " + e);
            }
        }

        private void handlerAdder(string[] handlers)
        {
            foreach (string handler in handlers)
            {
                if (!Handlers.Contains(handler))
                {
                    Handlers.Add(handler);
                }
            }

        }
        private void initCRE(String[] str)
        {
            OutputDir = str[0];
            SrcName = str[1];
            LogName = str[2];
        }

        //members
        [Required]
        [DataType(DataType.Text)]
        public bool Enabled { get; set; }

        [Required]
        [Display(Name = "Output Directory")]
        public string OutputDir { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name")]
        public string SrcName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name")]
        public string LogName { get; set; }

        [Required]
        [Display(Name = "Thumbnail Size")]
        public int ThumbnailSize { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Handlers")]
        public ObservableCollection<string> Handlers { get; set; }

    }
}
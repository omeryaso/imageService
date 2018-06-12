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
                WebClient = Communication.WebClient.Instance;
                WebClient.RecieveMessage();
                WebClient.UpdateData += (CommandRecievedEventArgs res) =>
                {
                    try
                    {
                        if (res == null)
                            return;

                        if (res.CommandID == (int)CommandEnum.GetConfigCommand)
                            UpdateConfigurations(res);
                        else if (res.CommandID == (int)CommandEnum.HandlerShutDown)
                            HandlerClose(res);
                        Notify?.Invoke();
                        //update controller
                        Notify?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ConfigModel: " + e);
                    }
                };

                OutputDirectory = LogName = SourceName = "";
                ThumbnailSize = 1;
                Handlers = new ObservableCollection<string>();
                Enabled = false;
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
        /// <param name="responseObj">the info came from srv</param>
        private void UpdateConfigurations(CommandRecievedEventArgs responseObj)
        {
            try
            {
                OutputDirectory = responseObj.Args[0];
                SourceName = responseObj.Args[1];
                LogName = responseObj.Args[2];
                int num;
                int.TryParse(responseObj.Args[3], out num);
                ThumbnailSize = num;
                string[] handlers = responseObj.Args[4].Split(';');
                foreach (string handler in handlers)
                {
                    if (!Handlers.Contains(handler))
                    {
                        Handlers.Add(handler);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        //members
        [Required]
        [DataType(DataType.Text)]
        public bool Enabled { get; set; }

        [Required]
        [Display(Name = "Output Directory")]
        public string OutputDirectory { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name")]
        public string SourceName { get; set; }

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
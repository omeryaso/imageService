﻿using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ImageServiceWeb.Models
{
    public class Config
    {
        public delegate void NotifyAboutChange();
        public event NotifyAboutChange Notify;
        private static Communication.IWebClient WebClient { get; set; }


        /// <summary>
        /// constructor.
        /// initialize new config params.
        /// </summary>
        public Config()
        {
            try
            {
                WebClient = Communication.WebClient.Instance;
                WebClient.RecieveMessage();
                WebClient.UpdateData += UpdateResponse;
                SourceName = "";
                LogName = "";
                OutputDirectory = "";
                ThumbnailSize = 1;
                Handlers = new ObservableCollection<string>();
                Enabled = false;
                string[] arr = new string[5];
                CommandRecievedEventArgs request = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, arr, "");
                WebClient.SendMessage(request);
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// DeleteHandler function.
        /// deletes handler.
        /// </summary>
        /// <param name="toBeDeleted"></param>
        public void DeleteHandler(string toBeDeleted)
        {
            try
            {
                string[] arr = { toBeDeleted };
                CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs((int)CommandEnum.HandlerShutDown, arr, "");
                WebClient.SendMessage(eventArgs);
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// UpdateResponse function.
        /// updates the model when message recieved from srv.
        /// </summary>
        /// <param name="responseObj">the info came from srv</param>
        private void UpdateResponse(CommandRecievedEventArgs responseObj)
        {
            try
            {
                if (responseObj != null)
                {
                    switch (responseObj.CommandID)
                    {
                        case (int)CommandEnum.GetConfigCommand:
                            UpdateConfigurations(responseObj);
                            break;
                        case (int)CommandEnum.HandlerShutDown:
                            CloseHandler(responseObj);
                            break;
                    }
                    //update controller
                    Notify?.Invoke();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// CloseHandler function.
        /// </summary>
        /// <param name="responseObj">the info came from srv</param>
        private void CloseHandler(CommandRecievedEventArgs responseObj)
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
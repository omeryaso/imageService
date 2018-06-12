using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using ImageServiceWeb.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class LogCollection
    {
        public delegate void NotifyAboutChange();
        public event NotifyAboutChange Notify;
        private static Communication.IWebClient WebClient;

        /// <summary>
        /// LogCollection constructor.
        /// initialize new Log list.
        /// </summary>
        public LogCollection()
        {
            try
            {
                WebClient = Communication.WebClient.Instance;
                WebClient.UpdateData += UpdateResponse;
                WebClient.RecieveMessage();
                this.InitializeLogsParams();
            }
            catch (Exception ex)
            {

            }
        }

        //members
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Enteries")]
        public List<Log> LogEntries { get; set; }

        /// <summary>
        /// retreive event log entries list from the image service.
        /// </summary>
        private void InitializeLogsParams()
        {
            try
            {
                LogEntries = new List<Log>();
                CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
                WebClient.SendMessage(commandRecievedEventArgs);
            }
            catch (Exception )
            {

            }
        }

        /// <summary>
        /// get CommandRecievedEventArgs object which was sent from the image service.
        /// reacts only if the commandID is relevant to the log model.
        /// </summary>
        /// <param name="responseObj"></param>
        private void UpdateResponse(CommandRecievedEventArgs responseObj)
        {
            try
            {
                if (responseObj != null)
                {
                    switch (responseObj.CommandID)
                    {
                        case (int)CommandEnum.LogCommand:
                            IntializeLogEntriesList(responseObj);
                            break;
                        case (int)CommandEnum.NewLog:
                            AddLogEntry(responseObj);
                            break;
                        default:
                            break;
                    }
                    Notify?.Invoke();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Initialize log event entries list.
        /// </summary>
        /// <param name="responseObj">expected json string of ObservableCollection<LogEntry> in responseObj.Args[0]</param>
        private void IntializeLogEntriesList(CommandRecievedEventArgs responseObj)
        {
            try
            {
                foreach (Log log in JsonConvert.DeserializeObject<ObservableCollection<Log>>(responseObj.Args[0]))
                {
                    LogEntries.Add( new Log { Type = log.Type, Message = log.Message } );
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// adds new log entry to the event log entries list
        /// </summary>
        /// <param name="responseObj">expected responseObj.Args[0] = EntryType,  responseObj.Args[1] = Message </param>
        private void AddLogEntry(CommandRecievedEventArgs responseObj)
        {
            try
            {
                Log newLogEntry = new Log { Type = responseObj.Args[0], Message = responseObj.Args[1] };
                this.LogEntries.Insert(0, new Log { Type = newLogEntry.Type, Message = newLogEntry.Message });
            }
            catch (Exception ex)
            {
            }
        }
    }
}
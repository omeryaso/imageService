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
        /// creates an instance of LogCollection
        /// </summary>
        public LogCollection()
        {
            try
            {
                WebClient = Communication.WebClient.Instance;
                WebClient.UpdateData += UpdateData;
                WebClient.RecieveMessage();
                StartLogs();
            }
            catch (Exception e)
            {
                Console.WriteLine("LogCollection - LogCollection: " + e);
            }
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Enteries")]
        public List<Log> LogEntries { get; set; }



        /// <summary>
        /// update the corresponing log entry to the web app
        /// </summary>
        /// <param name="msg"></param>
        private void UpdateData(CommandRecievedEventArgs msg)
        {
            try
            {
                if (msg != null)
                {
                    if (msg.CommandID == (int)CommandEnum.LogCommand)
                        InsertLogsEntries(msg);
                    else if (msg.CommandID == (int)CommandEnum.NewLog)
                        AddLogData(msg);
                }
                    Notify?.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine("LogCollection - UpdateData: " + e);
            }
        }

        /// <summary>
        /// set up all the log files
        /// </summary>
        /// <param name="msg">all the log entries in Json
        private void InsertLogsEntries(CommandRecievedEventArgs msg)
        {
            try
            {
                foreach (Log log in JsonConvert.DeserializeObject<ObservableCollection<Log>>(msg.Args[0]))
                {
                    LogEntries.Add( new Log { Type = log.Type, Message = log.Message } );
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("LogCollection - InsertLogsEntries: " + e);

            }
        }

        /// <summary>
        /// gets the log enteries from the service
        /// </summary>
        private void StartLogs()
        {
            try
            {
                LogEntries = new List<Log>();
                CommandRecievedEventArgs args = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
                WebClient.SendMessage(args);
            }
            catch (Exception e)
            {
                Console.WriteLine("LogCollection - StartLogs: " + e);
            }
        }

        /// <summary>
        /// adds new log entry to the event log entries list
        /// </summary>
        /// <param name="msg">expected responseObj.Args[0] = EntryType,  responseObj.Args[1] = Message </param>
        private void AddLogData(CommandRecievedEventArgs msg)
        {
            try
            {
                Log newLogEntry = new Log { Type = msg.Args[0], Message = msg.Args[1] };
                LogEntries.Insert(0, new Log { Type = newLogEntry.Type, Message = newLogEntry.Message });
            }
            catch (Exception e)
            {
                Console.WriteLine("LogCollection - AddLogData: " + e);
            }
        }

        /// <summary>
        /// Log class - this is the log object
        /// </summary>
        public class Log
        {
            //members
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Type")]
            public string Type { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Message")]
            public string Message { get; set; }


        }

        /// <summary>
        /// enum to the logs types
        /// </summary>
        public enum Type
        {
            INFO,
            FAIL,
            WARNING
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using ImageServiceGUI.Communication;
using Newtonsoft.Json;

namespace ImageServiceGUI.Model
{
    class LogsModel : ILogsModel
    {
        public IGUIClient Client { get ; set ; }
        private ObservableCollection<Log> logList;
        public ObservableCollection<Log> LogList {
            get { return logList; }
            set
            {
                logList = value;
                NotifyPropertyChanged("LogsList");
            }
        }
        public LogsModel()
        {
            Client = GUIClient.Instance;
            Client.RecieveMessage();
            Client.UpdateData += UpdateData;
            StartLogs();
        }

        private void StartLogs()
        {
            try
            {
                LogList = new ObservableCollection<Log>();
                Object Lock = new Object();
                BindingOperations.EnableCollectionSynchronization(LogList, Lock);
                CommandRecievedEventArgs msg = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
                Client.SendMessage(msg);
            }
            catch (Exception)
            {
                Console.WriteLine("Error in trying to write to the LogsModel");
            }
        }

        private void UpdateData(CommandRecievedEventArgs data)
        {
            if (data != null)
            {
                switch (data.CommandID)
                {
                    case (int)CommandEnum.NewLog:
                        AddLogData(data);
                        break;
                    case (int)CommandEnum.LogCommand:
                        InsertLogsEntries(data);
                        break;
                    default:
                        break;
                }
            }
        }

        private void AddLogData(CommandRecievedEventArgs data)
        {
            try
            {
                Log log = new Log { Type = data.Args[0], Message = data.Args[1] };
                LogList.Insert(0, log);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void InsertLogsEntries(CommandRecievedEventArgs data)
        {
            try
            {
                foreach (Log log in JsonConvert.DeserializeObject<ObservableCollection<Log>>(data.Args[0]))
                {
                    LogList.Add(log);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}

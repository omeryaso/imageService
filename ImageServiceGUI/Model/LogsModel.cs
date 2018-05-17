using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace ImageServiceGUI.Model
{
    class LogsModel : ILogsModel
    {
        private ObservableCollection<Log> logList;
        public ObservableCollection<Log> LogList {
            get { return this.logList; }
            set
            {
                this.logList = value;
                NotifyPropertyChanged("LogsList");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LogsModel()
        {
            logList = new ObservableCollection<Log>();
            logList.Add(new Log((int)MessageTypeEnum.INFO, "hello every1"));
            logList.Add(new Log((int)MessageTypeEnum.INFO, "hello every2"));
            logList.Add(new Log((int)MessageTypeEnum.INFO, "hello every3"));
            logList.Add(new Log((int)MessageTypeEnum.INFO, "hello every4"));
        }

        private void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}

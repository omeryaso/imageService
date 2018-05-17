using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace ImageServiceGUI.ViewModel
{
    class LogsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ILogsModel logsModel;
        public ObservableCollection<Log> VM_LogsList
        {
            get { return logsModel.LogList; }
            //set {
              //  logsModel.LogList = value;
                //PropertyChanged("LogsList");    
                //}
        }

        public LogsViewModel()
        {
            this.logsModel = new LogsModel();
            logsModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };

        }

        private void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}

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
    interface ILogsModel : INotifyPropertyChanged
    {
      
        ObservableCollection<Log> LogList { get; set; }
    }
}

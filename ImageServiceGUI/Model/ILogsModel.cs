using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;
using ImageServiceGUI.Communication;

namespace ImageServiceGUI.Model
{
    interface ILogsModel : INotifyPropertyChanged
    {
        IGUIClient Client { get; set; }
        ObservableCollection<Log> LogList { get; set; }
    }
}

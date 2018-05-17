using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    interface ISettingsModel : INotifyPropertyChanged
    {
        string OutDirectory { get; set; }
        string SrcName { get; set; }
        string LogName { get; set; }
        string ThumbSize { get; set; }
        void HandlerClose(string handler);
        ObservableCollection<string> Handlers {get; set;}

    }
}

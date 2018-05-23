using ImageServiceGUI.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    interface IMainWindowModel : INotifyPropertyChanged
    {
        string Background { get; set; }
        IGUIClient Client { get; set; }
    }
}

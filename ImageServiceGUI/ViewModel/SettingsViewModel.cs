using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private ISettingsModel model;

        public string VM_OutDirectory { get { return model.OutDirectory; } }
        public string VM_SrcName { get { return model.SrcName; } }
        public string VM_LogName { get { return model.LogName; } }
        public string VM_ThumbSize { get { return model.ThumbSize; } }
        public ObservableCollection<string> VM_Handlers { get { return model.Handlers; } }

        public SettingsViewModel(ISettingsModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_"+e.PropertyName);
            };
            
        }
        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void VM_HandlerClose(string handler)
        {
            model.HandlerClose(handler);
        }
        
    }
}

using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private IMainWindowModel model;

        public MainWindowViewModel(IMainWindowModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        private void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public string VM_Background { get { return model.Background; } }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

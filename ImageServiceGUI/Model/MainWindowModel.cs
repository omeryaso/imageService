using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGUI.Communication;

namespace ImageServiceGUI.Model
{
    class MainWindowModel : IMainWindowModel
    {
        private bool isConnected;
        private string background;

        public MainWindowModel()
        {
            Client = GUIClient.Instance;
            isConnected = Client.IsConnected;
            if (isConnected)
                Background = "White";
            else
            {
                Background = "Gray";
            }
        }
        public string Background { get { return background; }
            set { background = value;
                NotifyPropertyChanged("Background");
            }
        }

        public IGUIClient Client { get ;set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}

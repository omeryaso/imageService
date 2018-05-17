using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    class SettingsModel : ISettingsModel
    {
        public SettingsModel()
        {
            OutDirectory = "Omi";
            SrcName = "The";
            LogName = "King";
            ThumbSize = "!!!";
            Handlers = new ObservableCollection<string>();
            Handlers.Add("Moshik");
            Handlers.Add("Barvaz");
        }

        private string outDirectory;
        public string OutDirectory { get { return outDirectory; }
        set{
                outDirectory = value;
                NotifyPropertyChanged("OutDirectory");
            }
        }
        private string srcName;
        public string SrcName { get { return srcName; }
        set{
                srcName = value;
                NotifyPropertyChanged("SrcName");
            }
        }
        private string logName;
        public string LogName { get { return logName; }
        set{
                logName = value;
                NotifyPropertyChanged("LogName");
            }
        }
        private string thumbSize;


        public string ThumbSize{ get { return thumbSize; }
        set{
                thumbSize = value;
                NotifyPropertyChanged("ThumbSize");
            }
        }
        public ObservableCollection<string> Handlers { get; set; }
      

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void HandlerClose(string handler)
        {
            Handlers.Remove(handler);
        }
    }
}

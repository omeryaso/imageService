using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageServiceGUI.Communication;

namespace ImageServiceGUI.Model
{
    class SettingsModel : ISettingsModel
    {
        public IGUIClient Client { get ; set ; }
        public SettingsModel()
        {
            Client = GUIClient.Instance;
            Client.RecieveMessage();
            Client.UpdateData += UpdateData;
            StartSettings();
        }

        private void StartSettings()
        {
            try
            {
                OutDirectory = string.Empty;
                SrcName = string.Empty;
                LogName = string.Empty;
                ThumbSize = string.Empty;
                Handlers = new ObservableCollection<string>();
                //string for getting the data from the server
                string[] arr = new string[5];
                Object Lock = new Object();
                BindingOperations.EnableCollectionSynchronization(Handlers, Lock);
                CommandRecievedEventArgs msg = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, arr, "");
                Client.SendMessage(msg);
            }
            catch (Exception)
            {
                Console.WriteLine("Error in trying to write to the View");
            }
        }
    

        private void WriteSettings(CommandRecievedEventArgs msg)
        {

            try
            {
                OutDirectory = msg.Args[0];
                SrcName = msg.Args[1];
                LogName = msg.Args[2];
                ThumbSize = msg.Args[3];
                string[] handlers = msg.Args[4].Split(';');
                foreach (string handler in handlers)
                {
                    Handlers.Add(handler);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error in trying to write to the View");
            }
        }


        private void UpdateData(CommandRecievedEventArgs data)
        {
            {
                try
                {
                    if (data != null)
                    {
                        switch (data.CommandID)
                        {
                            case (int)CommandEnum.HandlerShutDown:

                                Handlers.Remove(data.Args[0]);
                                break;
                            case (int)CommandEnum.GetConfigCommand:
                                WriteSettings(data);
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error in trying to update the data in SettingsModel");
                }
            }
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void HandlerClose(string handler)
        {
            Console.WriteLine(handler);
            string[] Args = { handler };
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.HandlerShutDown, Args, handler);
            Client.SendMessage(commandRecievedEventArgs);
        }
    }
}

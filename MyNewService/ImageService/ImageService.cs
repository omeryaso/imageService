using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Server;
using ImageService.Modal;
using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Configuration;

public enum ServiceState
{
    SERVICE_STOPPED = 0x00000001,
    SERVICE_START_PENDING = 0x00000002,
    SERVICE_STOP_PENDING = 0x00000003,
    SERVICE_RUNNING = 0x00000004,
    SERVICE_CONTINUE_PENDING = 0x00000005,
    SERVICE_PAUSE_PENDING = 0x00000006,
    SERVICE_PAUSED = 0x00000007,
}

[StructLayout(LayoutKind.Sequential)]
public struct ServiceStatus
{
    public int dwServiceType;
    public ServiceState dwCurrentState;
    public int dwControlsAccepted;
    public int dwWin32ExitCode;
    public int dwServiceSpecificExitCode;
    public int dwCheckPoint;
    public int dwWaitHint;
};

namespace ImageService
{

    public partial class ImageService : ServiceBase
    {
        private ImageServer m_imageServer;          // The Image Server
        private IImageServiceModal modal;
        private IImageController contr;
        private ILoggingService logging;
        private System.ComponentModel.IContainer components;
        private System.Diagnostics.EventLog eventLog1;
        private int eventId = 1;
        public ImageService(string[] args)
        {
            InitializeComponent();
            string eventSourceName = ConfigurationManager.AppSettings.Get("SourceName");
            string logName = ConfigurationManager.AppSettings.Get("LogName");
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
            this.logging = new LoggingService();
            this.logging.MessageRecieved += this.Logwrite;
            this.modal = new ImageServiceModal(ConfigurationManager.AppSettings["OutputDir"], Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize")));
            this.contr = new ImageController(this.modal);
            this.m_imageServer = new ImageServer(this.contr, this.logging);

        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  

            eventLog1.WriteEntry("Start Pending");
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLog1.WriteEntry("In OnStart");

        }

        protected override void OnStop()
        {
            this.logging.MessageRecieved -= this.Logwrite;
            eventLog1.WriteEntry("In onStop.");
        }
        /*
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }
        */
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        private void Logwrite(object sender, MessageRecievedEventArgs e)
        {
            this.eventLog1.WriteEntry(e.Message);
        }
    }
}

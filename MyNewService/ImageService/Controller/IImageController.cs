using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// IImageController interface. resposible to execute the Command Requet
    /// </summary>
    public interface IImageController
    {
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet
        ImageServer ImageServer { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    /// <summary>
    /// DirectoryCloseEventArgs class implements EventArgs
    /// </summary>
    public class DirectoryCloseEventArgs : EventArgs
    {
        public string DirectoryPath { get; set; }

        public string Message { get; set; }             // The Message That goes to the logger

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="message"></param>
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;                    // Setting the Directory Name
            Message = message;                          // Storing the String
        }

    }
}

using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// inteface Command. executing commands
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// The fuction recives args and execuiting using it. the outcume will be written to
        /// the result bollean that she get (must be initialize). true for success false for failiure. 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        string Execute(string[] args, out bool result);          // The Function That will Execute The 
    }
}

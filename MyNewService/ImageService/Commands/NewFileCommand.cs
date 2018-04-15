using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImageService.Commands
{
    /// <summary>
    /// NewFileCommand implements Icommand
    /// </summary>
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        /// <summary>
        /// the constructor of the class
        /// </summary>
        /// <param name="modal"></param>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }
        
        /// <summary>
        /// implemention of Execute.
        /// returns the value of the use of AddFile function of the m_modal member.  
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            return m_modal.AddFile(args[0], out result);
        }
    }
}

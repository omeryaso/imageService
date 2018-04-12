using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        #endregion
        public ImageServiceModal(string path, int size)
        {
            m_OutputFolder = path;
            m_thumbnailSize = size;
        }

        public ImageServiceModal()
        {
        }

        public string AddFile(string path, out bool result)
        {
            if (File.Exists(path))
            {
                string outPath, thumbOutPath;
                DirectoryCreator(path, out outPath, out thumbOutPath);
                outPath = outPath + path.Substring(path.LastIndexOf("\\"));
                thumbOutPath = thumbOutPath + path.Substring(path.LastIndexOf("\\"));
                thumbOutPath = Path.ChangeExtension(thumbOutPath, "thumb");
                //TODO: change the implementation
                outPath = ChecksifExists(outPath);
                thumbOutPath = ChecksifExists(thumbOutPath);
                //move the imgaes
                Move(path, outPath, thumbOutPath);
                result = true;
                return "Moved the image successfully from " + path + "to " + outPath;
            }

            result = false;
            return "The image is doesn't exist in " + path;
        }

        private void Move(string path, string outPath, string thumbOutPath)
        {
            //save the image as a thumbnail
            Image image = Image.FromFile(path);
            Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);

            thumb.Save(thumbOutPath);
            image.Dispose();
            thumb.Dispose();

            //move the files
            Directory.Move(path, outPath);
        }

        //TODO: change the implementation

        private string ChecksifExists(string path)
        {
            int count = 1;
            string fileNameOnly = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            string directoryPath = Path.GetDirectoryName(path);

            while (File.Exists(path))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                path = Path.Combine(directoryPath, tempFileName + extension);
            }
            return path;        
        }

        private void DirectoryCreator(string path, out string outPath, out string thumbOutPath)
        {
            DateTime created = new DateTime();
            // try to get the date from the file
            try
            {
                created = File.GetCreationTime(path);
            }
            catch (Exception e)
            {
                throw e;
            }
            int iMonth = created.Month;
            int year = created.Year;
            string month = ImonthToString(iMonth);
            string yearMonth = year.ToString() + "\\" + month;
            outPath = m_OutputFolder + "\\" + yearMonth;
            thumbOutPath = m_OutputFolder + "\\Thumbnails" + "\\" + yearMonth;

            // creates the output directory if it doesn't exist (as a hidden directory)
            DirectoryInfo dirInfo = Directory.CreateDirectory(m_OutputFolder);
            dirInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

            // creates directories to the images and thumbnails
            Directory.CreateDirectory(outPath);
            Directory.CreateDirectory(thumbOutPath);
           
        }

        private string ImonthToString(int iMonth)
        {
            switch (iMonth)
            {
                case (1):
                    return "January";
                case (2):
                    return "February";
                case (3):
                    return "March";
                case (4):
                    return "April";
                case (5):
                    return "May";
                case (6):
                    return "June";
                case (7):
                    return "July";
                case (8):
                    return "August";
                case (9):
                    return "September";
                case (10):
                    return "October";
                case (11):
                    return "November";
                case (12):
                    return "December";
                default:
                    return "Error";
            }
        }
    }
}
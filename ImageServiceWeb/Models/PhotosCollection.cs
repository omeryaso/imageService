using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using static ImageServiceWeb.Models.ConfigModel;

namespace ImageServiceWeb.Models
{
    public class PhotosCollection
    {
        public event ChangeNotifyer NotifyEvent;
        private static ConfigModel m_config;
        private string m_outputDir;
        public List<Photo> PhotosList = new List<Photo>();

        /// <summary>
        /// constructor.
        /// </summary>
        public PhotosCollection()
        {
            m_config = new ConfigModel();
            m_config.Notify += Notify;
        }
        /// <summary>
        /// Notify function.
        /// notify controller about update.
        /// </summary>
        void Notify()
        {
            if (m_config.OutputDir != "")
            {
                m_outputDir = m_config.OutputDir;
                GetPhotos();
                NotifyEvent?.Invoke();
            }
        }

        /// <summary>
        /// GetPhotos function.
        /// get the photos from the output dir.
        /// </summary>
        public void GetPhotos()
        {
            try
            {
                if (m_outputDir == "")
                {
                    return;
                }
                string thumbnailDir = m_outputDir + "\\Thumbnails";
                if (!Directory.Exists(thumbnailDir))
                {
                    return;
                }
                DirectoryInfo di = new DirectoryInfo(thumbnailDir);
                //The only file types are relevant.
                string[] validExtensions = { ".jpg", ".png", ".gif", ".bmp" };
                foreach (DirectoryInfo yearDirInfo in di.GetDirectories())
                {
                    if (!Path.GetDirectoryName(yearDirInfo.FullName).EndsWith("Thumbnails"))
                    {
                        continue;
                    }
                    foreach (DirectoryInfo monthDirInfo in yearDirInfo.GetDirectories())
                    {


                        foreach (FileInfo fileInfo in monthDirInfo.GetFiles())
                        {
                            if (validExtensions.Contains(fileInfo.Extension.ToLower()))
                            {
                                Photo p = PhotosList.Find(x => (x.ImageUrl == fileInfo.FullName));
                                if (p == null)
                                {
                                    PhotosList.Add(new Photo(fileInfo.FullName));

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public class Photo
        {
            /// <summary>
            /// constructor.
            /// initialize new photo obj.
            /// </summary>
            /// <param name="imageUrl"></param>
            public Photo(string imageUrl)
            {
                try
                {
                    GetInfo(imageUrl);
                    GetPaths(imageUrl);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Photo - Photo: " + e);
                }
            }
            /// <summary>
            /// fills the basic info of the image - date and url
            /// </summary>
            /// <param name="imageUrl"></param> the URL of the image
            private void GetInfo(string imageUrl)
            {
                ImageUrl = imageUrl;
                ImageFullUrl = imageUrl.Replace(@"Thumbnails\", string.Empty);
                Name = Path.GetFileNameWithoutExtension(ImageUrl);
                Month = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(ImageUrl));
                Year = Path.GetFileNameWithoutExtension(Path.GetDirectoryName((Path.GetDirectoryName(ImageUrl))));
            }

            /// <summary>
            /// gets the relative path of the image
            /// </summary>
            /// <param name="imageUrl"></param>the URL of the image
            private void GetPaths(string imageUrl)
            {
                int location = imageUrl.IndexOf("target");
                int length = imageUrl.Length; ;
                string dirName = imageUrl.Substring(location, length - location);
                ImageRelativePathThumbnail = @"~\" + dirName;
                ImageRelativePath = ImageRelativePathThumbnail.Replace(@"Thumbnails\", string.Empty);
            }



            //members
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Year")]
            public string Year { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Month")]
            public string Month { get; set; }

            [Required]
            [DataType(DataType.ImageUrl)]
            [Display(Name = "ImageUrl")]
            public string ImageUrl { get; set; }

            [Required]
            [DataType(DataType.ImageUrl)]
            [Display(Name = "ImageRelativePath")]
            public string ImageRelativePathThumbnail { get; set; }

            [Required]
            [DataType(DataType.ImageUrl)]
            [Display(Name = "ImageRelativePath")]
            public string ImageRelativePath { get; set; }

            [Required]
            [DataType(DataType.ImageUrl)]
            [Display(Name = "ImageRelativePath")]
            public string ImageFullUrl { get; set; }

        }
    }
}

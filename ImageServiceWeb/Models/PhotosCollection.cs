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
        private static ConfigModel configuration;
        private string outputDir;
        public event ChangeNotifyer NotifyEvent;
        public List<Photo> PhotoList = new List<Photo>();

        /// <summary>
        /// creates an instance of PhotoCollection.
        /// </summary>
        public PhotosCollection()
        {
            configuration = new ConfigModel();
            configuration.Notify += Notify;
        }
        /// <summary>
        /// notify the controller that there was an update.
        /// </summary>
        void Notify()
        {
            if (configuration.OutputDir != "")
            {
                outputDir = configuration.OutputDir;
                ImportPhotos();
                NotifyEvent?.Invoke();
            }
        }

        /// <summary>
        /// import all the photos from the target directory
        /// </summary>
        public void ImportPhotos()
        {
            try
            {
                string thumbnailDir = outputDir + "\\Thumbnails";
                if (outputDir == "" || !Directory.Exists(thumbnailDir))
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
                        foreach (FileInfo info in monthDirInfo.GetFiles())
                        {
                            if (!validExtensions.Contains(info.Extension.ToLower()))
                            {
                                continue;
                            }
                            else
                            {
                                Photo p = PhotoList.Find(x => (x.ImageUrl == info.FullName));
                                if (p == null)
                                {
                                    PhotoList.Add(new Photo(info.FullName));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("PhotosCollection - ImportPhotos: " + e);
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

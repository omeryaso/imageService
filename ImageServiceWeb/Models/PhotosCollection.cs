﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using static ImageServiceWeb.Models.ConfigModel;

namespace ImageServiceWeb.Models
{
    public class PhotosCollection
    {
        public event NotifyAboutChange NotifyEvent;
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
            if (m_config.OutputDirectory != "")
            {
                m_outputDir = m_config.OutputDirectory;
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
    }
}
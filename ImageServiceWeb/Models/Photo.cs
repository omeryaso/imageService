﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
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
                ImageUrl = imageUrl;
                ImageFullUrl = imageUrl.Replace(@"Thumbnails\", string.Empty);
                Name = Path.GetFileNameWithoutExtension(ImageUrl);
                Month = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(ImageUrl));
                Year = Path.GetFileNameWithoutExtension(Path.GetDirectoryName((Path.GetDirectoryName(ImageUrl))));
                string strDirName;

                int intLocation, intLength;

                intLength = imageUrl.Length;
                intLocation = imageUrl.IndexOf("target");

                strDirName = imageUrl.Substring(intLocation, intLength- intLocation);

                ImageRelativePathThumbnail = @"~\" + strDirName;// Images\Thumbnails\" + Year + @"\" + Month + @"\" + Path.GetFileName(ImageUrl);
                ImageRelativePath = ImageRelativePathThumbnail.Replace(@"Thumbnails\",string.Empty);
            }
            catch (Exception ex)
            {

            }
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
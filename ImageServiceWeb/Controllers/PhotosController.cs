using  ImageServiceWeb.Models;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;

namespace  ImageServiceWeb.Controllers
{
    public class PhotosController : Controller
    {
        public static PhotosCollection photos = new PhotosCollection();
        private static PhotosCollection.Photo m_currentPhoto;

        /// <summary>
        /// constructor.
        /// </summary>
        public PhotosController()
        {
            photos.NotifyEvent -= () => { Photos(); };
            photos.NotifyEvent += () => { Photos(); };
        }

        // GET: Photos
        public ActionResult Photos()
        {
            photos.PhotoList.Clear();
            photos.ImportPhotos();
            return View(photos.PhotoList);
        }

        /// <summary>
        /// PhotosViewer.
        /// </summary>
        /// <param name="RelPath"> the photo in the web for view</param>
        /// <returns>PhotosViewer</returns>
        public ActionResult PhotosViewer(string RelPath)
        {
            UpdatePhotoPath(RelPath);
            return View(m_currentPhoto);
        }

        /// <summary>
        /// DeletePhoto function.
        /// </summary>
        /// <param name="photoRelPath"></param>
        /// <returns></returns>
        public ActionResult DeletePhoto(string photoRelPath)
        {
            UpdatePhotoPath(photoRelPath);
            return View(m_currentPhoto);
        }

        /// <summary>
        /// DeleteYes.
        /// </summary>
        /// <param name="photoRelPath"></param>
        /// <returns>DeleteYes</returns>
        public ActionResult DeleteYes(string photoRelPath)
        {
            try
            {
                System.IO.File.Delete(m_currentPhoto.ImageUrl);
                System.IO.File.Delete(m_currentPhoto.ImageFullUrl);
                photos.PhotoList.Remove(m_currentPhoto);
            } catch (Exception e)
            {
                Console.WriteLine("PhotosController - ActionResult: " + e);
            }
            return RedirectToAction("Photos");
        }

        /// <summary>
        /// UpdatePhotoPath.
        /// </summary>
        /// <param name="photoRelPath"></param>
        private void UpdatePhotoPath(string photoRelPath)
        {
            foreach (PhotosCollection.Photo photo in photos.PhotoList)
            {
                if (photo.ImageRelativePath != photoRelPath)
                {
                    continue;
                }
                    m_currentPhoto = photo;
                    break;
            }
        }
    }
}
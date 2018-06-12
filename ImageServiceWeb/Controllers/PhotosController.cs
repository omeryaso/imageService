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
            photos.NotifyEvent -= Notify;
            photos.NotifyEvent += Notify;

        }

        /// <summary>
        /// Notify function.
        /// notify view about update.
        /// </summary>
        void Notify()
        {
            Photos();
        }

        // GET: Photos
        public ActionResult Photos()
        {
            photos.PhotoList.Clear();
            photos.ImportPhotos();
            return View(photos.PhotoList);
        }

        /// <summary>
        /// \ function.
        /// </summary>
        /// <param name="photoRelPath"> the pic to be presented</param>
        /// <returns></returns>
        public ActionResult PhotosViewer(string photoRelPath)
        {
            UpdateCurrentPhotoFromRelPath(photoRelPath);
            return View(m_currentPhoto);
        }

        /// <summary>
        /// DeletePhoto function.
        /// </summary>
        /// <param name="photoRelPath"></param>
        /// <returns></returns>
        public ActionResult DeletePhoto(string photoRelPath)
        {
            UpdateCurrentPhotoFromRelPath(photoRelPath);
            return View(m_currentPhoto);
        }

        /// <summary>
        /// DeleteYes function.
        /// confirmation of the delete.
        /// </summary>
        /// <param name="photoRelPath"></param>
        /// <returns></returns>
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
        /// UpdateCurrentPhotoFromRelPath function.
        /// updates the current photo.
        /// </summary>
        /// <param name="photoRelPath"></param>
        private void UpdateCurrentPhotoFromRelPath(string photoRelPath)
        {
            foreach (PhotosCollection.Photo photo in photos.PhotoList)
            {
                if (photo.ImageRelativePath == photoRelPath)
                {
                    m_currentPhoto = photo;
                    break;
                }
            }
        }
    }
}
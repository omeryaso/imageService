using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ImageServiceWeb.Models.ImageWebModel;

namespace ImageServiceWeb.Models
{
    public class ImageWebController : Controller
    {
        static ImageWebModel ImageViewInfoObj = new ImageWebModel();
        /// <summary>
        /// constructor.
        /// </summary>
        public ImageWebController()
        {
            ImageViewInfoObj.ChangeNotifyer -= Notify;
            ImageViewInfoObj.ChangeNotifyer += Notify;

        }
        /// <summary>
        /// Notify function.
        /// notify view about update.
        /// </summary>
        void Notify()
        {
            ImageWeb();
        }

        // GET: ImageView
        public ActionResult ImageWeb()
        {
            ViewBag.NumofPics = ImageWebModel.GetNumOfPics();
            ViewBag.IsConnected = ImageViewInfoObj.IsConnected;
            return View(ImageViewInfoObj);
        }
        
    }
}
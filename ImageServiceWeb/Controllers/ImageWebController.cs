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
        /// ImageWebController constructor.
        /// </summary>
        public ImageWebController()
        {
            ImageViewInfoObj.ChangeNotifyer -= () => {ImageWeb();};
            ImageViewInfoObj.ChangeNotifyer += () => {ImageWeb();};

        }

        // GET: ImageView
        public ActionResult ImageWeb()
        {
            ViewBag.NumofPics = ImageWebModel.GetPicsNum();
            ViewBag.IsConnected = ImageViewInfoObj.IsConnected;
            return View(ImageViewInfoObj);
        }
        
    }
}
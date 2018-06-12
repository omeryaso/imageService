using ImageService.Modal;
using System;
using ImageServiceWeb.Models;
using System.Linq;
using ImageServiceWeb.Communication;
using System.Collections.Generic;
using ImageService.Infrastructure.Enums;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Web;

namespace ImageServiceWeb.Controllers
{
    public class ConfigController : Controller
    {
        static ConfigModel config = new ConfigModel();
        private static string m_handler;


        /// <summary>
        /// constructor.
        /// </summary>
        public ConfigController()
        {
            config.Notify += () => {Config();};
        }

        // GET: Confirm
        public ActionResult Confirm()
        {
            return View(config);
        }
        // GET: Config
        public ActionResult Config()
        {
            return View(config);
        }

        // GET: ConfigController/DeleteCancel/
        public ActionResult DeleteCancel()
        {
            return RedirectToAction("Config");
        }

        // GET: ConfigController/DeleteHandler
        public ActionResult DeleteHandler(string handler)
        {
            m_handler = handler;
            return RedirectToAction("Confirm");
        }

        // GET: ConfigController/DeleteOK/
        public ActionResult DeleteConfirm()
        {
            //delete the handler
            config.DeleteHandler(m_handler);
            System.Threading.Thread.Sleep(1000);
            return RedirectToAction("Config");

        }
    }
}
using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class LogsController : Controller
    {
        public static LogCollection log = new LogCollection();
        /// <summary>
        /// LogsController constructor.
        /// </summary>
        public LogsController()
        {
            log.Notify -= () => { Logs(); };
            log.Notify += () => { Logs(); };
        }

        // GET: Logs
        public ActionResult Logs()
        {
            return View(log.LogEntries);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult Logs(FormCollection collection)
        { 
            string type = collection["typeFilter"].ToString();
            if (type != "")
            {
                List<LogCollection.Log> filteredList = new List<LogCollection.Log>();
                foreach (LogCollection.Log log in log.LogEntries)
                {
                    if (log.Type == type)
                    {
                        filteredList.Add(log);
                    }
                }
                return View(filteredList);
            }
            return View(log.LogEntries);
        }
    }
}
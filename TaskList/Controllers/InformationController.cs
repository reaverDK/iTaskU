using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class InformationController : Controller
    {
        private InformationDataContext Infodb = new InformationDataContext();
        private IllnessDataContext illDB = new IllnessDataContext();

        object _lock = new object();

        // GET: Information
        public ActionResult Index()
        {
            lock (_lock)
            {

                var informations = Infodb.Informations.Where(i => i.Active);

                var illness = illDB.Illnesses.Where(i => i.StillSick);
                ViewBag.Ill = illness;

                return View(informations.ToList());
            }
            
        }

        [Authorize]
        public ActionResult CreateInformation()
        {
            return View();
        }

        [Authorize]
        public ActionResult ReportIll()
        {
            return View();
        }
    }
}
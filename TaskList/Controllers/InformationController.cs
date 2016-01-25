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

                return View(informations.ToList().OrderByDescending(x=>x.Id));
            }
            
        }

        [Authorize]
        public ActionResult DeactivateInformation(int id)
        {
            lock (_lock)
            {
                Information information = Infodb.Informations.FirstOrDefault(i => i.Id == id);
                information.Active = false;

                Infodb.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public ActionResult CreateInformation()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateInformation(string title, string content)
        {
            lock (_lock)
            {
                Information info = new Information();
                info.Active = true;
                info.Title = title;
                info.Content = content;
                info.Created = DateTime.Now;

                Infodb.Informations.InsertOnSubmit(info);
                Infodb.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult ReportIll()
        {
            return View();
        }

        public ActionResult FreshById(int id)
        {
            lock (_lock)
            {
                var person = illDB.Illnesses.FirstOrDefault(p => p.Id == id);
                if (person != null)
                {
                    person.StillSick = false;
                    person.EndDate = DateTime.Now;
                    illDB.SubmitChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult IsIll(string name)
        {
            Illness ill = new Illness();
            ill.Name = name;
            ill.StillSick = true;
            ill.StartDate = DateTime.Now;
            illDB.Illnesses.InsertOnSubmit(ill);
            illDB.SubmitChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult IsIll()
        {
            return View();
        }
    }
}
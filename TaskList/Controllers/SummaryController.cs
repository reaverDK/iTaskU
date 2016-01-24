using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskList.Models;

namespace TaskList.Controllers
{
	[Authorize]
    public class SummaryController : Controller
    {

		private SummaryListDataContext db = new SummaryListDataContext();
		object _lock = new object();

		// GET: Summary
		public ActionResult Index()
        {
			lock (_lock)
			{
				var tables = from t in db.Tables
							 orderby t.EntryDate
							 descending
							 select t;
				return View(tables.ToList());
			}
		}

		[Authorize]
		public ActionResult NewSummary(string taskTitle, string taskDescription, string taskAuthor)
		{
			lock (_lock)
			{
				//Add the new summary to database
				Table newTable = new Table();
				newTable.Title = taskTitle;
				newTable.Description = taskDescription;
				newTable.EntryDate = DateTime.Now;
				newTable.Author = taskAuthor;

				db.Tables.InsertOnSubmit(newTable);
				db.SubmitChanges();
			}

			return RedirectToAction("Index");
		}

		[Authorize]
		//Display a form for creating a new summary
		public ActionResult Create()
		{
			return View();
		}

		public ActionResult Cancel()
		{
			return RedirectToAction("Index");
		}

		[Authorize]
		//Adding a new summary to the database
		public ActionResult CreateNew(string taskTitle, string taskDescription, string taskAuthor)
		{
			lock (_lock)
			{
				//Add the new task to database
				Table newTable = new Table();
				newTable.Title = taskTitle;
				newTable.Description = taskDescription;
				newTable.Author = taskAuthor;
				newTable.EntryDate = DateTime.Now;

				db.Tables.InsertOnSubmit(newTable);
				db.SubmitChanges();
			}

			return RedirectToAction("Index");
		}
	}
}
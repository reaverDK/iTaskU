using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskList.Models; 


namespace TaskList.Controllers
{
	[Authorize]
    public class HomeController : Controller
    {
		private TaskListDataContext db = new TaskListDataContext();
		object _lock = new object();
				
		// GET: Home
		//Display a list of tasks
		public ActionResult Index()
        {

            lock (_lock)
			{
				var tasks = from t in db.Tasks
							orderby t.EntryDate
							descending
							select t;
				return View(tasks.ToList());
			}
        }

		//Display a form for creating a new task
		public ActionResult Create()
		{
			var member = new ApplicationDbContext();
			ViewBag.member = member.Users.ToList();
			return View();
		}

		public ActionResult Cancel()
		{
			return RedirectToAction("Index");
		}

		[Authorize]
		//Adding a new task to the database
		public ActionResult CreateNew(string taskTitle, string taskDescription, string taskAuthor)
		{
			lock (_lock)
			{
				//Add the new task to database
				Task newTask = new Task();
				newTask.Task1 = taskTitle;
				newTask.Description = taskDescription;
				newTask.Author = taskAuthor;
				newTask.IsCompleted = false;
				newTask.IsBegun = false;
				newTask.EntryDate = DateTime.Now;

				db.Tasks.InsertOnSubmit(newTask);
				db.SubmitChanges();
			}
			 
			return RedirectToAction("Index");
		}

		[Authorize]
		//Mark a task as complete
		public ActionResult Complete(int id)
		{
			lock (_lock)
			{
				//Database Logic
				var tasks = from t in db.Tasks where t.Id == id select t;
				foreach (Task match in tasks)
					match.IsCompleted = true;

				db.SubmitChanges();
			}

			return RedirectToAction("Index");
		}

		[Authorize]
		public ActionResult Begin(int id)
		{
			lock (_lock)
			{
				//Database Logic
				var tasks = from t in db.Tasks where t.Id == id select t;
				foreach (Task match in tasks)
					match.IsBegun = true;

				db.SubmitChanges();
			}

			return RedirectToAction("Index");
		}
	}
}
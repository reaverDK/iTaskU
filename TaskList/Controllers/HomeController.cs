using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskList.Models;
using Microsoft.AspNet.Identity;


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
			var member = new ApplicationDbContext();
			ViewBag.member = member.Users.ToList();

			lock (_lock)
			{
				var tasks = from t in db.Tasks
							orderby t.Priority
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
		[HttpPost]
		//Adding a new task to the database
		public ActionResult CreateNew(string taskTitle, string taskDescription, int taskPrioritet, string taskAuthor)
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
				newTask.IsActive = true;
				newTask.EntryDate = DateTime.Now;
			    newTask.Priority = taskPrioritet;


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
				{
					match.IsCompleted = true;
					match.IsActive = true;
					match.EndDate = DateTime.Now;
				}
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
				{
					match.IsBegun = true;
					match.IsActive = true;
				}

				db.SubmitChanges();
			}
			return RedirectToAction("Index");
		}

		[Authorize]
		public ActionResult Restart(int id)
		{
			lock (_lock)
			{
				//Database Logic
				var tasks = from t in db.Tasks where t.Id == id select t;
				foreach (Task match in tasks) {
					match.IsBegun = false;
					match.IsCompleted = false;
					match.IsActive = true;
					match.EntryDate = DateTime.Now;
                }

				db.SubmitChanges();
			}
			return RedirectToAction("Index");
		}

		//Adding a comment to the finished task and to the database
		[Authorize]
		[HttpPost]
		public ActionResult AddComment(int id, string comment)
		{
			lock (_lock)
			{
				
				Task task = db.Tasks.FirstOrDefault(i => i.Id == id);
				task.finalComment = comment;

				db.SubmitChanges();
			}
			return RedirectToAction("Index");
		}

		[Authorize]
		public ActionResult Comment(int id)
		{
			Task task = db.Tasks.FirstOrDefault(i => i.Id == id);
			ViewBag.id = task.Id;

			return View();
		}

		public ActionResult Show(string id)
		{
			lock (_lock)
			{
				//Database Logic
				var tasks = from t in db.Tasks where t.Author == id select t;
				foreach (Task match in tasks)
				{
					return RedirectToAction("Index");
				}
				
			}	
		return RedirectToAction("Index");
		}

		[Authorize]
		public ActionResult Delete(int id)
		{
			lock (_lock)
			{
				Task task = db.Tasks.FirstOrDefault(i => i.Id == id);
				task.IsActive = false;

				db.SubmitChanges();
			}
			return RedirectToAction("Index");
		}
	}
}